using System;

using UnityEngine;
using UnityEngine.UI;

using Adrenak.Unex;

namespace Adrenak.UPF {
    [Serializable]
    public class DynamicImage : Image {
        public static Cache<Texture2D> Cache { get; set; } = new DynamicImageInMemoryCache();

        public enum Source {
            Asset,
            ResourcePath,
            RemotePath
        }

        public enum Compression {
            None,
            HighQuality,
            LowQuality
        }

        public Source source = Source.RemotePath;

        Compression oldCompression;
        public Compression compression = Compression.LowQuality;

        string oldPath;
        public string path = string.Empty;
        
        public bool loadOnStart = true;

        protected override void Start() {
            if(loadOnStart)
                Refresh();
        }

        [ContextMenu("Refresh")]
        public void Refresh() {
            if (!Application.isPlaying) return;

            Cache.Free(new object[] { oldPath, oldCompression, this });

            switch (source) {
                case Source.Asset:
                    break;

                case Source.ResourcePath:
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

                case Source.RemotePath:
                    if (string.IsNullOrWhiteSpace(path)) {
                        Debug.LogWarning("Source path is null or whitespace");
                        break;
                    }

                    try {
                        Cache.Get(
                            new object[] { path, compression, this },
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

            Cache.Free(new object[] { path, compression, this });
        }
    }
}
