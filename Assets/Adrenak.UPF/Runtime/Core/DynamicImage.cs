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
                    repo = new DynamicImageFakeRepo();
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

        Compression oldCompression;
        public Compression compression = Compression.LowQuality;

        string oldPath;
        public string path = string.Empty;

        public bool loadOnStart = true;

        protected override void Start() {
            if (loadOnStart)
                Refresh();
        }

        [ContextMenu("Refresh")]
        public void Refresh() {
            if (!Application.isPlaying) return;

            Repo.Free(oldPath, oldCompression, this);

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
                    SetSprite(resourceSprite);
                    break;

                case Source.URL:
                    if (string.IsNullOrWhiteSpace(path)) {
                        Debug.LogWarning("Source path is null or whitespace");
                        break;
                    }

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

        void SetSprite(Sprite s) {
            sprite = s;
        }

        protected override void OnDestroy() {
            if (!Application.isPlaying) return;

            Repo.Free(path, compression, this);
        }
    }
}
