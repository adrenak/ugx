using System;
using System.Threading.Tasks;

using UnityEngine;

using Adrenak.Unex;
using System.Collections.Generic;
using System.Linq;

namespace Adrenak.UPF {
    public class DynamicImageInMemoryRepo : DynamicImageRepository {
        class Key {
            public string path;
            public DynamicImage.Compression compression;

            public Key(string path, DynamicImage.Compression compression) {
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

        Dictionary<Key, List<DynamicImage>> instances = new Dictionary<Key, List<DynamicImage>>();
        Dictionary<Key, Texture2D> resources = new Dictionary<Key, Texture2D>();
        Dictionary<Key, List<Request>> requests = new Dictionary<Key, List<Request>>();
        int maxResourceCount;

        public DynamicImageInMemoryRepo(int maxCount) {
            maxResourceCount = maxCount;
        }

        public override Task Init(object obj = null) {
            return Task.CompletedTask;
        }

        public override void Get(string location, DynamicImage.Compression compression, DynamicImage instance, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            var key = new Key(location, compression);

            if (resources.Keys.Contains(key)) {
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

            DownloadSprite(location, compression,
                result => {
                    resources.EnsureKey(key, result);
                    instances.EnsureKey(key, new List<DynamicImage>());
                    instances[key].Add(instance);

                    foreach (var req in requests[key])
                        req.onSuccess?.Invoke(result);
                    requests[key].Remove(request);

                    onSuccess?.Invoke(result);
                },
                error => {
                    requests[key].ForEach(x => x.onFailure?.Invoke(error));
                    requests[key].Remove(request);
                    onFailure?.Invoke(error);
                }
            );
        }

        public override Task<Texture2D> Get(string location, DynamicImage.Compression compression, DynamicImage instance) {
            var source = new TaskCompletionSource<Texture2D>();
            Get(location, compression, instance,
                result => source.SetResult(result),
                exception => source.SetException(exception)
            );
            return source.Task;
        }

        public override void Free(string location, DynamicImage.Compression compression, DynamicImage instance, Action onSuccess, Action<Exception> onFailure) {
            try {
                var key = new Key(location, compression);

                for (int i = 0; i < instances.Keys.Count; i++) {
                    var k = instances.Keys.ToList()[i];

                    if (k.Equals(key)) {
                        // Find the matching key and remove the instance
                        instances[k].Remove(instance);

                        //if (instances[k].Count <= 0) {
                            var unusedKeys = instances.Where(x => x.Value.Count <= 0).Select(x => x.Key).ToList();

                            while (resources.Count > maxResourceCount && 
                            unusedKeys.Count > 0) {
                                //&& unused.Count > 0) {
                                var oldestUnused = unusedKeys[0];
                                instances[oldestUnused].Clear();
                                instances.Remove(oldestUnused);

                                MonoBehaviour.Destroy(resources[oldestUnused]);
                                resources.Remove(oldestUnused);
                            }
                        //}
                    }
                }
                onSuccess?.Invoke();
            }
            catch (Exception e) {
                onFailure?.Invoke(e);
            }
        }

        public override Task Free(string location, DynamicImage.Compression compression, DynamicImage instance) {
            var source = new TaskCompletionSource<bool>();
            Free(location, compression, instance,
                () => source.SetResult(true),
                exception => source.SetException(exception)
            );
            return source.Task;
        }
    }
}
