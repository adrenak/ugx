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
				repo = value ?? throw new Exception("DynamicImage.Cache cannot set Cache to null!");
            }
        }

        public enum Source {
            Asset,
            Resource,
            URL
        }

        public UnityEvent onRefreshStart;
        public UnityEvent onRefreshSuccess;
        public UnityEvent onRefreshFailure;

        public bool refreshOnStart;
        public bool updateWhenOffScreen;

        public Source source = Source.URL;

        Texture2DCompression oldCompression = Texture2DCompression.None;
        public Texture2DCompression compression = Texture2DCompression.LowQuality;

        string oldPath = string.Empty;
        public string path = string.Empty;

        RectTransform rt;
        RectTransform RT => rt == null ? rt = GetComponent<RectTransform>() : rt;

        Visibility currentVisibility = Visibility.None;
        public Visibility CurrentVisibility => currentVisibility;

        protected override void Start() {
            rt = GetComponent<RectTransform>();
            if(refreshOnStart)
                Refresh();
        }

        [Button("Refresh")]
        public void Refresh() {
            if (!Application.isPlaying) return;
            if (currentVisibility == Visibility.None && !updateWhenOffScreen) return;   // TODO: Try to remove
            if (string.IsNullOrWhiteSpace(path)) return;

            Cache.Free(oldPath, oldCompression, this);

            switch (source) {
                case Source.Resource:
                    onRefreshStart.Invoke();
                    var resourceSprite = Resources.Load<Sprite>(path);
                    if (resourceSprite == null) {
                        onRefreshFailure.Invoke();
                        Debug.LogError($"Not Resource found at {path}");
                        break;
                    }

                    onRefreshSuccess.Invoke();
                    SetSprite(resourceSprite);
                    break;

                case Source.URL:
                    try {
                        onRefreshStart.Invoke();
                        Cache.Get(
                            path, compression, this,
                            result => {
                                SetSprite(result.ToSprite());
                                onRefreshSuccess.Invoke();
                            },
                            error => {
                                Debug.LogError($"Dynamic Image Refresh from remote path failed: " + error);
                                onRefreshFailure.Invoke();
                            }
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

            var oldVisibility = currentVisibility;
            currentVisibility = GetVisibility();
            
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
                onRefreshSuccess?.Invoke();
            }
        }

        Visibility GetVisibility() {
            var result = RT.IsVisible(out bool? partially);

            if (!partially.Value)
                return result ? Visibility.Full : Visibility.None;
            else
                return Visibility.Partial;
        }

        bool destroyed = false;
        protected override void OnDestroy() {
            if (destroyed) return;
            destroyed = true;

            if (!Application.isPlaying) return;
            Cache.Free(path, compression, this);
        }
    }
}
