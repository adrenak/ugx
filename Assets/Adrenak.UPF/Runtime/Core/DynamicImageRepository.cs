using System;
using UnityEngine;
using Adrenak.Unex;
using System.Collections;
using System.Threading.Tasks;

namespace Adrenak.UPF {
    public abstract class DynamicImageRepository {
        public abstract Task Init(object obj = null);

        public abstract void Get(string location, DynamicImage.Compression compression, DynamicImage instance, Action<Texture2D> onSuccess, Action<Exception> onFailure);

        public abstract Task<Texture2D> Get(string location, DynamicImage.Compression compression, DynamicImage instance);

        public abstract void Free(string location, DynamicImage.Compression compression, DynamicImage instance, Action onSuccess, Action<Exception> onFailure);

        public abstract Task Free(string location, DynamicImage.Compression compression, DynamicImage instance);

        protected void DownloadSprite(string path, DynamicImage.Compression compression, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            Runnable.Run(DownloadSpriteCo(path, compression, onSuccess, onFailure));
        }

        protected IEnumerator DownloadSpriteCo(string path, DynamicImage.Compression compression, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
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