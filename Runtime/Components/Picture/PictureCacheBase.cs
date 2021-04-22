using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    public abstract class PictureCacheBase {
        public abstract UniTask Init(object obj = null);

        public abstract void Get(string location, Texture2DCompression compression, Picture instance, Action<Texture2D> onSuccess, Action<Exception> onFailure);
        public abstract UniTask<Texture2D> Get(string location, Texture2DCompression compression, Picture instance);

        public abstract void Free(string location, Texture2DCompression compression, Picture instance, Action onSuccess, Action<Exception> onFailure);
        public abstract UniTask Free(string location, Texture2DCompression compression, Picture instance);

        static Texture2DDownloader downloader;
        public static bool DownloaderLocked => downloader != null;
        public static Texture2DDownloader Downloader {
            get {
                if (downloader == null)
                    downloader = Texture2DDownloader.New();
                return downloader;
            }
            set {
                if (downloader != null)
                    throw new Exception("DynamicImage.Downloader can only be set once and before any get calls");
                if (value == null)
                    throw new Exception("DynamicImage.Downloader cannot be set to null!");
                downloader = value;
            }
        }
    }
}