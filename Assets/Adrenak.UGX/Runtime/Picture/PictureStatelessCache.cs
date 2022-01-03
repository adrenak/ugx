﻿using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// This a stateless cache, it just downloads downloads the image every time
    /// </summary>
    public class PictureStatelessCache : PictureCacheBase {
        public override UniTask Init(object obj = null) {
            return UniTask.CompletedTask;
        }

        public override void Free(string location, Texture2DCompression compression, Picture instance, Action onSuccess, Action<Exception> onFailure) {
            // DynamicImageFaceCache does not free instances as it never stores them in the first place
        }

        public override UniTask Free(string location, Texture2DCompression compression, Picture instance) {
            return UniTask.CompletedTask;
        }

        public override void Get(string location, Texture2DCompression compression, Picture instance, Action<Sprite> onSuccess, Action<Exception> onFailure) {
            Downloader.Download(location, result => {
                if (compression != Texture2DCompression.None)
                    result.Compress(compression == Texture2DCompression.HighQuality);
            }, onFailure);
        }

        public override UniTask<Sprite> Get(string location, Texture2DCompression compression, Picture instance) {
            var source = new UniTaskCompletionSource<Sprite>();
            Get(location, compression, instance,
                result => source.TrySetResult(result),
                exception => source.TrySetException(exception)
            );
            return source.Task;
        }
    }
}
