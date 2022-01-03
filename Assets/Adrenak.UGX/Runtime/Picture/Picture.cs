using System;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Events;

namespace Adrenak.UGX {
    [Serializable]
    public class Picture : Image {
        // Ambient context dependency pattern
        static PictureCacheBase repo;
        public static bool CacheLocked => repo != null;
        public static PictureCacheBase Cache {
            get {
                if (repo == null)
                    repo = new PictureMemoryCache(50);
                return repo;
            }
            set {
                if(repo == null)
                    throw new Exception("Cannot set Cache to null!");

                if (repo != null)
                    throw new Exception("DynamicImage.Cache can only be set " +
                    "once and before any get calls");
                repo = value;
            }
        }

        public UnityEvent onLoadStart;
        public UnityEvent onLoadSuccess;
        public UnityEvent onLoadFailure;

        public GameObject statusRootObj;
        public GameObject loaderObj;
        public GameObject errorObj;

        public bool refreshOnStart;
        public bool updateWhenOffScreen;

        Texture2DCompression oldCompression = Texture2DCompression.None;
        public Texture2DCompression compression = Texture2DCompression.None;

        string oldPath = string.Empty;
        public string path = string.Empty;

        RectTransform rt;
        RectTransform RT => rt ?? (rt = GetComponent<RectTransform>());

        Visibility currentVisibility = Visibility.None;
        public Visibility CurrentVisibility => currentVisibility;

        protected override void Start() {
            rt = GetComponent<RectTransform>();
            if (refreshOnStart)
                Refresh();
        }

        [ContextMenu("Refresh")]
        public void Refresh() {
            if (!Application.isPlaying)
                return;

            if (currentVisibility == Visibility.None && !updateWhenOffScreen)
                return;

            if (compression == oldCompression && path == oldPath)
                return;

            try {
                Cache.Free(oldPath, oldCompression, this);
                sprite = null;

                SetStatusObjects(true, false);
                onLoadStart.Invoke();

                Cache.Get(
                    path, compression, this,
                    result => {
                        SetStatusObjects(false, false);

                        if (sprite == null || sprite.texture == null) {
                            SetSprite(result);
                            onLoadSuccess.Invoke();
                            return;
                        }
                        if (sprite != null 
                            && sprite.texture != null 
                            && result.texture != sprite.texture
                        ) {
                            SetSprite(result);
                            onLoadSuccess.Invoke();
                        }
                    },
                    error => {
                        SetStatusObjects(false, true);
                        Debug.LogError($"Refresh failed: " + error);
                        onLoadFailure.Invoke();
                    }
                );
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
            oldPath = path;
            oldCompression = compression;
        }

        void SetStatusObjects(bool loaderState, bool errorState) {
            statusRootObj?.SetActive(loaderState || errorState);
            loaderObj?.SetActive(loaderState);
            errorObj?.SetActive(errorState);
        }

        // NOTE: We ignore the first 2 frames because often times Pictures are
        // visible for a few frames on instantiation when used inside an 
        // AutoLayout, but are not supposed to be. Unity seems to update 
        // layouts in the next frame after they are enabled.
        int age = 0;
        void Update() {
            age++;
            if (age <= 2) return;

            var oldVisibility = currentVisibility;
            currentVisibility = RT.GetVisibility();

            if (currentVisibility != oldVisibility) {
                if (oldVisibility == Visibility.None && currentVisibility != Visibility.None)
                    Refresh();
                else if (oldVisibility != Visibility.None && currentVisibility == Visibility.None)
                    Cache.Free(path, compression, this);
            }
        }

        void SetSprite(Sprite s) {
            if (s != null && !destroyed) {
                sprite = s;
                onLoadSuccess?.Invoke();
            }
        }

        bool destroyed = false;
        protected override void OnDestroy() {
            base.OnDestroy();
            if (destroyed) return;
            destroyed = true;

            if (!Application.isPlaying) return;
            Cache.Free(path, compression, this);
        }
    }
}
