using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UGX {
    /// <summary>
    /// This a stateless cache, it just downloads downloads the image every time
    /// </summary>
    public class PictureStatelessCache : AbstractPictureCache {
        public override Task Init(object obj = null) {
            return Task.CompletedTask;
        }

        public override void Free(string location, Texture2DCompression compression, Picture instance, Action onSuccess, Action<Exception> onFailure) {
            // DynamicImageFaceCache does not free instances as it never stores them in the first place
        }

        public override Task Free(string location, Texture2DCompression compression, Picture instance) {
            return Task.CompletedTask;
        }

        public override void Get(string location, Texture2DCompression compression, Picture instance, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            Downloader.Download(location, compression, onSuccess, onFailure);
        }

        public override Task<Texture2D> Get(string location, Texture2DCompression compression, Picture instance) {
            var source = new TaskCompletionSource<Texture2D>();
            Get(location, compression, instance,
                result => source.SetResult(result),
                exception => source.SetException(exception)
            );
            return source.Task;
        }
    }
}
