using System;
using System.Collections;
using System.Collections.Generic;
using Adrenak.Unex;
using UnityEngine;

namespace Adrenak.UGX {
    public class Texture2DDownloader : MonoBehaviour {
        class Request {
            public string path;
            public Action<Texture2D> onSuccess;
            public Action<Exception> onFailure;

            public Request(string _path, Action<Texture2D> _onSuccess, Action<Exception> _onFailure) {
                path = _path;
                onSuccess = _onSuccess;
                onFailure = _onFailure;
            }
        }

        int maxConcurrentDownloads;
        float secondsPerTextureLoad;
        List<Request> pending = new List<Request>();
        List<Request> ongoing = new List<Request>();
        List<Action> textureLoads = new List<Action>();

        bool CanSendNewRequest => ongoing.Count < maxConcurrentDownloads;
        float loadTimer = 0;

        Texture2DDownloader() { }

        public static Texture2DDownloader New(int _maxConcurrentDownloads = 5, float _secondsPerTextureLoad = .1f) {
            var go = new GameObject("Texture2DDownloader");
            DontDestroyOnLoad(go);
            go.hideFlags = HideFlags.DontSave;
            var instance = go.AddComponent<Texture2DDownloader>();
            instance.maxConcurrentDownloads = _maxConcurrentDownloads;
            instance.secondsPerTextureLoad = _secondsPerTextureLoad;
            instance.loadTimer = _secondsPerTextureLoad;
            return instance;
        }

		void Update() {
            DispatchRequests();

            loadTimer += Time.unscaledDeltaTime;
            if(loadTimer > secondsPerTextureLoad) {
                loadTimer = 0;
                DispatchTextureLoads();
			}
		}

        void DispatchRequests() {
            if(ongoing.Count < maxConcurrentDownloads && pending.Count > 0)
                StartCoroutine(SendRequest(pending[0]));
        }

        void DispatchTextureLoads() {
            if(textureLoads.Count > 0) {
                textureLoads[0]?.Invoke();
                textureLoads.RemoveAt(0);
			}
		}

		public void Download(string path, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            var req = new Request(path, onSuccess, onFailure);
            if (CanSendNewRequest) {
                Runnable.Run(SendRequest(req));
                return;
            }
            else
                pending.Add(req);
        }

        IEnumerator SendRequest(Request request) {
            pending.EnsureDoesntContain(request);
            ongoing.EnsureContains(request);

            string path = request.path;
            if (!path.StartsWith("http"))
                path = "file://" + path;
            var www = new WWW(path);
            
            yield return www;
            while (!www.isDone)
                yield return null;

            if (!string.IsNullOrWhiteSpace(www.error)) {
                request.onFailure?.Invoke(new Exception(www.error));
                ongoing.Remove(request);
                yield break;
            }

            textureLoads.Add(() => {
                var tex = new Texture2D(2, 2);
                tex.LoadImage(www.bytes);
                tex.Apply();

                request.onSuccess?.Invoke(tex);
                ongoing.Remove(request);
                www.Dispose();
            });
        }
    }
}
