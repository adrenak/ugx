using System;
using System.Threading.Tasks;

using UnityEngine;

using Adrenak.Unex;
using System.Collections.Generic;
using System.Linq;

namespace Adrenak.UPF {
    public class DynamicImageInMemoryRepo : DynamicImageRepository {
        class CachingKey {
            public string path;
            public DynamicImage.Compression compression;

            public override bool Equals(object obj) {
                return obj is CachingKey key &&
                       path == key.path &&
                       compression == key.compression;
            }

            public override int GetHashCode() {
                int hashCode = -770161765;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(path);
                hashCode = hashCode * -1521134295 + compression.GetHashCode();
                return hashCode;
            }
        }

        class Request {
            public Action<Texture2D> onSuccess;
            public Action<Exception> onFailure;

            public Request(Action<Texture2D> _onSuccess, Action<Exception> _onFailure) {
                onSuccess = _onSuccess;
                onFailure = _onFailure;
            }
        }

        Dictionary<CachingKey, List<DynamicImage>> instances = new Dictionary<CachingKey, List<DynamicImage>>();
        Dictionary<CachingKey, Texture2D> resources = new Dictionary<CachingKey, Texture2D>();
        Dictionary<CachingKey, List<Request>> pendingRequests = new Dictionary<CachingKey, List<Request>>();

        /// <summary>
        /// <see cref="obj"/> is not used
        /// </summary>
        public override Task Init(object obj = null) {
            return Task.CompletedTask;
        }

        public override void Get(string location, DynamicImage.Compression compression, DynamicImage instance, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            var cachingKey = new CachingKey { path = location, compression = compression };

            if (resources.Keys.Contains(cachingKey)) {
                var tex = resources[cachingKey];
                onSuccess?.Invoke(tex);

                if (!instances[cachingKey].Contains(instance))
                    instances[cachingKey].Add(instance);

                return;
            }

            if(!pendingRequests.ContainsKey(cachingKey))
                pendingRequests.Add(cachingKey, new List<Request>());
            var request = new Request(onSuccess, onFailure);
            pendingRequests[cachingKey].Add(request);

            if (pendingRequests[cachingKey].Count > 1) {
                return;
            }

            // If the key requested is new, we need to fetch the texture,
            // add it to the cache, and start a new instance entry
            Runnable.Run(DownloadSpriteCo(location, compression,
                result => {
                    resources.Add(cachingKey, result);
                    instances.Add(cachingKey, new List<DynamicImage> { instance });

                    foreach(var req in pendingRequests[cachingKey])
                        req.onSuccess?.Invoke(result);

                    pendingRequests[cachingKey].Remove(request);
                },
                error => {
                    foreach (var req in pendingRequests[cachingKey])
                        req.onFailure?.Invoke(error);
                    pendingRequests[cachingKey].Remove(request);
                }
            ));
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
                var cachingKey = new CachingKey { path = location, compression = compression };

                for (int i = 0; i < instances.Keys.Count; i++) {
                    var key = instances.Keys.ToList()[i];

                    if (key.Equals(cachingKey)) {
                        // Find the matching key and remove the instance
                        instances[key].Remove(instance);

                        // If after removing the instance, the instance count is 0
                        // then we destroy the texture associated with the key
                        // and basically go back to as if that texture was never requested.
                        if (instances[key].Count <= 0) {
                            MonoBehaviour.Destroy(resources[key]);
                            resources.Remove(key);

                            instances[key].Clear();
                            instances.Remove(key);

                            break;
                        }
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
