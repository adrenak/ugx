using System;

using UnityEngine;
using UnityEngine.UI;

using Adrenak.Unex;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Adrenak.UGX {
    [Serializable]
    public class Picture : Image {
        // Ambient context dependency pattern
        static PictureCacheBase repo;
        public static bool CacheLocked => repo != null;
        public static PictureCacheBase Cache {
            get {
                if (repo == null)
                    repo = new PictureStatelessCache();
                return repo;
            }
            set {
                if (repo != null)
                    throw new Exception("DynamicImage.Cache can only be set once and before any get calls");

                if (value == null)
                    throw new Exception("DynamicImage.Cache cannot set Cache to null!");

                repo = value;
            }
        }

        public enum Source {
            Asset,
            Resource,
            URL
        }

        public UnityEvent onSpriteSet;
        public UnityEvent onSpriteRemoved;

        public Source source = Source.URL;

        Texture2DCompression oldCompression = Texture2DCompression.None;
        public Texture2DCompression compression = Texture2DCompression.LowQuality;

        string oldPath = string.Empty;
        public string path = string.Empty;

        public bool loadOnStart = true;

        RectTransform rt;
        RectTransform RT => rt == null ? rt = GetComponent<RectTransform>() : rt;

        public ViewVisibility CurrentVisibility { get; private set; } = ViewVisibility.None;

        protected override void Awake() {
            Clear();
        }

        protected override void Start() {
            rt = GetComponent<RectTransform>();
            if (loadOnStart && Application.isPlaying)
                Refresh();
        }

        [Button("Refresh")]
        public void Refresh() {
            if (!Application.isPlaying) return;

            Cache.Free(oldPath, oldCompression, this);

            switch (source) {
                case Source.Resource:
                    if (string.IsNullOrWhiteSpace(path))
                        break;

                    var resourceSprite = Resources.Load<Sprite>(path);
                    if (resourceSprite == null) {
                        Debug.LogError($"Not Resource found at {path}");
                        break;
                    }

                    SetSprite(resourceSprite);
                    break;

                case Source.URL:
                    if (string.IsNullOrWhiteSpace(path))
                        break;

                    try {
                        Cache.Get(
                            path, compression, this,
                            result => SetSprite(result.ToSprite()),
                            error => Debug.LogError($"Dynamic Image Refresh from remote path failed: " + error)
                        );
                    }
                    catch (Exception e) {
                        Debug.LogError(e);
                    }
                    break;
            }

            oldPath = path;
            oldCompression = compression;
        }

        int age = 0;
        void Update() {
            age++;
            if (age <= 1) return;

            var visibility = GetVisibility();
            if (CurrentVisibility != visibility) {
                if (CurrentVisibility == ViewVisibility.None && visibility != ViewVisibility.None)
                    Refresh();
                else if (CurrentVisibility != ViewVisibility.None && visibility == ViewVisibility.None)
                    Cache.Free(path, compression, this);

                CurrentVisibility = visibility;
            }
        }

        public void Clear() {
            sprite = null;
            onSpriteRemoved?.Invoke();
        }

        void SetSprite(Sprite s) {
            if (s != null && !destroyed) {
                sprite = s;
                onSpriteSet?.Invoke();
            }
        }

        ViewVisibility GetVisibility() {
            if (RT.IsVisible(out bool? fully))
                return fully.Value ? ViewVisibility.Full : ViewVisibility.Partial;
            else
                return ViewVisibility.None;
        }

        bool destroyed = false;
        protected override void OnDestroy() {
            destroyed = true;
            if (!Application.isPlaying) return;
            Cache.Free(path, compression, this);
        }
    }
}
