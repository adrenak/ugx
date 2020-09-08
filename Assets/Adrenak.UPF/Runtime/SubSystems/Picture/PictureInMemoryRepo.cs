using System;
using System.Threading.Tasks;

using UnityEngine;

using Adrenak.Unex;
using System.Collections.Generic;
using System.Linq;

namespace Adrenak.UPF {
    public class PictureInMemoryRepo : PictureRepository {
        class Key {
            public string path;
            public Texture2DCompression compression;

            public Key(string path, Texture2DCompression compression) {
                this.path = path;
                this.compression = compression;
            }
            #region OVERRIDES
            public override bool Equals(object obj) {
                return obj is Key key &&
                       path == key.path &&
                       compression == key.compression;
            }

            public override int GetHashCode() {
                int hashCode = -770161765;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(path);
                hashCode = hashCode * -1521134295 + compression.GetHashCode();
                return hashCode;
            }
            #endregion
        }

        class Request {
            public Action<Texture2D> onSuccess;
            public Action<Exception> onFailure;

            public Request(Action<Texture2D> _onSuccess, Action<Exception> _onFailure) {
                onSuccess = _onSuccess;
                onFailure = _onFailure;
            }
        }

        List<Key> unused = new List<Key>();
        Dictionary<Key, List<Picture>> instances = new Dictionary<Key, List<Picture>>();
        Dictionary<Key, Texture2D> resources = new Dictionary<Key, Texture2D>();
        Dictionary<Key, List<Request>> requests = new Dictionary<Key, List<Request>>();
        int maxResourceCount;

        public PictureInMemoryRepo(int maxCount) {
            maxResourceCount = maxCount;
        }

        public override Task Init(object obj = null) {
            return Task.CompletedTask;
        }

        public override void Get(string location, Texture2DCompression compression, Picture instance, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            var key = new Key(location, compression);

            if (resources.Keys.Contains(key)) {
                unused.EnsureDoesntExist(key);
                instances[key].EnsureExists(instance);

                var tex = resources[key];
                onSuccess?.Invoke(tex);
                return;
            }

            var request = new Request(onSuccess, onFailure);
            requests.EnsureKey(key, new List<Request>());
            requests[key].Add(request);
            if (requests[key].Count > 1)
                return;

            Downloader.Download(location, compression,
                result => {
                    unused.EnsureDoesntExist(key);
                    resources.EnsureKey(key, result);
                    instances.EnsureKey(key, new List<Picture>());
                    instances[key].Add(instance);

                    foreach (var req in requests[key])
                        req.onSuccess?.Invoke(result);

                    requests[key].Clear();
                    requests.Remove(key);

                    if (instance != null)
                        onSuccess?.Invoke(result);
                },
                error => {
                    requests[key].ForEach(x => x.onFailure?.Invoke(error));
                    requests[key].Clear();
                    requests.Remove(key);

                    if (instance != null)
                        onFailure?.Invoke(error);
                }
            );
        }

        public override Task<Texture2D> Get(string location, Texture2DCompression compression, Picture instance) {
            var source = new TaskCompletionSource<Texture2D>();
            Get(location, compression, instance,
                result => source.SetResult(result),
                exception => source.SetException(exception)
            );
            return source.Task;
        }

        public override void Free(string location, Texture2DCompression compression, Picture instance, Action onSuccess, Action<Exception> onFailure) {
            try {
                var k = new Key(location, compression);

                for (int i = 0; i < instances.Keys.Count; i++) {
                    var key = instances.Keys.ToList()[i];

                    if (key.Equals(k)) {
                        instances[key].Remove(instance);

                        if (instances[key].Count <= 0) {
                            unused.EnsureExists(key);

                            var unusedKeys = instances.Where(x => x.Value.Count <= 0).Select(x => x.Key).ToList();
                            while (resources.Count > maxResourceCount && unusedKeys.Count > 0) {
                                var oldestUnused = unused[0];
                                instances[oldestUnused].Clear();
                                instances.Remove(oldestUnused);

                                MonoBehaviour.Destroy(resources[oldestUnused]);
                                resources.Remove(oldestUnused);
                                unused.EnsureDoesntExist(oldestUnused);
                            }
                        }
                    }
                }
                onSuccess?.Invoke();
            }
            catch (Exception e) {
                onFailure?.Invoke(e);
            }
        }

        public override Task Free(string location, Texture2DCompression compression, Picture instance) {
            var source = new TaskCompletionSource<bool>();
            Free(location, compression, instance,
                () => source.SetResult(true),
                exception => source.SetException(exception)
            );
            return source.Task;
        }
    }
}
