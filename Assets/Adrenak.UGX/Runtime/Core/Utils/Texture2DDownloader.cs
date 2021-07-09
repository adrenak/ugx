using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Adrenak.UGX {
    /// <summary>
    /// A coroutine based image downloader that returns the result as a Texture2D.
    /// </summary>
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
        float textureLoadPeriod;
        List<Request> pending = new List<Request>();
        List<Request> ongoing = new List<Request>();
        List<Action> textureLoads = new List<Action>();

        bool CanSendNewRequest => ongoing.Count < maxConcurrentDownloads;
        float loadTimer = 0;

        /// <summary>
        /// Cannot construct using 'new' keyword. Use .New method.
        /// </summary>
        Texture2DDownloader() { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="_maxConcurrentDownloads">The maximum number of cocurrent downloads possible.</param>
        /// <param name="_textureLoadPeriod">The minimum time between loading consecutive Texture2D objects from byte[]</param>
        /// <returns></returns>
        public static Texture2DDownloader New(int _maxConcurrentDownloads = 5, float _textureLoadPeriod = .1f) {
            var go = new GameObject("Texture2DDownloader");
            DontDestroyOnLoad(go);
            go.hideFlags = HideFlags.DontSave;
            var instance = go.AddComponent<Texture2DDownloader>();
            instance.maxConcurrentDownloads = _maxConcurrentDownloads;
            instance.textureLoadPeriod = _textureLoadPeriod;
            instance.loadTimer = _textureLoadPeriod;
            return instance;
        }

		void Update() {
            DispatchRequests();

            loadTimer += Time.unscaledDeltaTime;
            if(loadTimer > textureLoadPeriod) {
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

        /// <summary>
        /// Downloads an image from the URL and returns the results as callbacks
        /// </summary>
        /// <param name="path">The URL/path</param>
        /// <param name="onSuccess">Callback when the download is successful</param>
        /// <param name="onFailure">Callback for when the download is unsuccessful</param>
		public void Download(string path, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            var req = new Request(path, onSuccess, onFailure);
            if (CanSendNewRequest) {
                StartCoroutine(SendRequest(req));
                return;
            }
            else
                pending.Add(req);
        }

        /// <summary>
        /// Downloads an image form the URL and returns the results using UniTask
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A UniTask<Texture2D> instance</Texture2D></returns>
        public UniTask<Texture2D> Download(string path){
            var source = new UniTaskCompletionSource<Texture2D>();
            Download(path,
                result => source.TrySetResult(result),
                exception => source.TrySetException(exception)
            );
            return source.Task;
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
                var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                tex.LoadImage(www.bytes);
                tex.Apply();

                request.onSuccess?.Invoke(tex);
                ongoing.Remove(request);
                www.Dispose();
            });
        }
    }
}
