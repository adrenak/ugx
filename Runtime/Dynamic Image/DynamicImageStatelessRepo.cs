using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UPF {
    /// <summary>
    /// This isn't a cache, it just downloads downloads the image every time
    /// </summary>
    public class DynamicImageStatelessRepo : DynamicImageRepository {
        public override Task Init(object obj = null) {
            return Task.CompletedTask;
        }

        public override void Free(string location, Texture2DCompression compression, DynamicImage instance, Action onSuccess, Action<Exception> onFailure) {
            // DynamicImageFaceCache does not free instances as it never stores them in the first place
        }

        public override Task Free(string location, Texture2DCompression compression, DynamicImage instance) {
            return Task.CompletedTask;
        }

        public override void Get(string location, Texture2DCompression compression, DynamicImage instance, Action<Texture2D> onSuccess, Action<Exception> onFailure) {
            Downloader.Download(location, compression, onSuccess, onFailure);
        }

        public override Task<Texture2D> Get(string location, Texture2DCompression compression, DynamicImage instance) {
            var source = new TaskCompletionSource<Texture2D>();
            Get(location, compression, instance,
                result => source.SetResult(result),
                exception => source.SetException(exception)
            );
            return source.Task;
        }
    }
}
