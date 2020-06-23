using System;
using System.Collections;
using System.Threading.Tasks;

using UnityEngine;

using Adrenak.Unex;
using System.Collections.Generic;
using System.Linq;

namespace Adrenak.UPF {
    public class DynamicImageInMemoryCache : Cache<Texture2D> {
        public class CacheKey {
            public string path;
            public DynamicImage.Compression compression;
        }

        Dictionary<CacheKey, List<DynamicImage>> instances = new Dictionary<CacheKey, List<DynamicImage>>();
        Dictionary<CacheKey, Texture2D> cache = new Dictionary<CacheKey, Texture2D>();

        public override Task Init() {
            Runnable.Init();
            return Task.CompletedTask;
        }

        public override Task<Texture2D> Get(object obj) {
            var source = new TaskCompletionSource<Texture2D>();
            Get(obj,
                result => source.SetResult(result),
                exception => source.SetException(exception)
            );
            return source.Task;
        }

        public override void Get(object obj, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            var path = (obj as object[])[0] as string;
            var compression = (DynamicImage.Compression)(obj as object[])[1];
            var instance = (DynamicImage)(obj as object[])[2];

            foreach (var key in cache.Keys) {
                if (key.compression == compression && key.path == path) {
                    var tex = cache[key];
                    onSuccess?.Invoke(tex);

                    if (!instances[key].Contains(instance))
                        instances[key].Add(instance);

                    return;
                }
            }

            var cacheKey = new CacheKey { path = path, compression = compression };
            Runnable.Run(DownloadSpriteCo(path, compression,
                result => {
                    instances.Add(cacheKey, new List<DynamicImage> { instance });
                    cache.Add(cacheKey, result);
                    onSuccess?.Invoke(result);
                },
                onFailure
            ));
        }

        public override void Free(object obj) {
            var path = (obj as object[])[0] as string;
            var compression = (DynamicImage.Compression)(obj as object[])[1];
            var instance = (DynamicImage)(obj as object[])[2];

            for (int i = 0; i < instances.Keys.Count; i++) {
                var key = instances.Keys.ToList()[i];

                if (key.compression == compression && key.path == path) {
                    instances[key].Remove(instance);

                    if (instances[key].Count <= 0) {
                        MonoBehaviour.Destroy(cache[key]);
                        cache.Remove(key);

                        instances[key].Clear();
                        instances.Remove(key);
                    }
                }
            }
        }

        IEnumerator DownloadSpriteCo(string path, DynamicImage.Compression compression, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            var www = new WWW(path);
            yield return www;
            while (!www.isDone)
                yield return null;

            if (!string.IsNullOrWhiteSpace(www.error)) {
                onFailure?.Invoke(new Exception(www.error));
                yield break;
            }

            var tex = new Texture2D(2, 2);
            tex.LoadImage(www.bytes);
            tex.Apply();
            if (compression != DynamicImage.Compression.None)
                tex.Compress(compression == DynamicImage.Compression.HighQuality);

            onSuccess?.Invoke(tex);

            www.Dispose();
        }
    }
}
