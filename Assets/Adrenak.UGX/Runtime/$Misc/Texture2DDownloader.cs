using System;
using System.Collections;
using System.Collections.Generic;
using Adrenak.Unex;
using UnityEngine;

namespace Adrenak.UGX {
    public class Texture2DDownloader {
        class Request {
            public string path;
            public Texture2DCompression compression;
            public Action<Texture2D> onSuccess;
            public Action<Exception> onFailure;

            public Request(string _path, Texture2DCompression _compression, Action<Texture2D> _onSuccess, Action<Exception> _onFailure) {
                path = _path;
                compression = _compression;
                onSuccess = _onSuccess;
                onFailure = _onFailure;
            }
        }

        int maxConcurrentDownloads;
        List<Request> pending = new List<Request>();
        List<Request> ongoing = new List<Request>();

        bool CanSendNewRequest => ongoing.Count < maxConcurrentDownloads;

        public Texture2DDownloader(int _maxConcurrentDownloads = 10000) {
            maxConcurrentDownloads = _maxConcurrentDownloads;
        }

        public void Download(string path, Texture2DCompression compression, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            var req = new Request(path, compression, onSuccess, onFailure);
            if (CanSendNewRequest) {
                Runnable.Run(SendRequest(req));
                return;
            }
            else
                pending.Add(req);
        }

        void OnRequestFinished(Request req) {
            ongoing.Remove(req);

            while (ongoing.Count < maxConcurrentDownloads && pending.Count > 0)
                Runnable.Run(SendRequest(pending[0]));
        }

        IEnumerator SendRequest(Request request) {
            pending.EnsureDoesntExist(request);
            ongoing.EnsureExists(request);

            var www = new WWW(request.path);
            yield return www;
            while (!www.isDone)
                yield return null;

            if (!string.IsNullOrWhiteSpace(www.error)) {
                request.onFailure?.Invoke(new Exception(www.error));
                OnRequestFinished(request);
                yield break;
            }

            var tex = new Texture2D(2, 2);
            tex.LoadImage(www.bytes);
            tex.Apply();
            if (request.compression != Texture2DCompression.None)
                tex.Compress(request.compression == Texture2DCompression.HighQuality);

            request.onSuccess?.Invoke(tex);
            OnRequestFinished(request);
            www.Dispose();
        }
    }
}
