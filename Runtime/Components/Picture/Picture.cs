﻿using System;

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

        RectTransform rt;
        RectTransform RT => rt == null ? rt = GetComponent<RectTransform>() : rt;

        [ReadOnly] public Visibility currentVisibility = Visibility.None;
        public Visibility CurrentVisibility => currentVisibility;

        protected override void Awake() {
            Clear();
        }

        protected override void Start() {
            rt = GetComponent<RectTransform>();
        }

        [Button("Refresh")]
        public void Refresh() {
            if (!Application.isPlaying) return;
            if (CurrentVisibility == Visibility.None) return;

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
                            result => {
                                if(sprite == null){
                                    SetSprite(result.ToSprite());
                                    return;
                                }
                                if(sprite.texture == null){
                                    SetSprite(result.ToSprite());
                                    return;
                                }
                                if(sprite.texture != result)
                                    SetSprite(result.ToSprite());
                            },
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

        // NOTE: We ignore the first 2 frames because often times Pictures are visible for
        // a few frames on instantiation when used inside an AutoLayout, but are not 
        // supposed to be. Unity seems to update layouts in the next frame after they are
        // enabled.
        int age = 0;
        void Update() {
            age++;
            if (age <= 2) return;

            var visibility = GetVisibility();
            if (currentVisibility != visibility) {
                if (currentVisibility == Visibility.None && visibility != Visibility.None){
                    currentVisibility = visibility;
                    Refresh();
                }
                else if (currentVisibility != Visibility.None && visibility == Visibility.None){
                    currentVisibility = visibility;
                    Cache.Free(path, compression, this);
                }
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

        [ContextMenu("Test")]
        void Test(){
            var result = RT.IsVisible(out bool? fully);
            Debug.Log(result + " " + fully);
        }

        Visibility GetVisibility() {
            var result = RT.IsVisible(out bool? fully);

            if (result)
                return fully.Value ? Visibility.Full : Visibility.Partial;
            else
                return Visibility.None;
        }

        bool destroyed = false;
        protected override void OnDestroy() {
            destroyed = true;
            if (!Application.isPlaying) return;
            Cache.Free(path, compression, this);
        }
    }
}
