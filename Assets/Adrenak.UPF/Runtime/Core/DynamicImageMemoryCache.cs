﻿using System;
using System.Collections;
using System.Threading.Tasks;

using UnityEngine;

using Adrenak.Unex;
using System.Collections.Generic;
using System.Linq;

namespace Adrenak.UPF {
    public class DynamicImageMemoryCache : Cache<Texture2D> {
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

        Dictionary<CachingKey, List<DynamicImage>> instances = new Dictionary<CachingKey, List<DynamicImage>>();
        Dictionary<CachingKey, Texture2D> resources = new Dictionary<CachingKey, Texture2D>();

        /// <summary>
        /// <see cref="obj"/> is not used
        /// </summary>
        public override Task Init(object obj = null) {
            Runnable.Init();
            return Task.CompletedTask;
        }

        /// <summary>
        /// <see cref="obj"/> should contain the image path, compression and instance. In that order
        /// </summary>
        public override void Get(object obj, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            var args = obj as object[];
            var path = (args)[0] as string;
            var compression = (DynamicImage.Compression)(args)[1];
            var instance = (DynamicImage)(args)[2];

            var cachingKey = new CachingKey { path = path, compression = compression };

            // Check if the cached resources have this key
            foreach (var key in resources.Keys) {
                // If the key exists in resources, we just send back the cached texture
                // And if the requesting DynamicImage instance is new, we add it
                if (key.Equals(cachingKey)) {
                    var tex = resources[key];
                    onSuccess?.Invoke(tex);

                    if (!instances[key].Contains(instance))
                        instances[key].Add(instance);

                    return;
                }
            }

            // If the key requested is new, we need to fetch the texture,
            // add it to the cache, and start a new instance entry
            var cacheKey = new CachingKey { path = path, compression = compression };
            Runnable.Run(DownloadSpriteCo(path, compression,
                result => {
                    instances.Add(cacheKey, new List<DynamicImage> { instance });
                    resources.Add(cacheKey, result);
                    onSuccess?.Invoke(result);
                },
                onFailure
            ));
        }

        /// <summary>
        /// <see cref="obj"/> should contain the image path, compression and instance. In that order
        /// </summary>
        public override Task<Texture2D> Get(object obj) {
            var source = new TaskCompletionSource<Texture2D>();
            Get(obj,
                result => source.SetResult(result),
                exception => source.SetException(exception)
            );
            return source.Task;
        }

        /// <summary>
        /// <see cref="obj"/> should contain the image path, compression and instance. In that order
        /// </summary>
        public override void Free(object obj, Action onSuccess, Action<Exception> onFailure) {
            try {
                var args = obj as object[];
                var path = args[0] as string;
                var compression = (DynamicImage.Compression)(args)[1];
                var instance = (DynamicImage)(args)[2];

                var cachingKey = new CachingKey { path = path, compression = compression };

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

        /// <summary>
        /// <see cref="obj"/> should contain the image path, compression and instance. In that order
        /// </summary>
        public override Task Free(object obj) {
            var source = new TaskCompletionSource<bool>();
            Free(obj,
                () => source.SetResult(true),
                exception => source.SetException(exception)
            );
            return source.Task;
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
