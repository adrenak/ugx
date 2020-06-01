using Adrenak.Unex;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    [Serializable]
    public class DynamicImage : Image {
        public enum Source {
            ResourcePath,
            RemotePath
        }

        public enum Compression{
            None,
            HighQuality,
            LowQuality
        }

        Compression _oldCompression = Compression.LowQuality;
        [SerializeField] Compression _compression = Compression.LowQuality;
        public Compression compression{
            get => _compression;
            set {
                _compression = value;
                if (NeedsRefresh())
                    Refresh();
            }
        }

        Source _oldSource = Source.RemotePath;
        [SerializeField] Source _source = Source.RemotePath;
        public Source source {
            get => _source;
            set {
                _source = value;
                if(NeedsRefresh())
                    Refresh();
            }
        }

        string _oldPath = string.Empty;
        [SerializeField] string _path = string.Empty;
        public string path {
            get => _path;
            set {
                _path = value;
                if (NeedsRefresh())
                    Refresh();
            }
        }

        [ContextMenu("Refresh")]
        public void Refresh() {
            if (_oldSource == source) {

            }
            switch (source) {
                case Source.ResourcePath:
                    var resourceSprite = Resources.Load<Sprite>(path);
                    if(resourceSprite == null){
                        Debug.LogError($"Not Resource found at {path}");
                        break;
                    }
                    SetSprite(resourceSprite);
                    break;
                case Source.RemotePath:
                    try {
                        DownloadSprite(path,
                            result => SetSprite(result),
                            error => Debug.LogError($"Dynamic Image Refresh from remote path failed: " + error)
                        );
                    }
                    catch (Exception e) {
                        Debug.LogError(e);
                    }
                    break;
            }
        }

        bool NeedsRefresh() {
            bool result = false;

            if (_oldCompression != compression)
                result = true;

            if (_oldSource != source) 
                result = true;

            if(!_oldPath.Equals(path))
                result = true;

            _oldCompression = compression;
            _oldSource = source;
            _oldPath = path;
            return result;
        }

        void SetSprite(Sprite s) {
            if (sprite != null) {
                var id = sprite.GetInstanceID();
                if(id < 0){
                    if (Application.isPlaying) {
                        Destroy(sprite.texture);
                        Destroy(sprite);
                    }
                    else {
                        DestroyImmediate(sprite.texture);
                        DestroyImmediate(sprite);
                    }
                }
            }
            sprite = s;
        }

        public void DownloadSprite(string path, Action<Sprite> onSuccess, Action<Exception> onFailure) {
            StartCoroutine(DownloadSpriteCo(path, onSuccess, onFailure));
        }

        IEnumerator DownloadSpriteCo(string path, Action<Sprite> onSuccess, Action<Exception> onFailure) {
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
            if(compression != Compression.None)
                tex.Compress(compression == Compression.HighQuality);

            onSuccess?.Invoke(tex.ToSprite());

            www.Dispose();
        }
    }
}
