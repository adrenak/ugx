using Adrenak.Unex;

using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// A utility MonoBehaviour to initialize UGX easily. Drag and drop in the scene,
    /// the first scene of the game that loads on application start is preferred.
    /// </summary>
    public class UGXInitializer : MonoBehaviour {
        /// <summary>
        /// The number of sprites the Picture cache will hold. The cache can overflow when
        /// more sprites are required, but once the requirement goes down, the cache will
        /// start to free up.
        /// </summary>
        [Tooltip("The number of sprites the Picture cache will hold. The cache can overflow when" +
        "more sprites are required, but once the requirement goes down, the cache will" +
        "start to free up.")]
        public int pictureCacheSize = 50;

        /// <summary>
        /// The maximum number of downloads that may occure simultaneously for Picture.
        /// </summary>
        [Tooltip("The maximum number of downloads that may occure simultaneously for Picture.")]
        public int maxConcurrentPictureDownloads = 5;

        /// <summary>
        /// The minumum time gap between two consecutive texture load. Settings a low value 
        /// may allow too many textures being loaded at once, causing major jitters.
        /// Setting large values may cause the Pictures to be shown late. Recommended 1f - 1.5f
        /// </summary>
        [Tooltip("The minumum time gap between two consecutive texture load. Settings a low value " +
        "may allow too many textures being loaded at once, causing major jitters." +
        "Setting large values may cause the Pictures to be shown late. Recommended 1f - 1.5f")]
        public float pictureTextureLoadPeriod = .125f;

        static UGXInitializer instance;

        void Awake() {
            Init();
        }

        static void EnsureInstance() {
            if (instance == null)
                instance = FindObjectOfType<UGXInitializer>();
            if (instance == null)
                instance = new GameObject("UGX Initializer").AddComponent<UGXInitializer>();
        }

        public static void Init() {
            UnexInitializer.Initialize();
            EnsureInstance();

            var cache = new PictureMemoryCache(instance.pictureCacheSize);
            PictureCacheBase.Downloader = Texture2DDownloader.New(instance.maxConcurrentPictureDownloads, instance.pictureTextureLoadPeriod);
            Picture.Cache = new PictureMemoryCache(50);
        }
    }
}
