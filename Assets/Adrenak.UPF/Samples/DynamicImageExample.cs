using UnityEngine;

namespace Adrenak.UPF {
    public class DynamicImageExample : MonoBehaviour {
        void Awake() {
            var cache = new DynamicImageInMemoryCache();
            cache.Init();
            DynamicImage.Cache = cache;
        }
    }
}
