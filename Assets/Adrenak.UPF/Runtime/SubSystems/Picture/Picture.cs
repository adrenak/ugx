using System;

using UnityEngine;
using UnityEngine.UI;

using Adrenak.Unex;
using UnityEngine.Events;

namespace Adrenak.UPF {
    [Serializable]
    public class Picture : Image {
        // Ambient context dependency pattern
        static PictureRepository repo;
        public static bool RepoLocked => repo != null;
        public static PictureRepository Repo {
            get {
                if (repo == null)
                    repo = new PictureStatelessRepo();
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
        RectTransform RT {
            get {
                if (rt == null)
                    rt = GetComponent<RectTransform>();
                return rt;
            }
        }

        public Visibility CurrentVisibility { get; private set; } = Visibility.None;

        int age = 0;

        protected override void Awake() {
            Clear();
        }

        protected override void Start() {
            rt = GetComponent<RectTransform>();
            if (loadOnStart && Application.isPlaying)
                Refresh();
        }

        [ContextMenu("Refresh")]
        public void Refresh() {
            if (!Application.isPlaying) return;

            Repo.Free(oldPath, oldCompression, this);

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
                        Repo.Get(
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

        void Update() {
            age++;
            if (age <= 1) return;

            var visibility = GetVisibility();
            if (CurrentVisibility != visibility) {
                if (CurrentVisibility == Visibility.None && visibility != Visibility.None)
                    Refresh();
                else if (CurrentVisibility != Visibility.None && visibility == Visibility.None)
                    Repo.Free(path, compression, this);

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

        Visibility GetVisibility() {
            if (RT.IsVisible(out bool? fully))
                return fully.Value ? Visibility.Full : Visibility.Partial;
            else
                return Visibility.None;
        }

        bool destroyed = false;
        protected override void OnDestroy() {
            destroyed = true;
            if (!Application.isPlaying) return;
            Repo.Free(path, compression, this);
        }
    }
}
