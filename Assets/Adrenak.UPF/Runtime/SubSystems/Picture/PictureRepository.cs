using System;
using UnityEngine;
using System.Threading.Tasks;

namespace Adrenak.UPF {
    public abstract class PictureRepository {
        public abstract Task Init(object obj = null);

        public abstract void Get(string location, Texture2DCompression compression, Picture instance, Action<Texture2D> onSuccess, Action<Exception> onFailure);
        public abstract Task<Texture2D> Get(string location, Texture2DCompression compression, Picture instance);

        public abstract void Free(string location, Texture2DCompression compression, Picture instance, Action onSuccess, Action<Exception> onFailure);
        public abstract Task Free(string location, Texture2DCompression compression, Picture instance);

        static ImageDownloader downloader;
        public static bool DownloaderLocked => downloader != null;
        public static ImageDownloader Downloader {
            get {
                if (downloader == null)
                    downloader = new ImageDownloader();
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