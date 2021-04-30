using System;
using Cysharp.Threading.Tasks;

using UnityEngine;

using System.Collections.Generic;
using System.Linq;
using Adrenak.Unex;

namespace Adrenak.UGX {
    public class PictureMemoryCache : PictureCacheBase {
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
            public Action<Sprite> onSuccess;
            public Action<Exception> onFailure;

            public Request(Action<Sprite> _onSuccess, Action<Exception> _onFailure) {
                onSuccess = _onSuccess;
                onFailure = _onFailure;
            }
        }

        List<Key> freed = new List<Key>();
        Dictionary<Key, List<Picture>> instances = new Dictionary<Key, List<Picture>>();
        Dictionary<Key, Sprite> resources = new Dictionary<Key, Sprite>();
        Dictionary<Key, List<Request>> requests = new Dictionary<Key, List<Request>>();
        int maxResourceCount;

        public PictureMemoryCache(int maxCount) {
            maxResourceCount = maxCount;
        }

        public override UniTask Init(object obj = null) {
            return UniTask.CompletedTask;
        }
        
        public override void Get(string location, Texture2DCompression compression, Picture instance, Action<Sprite> onSuccess, Action<Exception> onFailure) {
            if (string.IsNullOrEmpty(location)) {
                onFailure?.Invoke(new ArgumentException("location cannot be null or empty", "location"));
                return;
            }
            var key = new Key(location, compression);
            
            if (resources.Keys.Contains(key)) {
                freed.EnsureDoesntContain(key);
                instances[key].EnsureContains(instance);

                onSuccess?.Invoke(resources[key]);
                return;
            }

            var request = new Request(onSuccess, onFailure);
			requests.EnsureContains(key, new List<Request>() { });
            requests[key].Add(request);
            if (requests[key].Count > 1)
                return;

            Downloader.Download(location,
                result => {
                    if (compression != Texture2DCompression.None)
                        result.Compress(compression == Texture2DCompression.HighQuality);
                    var resultSprite = result.ToSprite();

                    freed.EnsureDoesntContain(key);
                    resources.EnsureContains(key, resultSprite);
                    instances.EnsureContains(key, new List<Picture>());
                    instances[key].Add(instance);

                    foreach (var req in requests[key])
                        req.onSuccess?.Invoke(resultSprite);

                    requests[key].Clear();
                    requests.Remove(key);

                    if (instance != null)
                        onSuccess?.Invoke(resultSprite);
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

        public override UniTask<Sprite> Get(string location, Texture2DCompression compression, Picture instance) {
            var source = new UniTaskCompletionSource<Sprite>();
            Get(location, compression, instance,
                result => source.TrySetResult(result),
                exception => source.TrySetException(exception)
            );
            return source.Task;
        }

        public override void Free(string location, Texture2DCompression compression, Picture instance, Action onSuccess, Action<Exception> onFailure) {
            if (string.IsNullOrEmpty(location)) return;

            try {
                var k = new Key(location, compression);

                for (int i = 0; i < instances.Keys.Count; i++) {
                    var key = instances.Keys.ToList()[i];

                    if (key.Equals(k)) {
                        instances[key].Remove(instance);

                        if (instances[key].Count <= 0) {
                            freed.EnsureContains(key);

                            var unusedKeys = instances.Where(x => x.Value.Count <= 0).Select(x => x.Key).ToList();
                            while (resources.Count > maxResourceCount && unusedKeys.Count > 0) {
                                var oldestFreed = freed[0];
                                instances[oldestFreed].Clear();
                                instances.Remove(oldestFreed);

                                MonoBehaviour.Destroy(resources[oldestFreed]);
                                resources.Remove(oldestFreed);
                                freed.EnsureDoesntContain(oldestFreed);
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

        public override UniTask Free(string location, Texture2DCompression compression, Picture instance) {
            var source = new UniTaskCompletionSource<bool>();
            Free(location, compression, instance,
                () => source.TrySetResult(true),
                exception => source.TrySetException(exception)
            );
            return source.Task;
        }
    }
}
