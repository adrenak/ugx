using System;

using UnityEngine;
using UnityEngine.UI;

using Adrenak.Unex;

namespace Adrenak.UPF {
    [Serializable]
    public class DynamicImage : Image {
        // Ambient context dependency pattern
        static DynamicImageRepository repo;
        public static DynamicImageRepository Repo {
            get {
                if (repo == null)
                    repo = new DynamicImageStatelessRepo();
                return repo;
            }
            set {
                if (repo != null)
                    throw new Exception("DynamicImage.Cache can only be set once and before any get calls");

                if (value == null)
                    throw new Exception("Can not set Cache to null!");
                repo = value;
            }
        }

        public enum Source {
            Resource,
            URL
        }

        public enum Compression {
            None,
            HighQuality,
            LowQuality
        }

        public Source source = Source.URL;

        Compression oldCompression = Compression.None;
        public Compression compression = Compression.LowQuality;

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

        protected override void Start() {
            rt = GetComponent<RectTransform>();

            if (loadOnStart && Application.isPlaying)
                Refresh();
        }

        [ContextMenu("Refresh")]
        public void Refresh() {
            if (!Application.isPlaying) return;
            if (oldPath.Equals(path) && oldCompression.Equals(oldCompression)) return;

            Repo.Free(oldPath, oldCompression, this);
            sprite = null;

            switch (source) {
                case Source.Resource:
                    if (string.IsNullOrWhiteSpace(path)) {
                        Debug.LogWarning("Source path is null or whitespace");
                        break;
                    }

                    var resourceSprite = Resources.Load<Sprite>(path);
                    if (resourceSprite == null) {
                        Debug.LogError($"Not Resource found at {path}");
                        break;
                    }

                    sprite = resourceSprite;
                    break;

                case Source.URL:
                    if (string.IsNullOrWhiteSpace(path)) {
                        Debug.LogWarning("Source path is null or whitespace");
                        break;
                    }

                    try {
                        Repo.Get(
                            path, compression, this,
                            result => sprite = result.ToSprite(),
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
                else if(CurrentVisibility != Visibility.None && visibility == Visibility.None)
                    Repo.Free(path, compression, this);

                CurrentVisibility = visibility;
            }
        }

        Visibility GetVisibility() {
            if (RT.IsVisible(out bool? fully))
                return fully.Value ? Visibility.Full : Visibility.Partial;
            else
                return Visibility.None;
        }

        protected override void OnDestroy() {
            if (!Application.isPlaying) return;

            Repo.Free(path, compression, this);
        }
    }
}
