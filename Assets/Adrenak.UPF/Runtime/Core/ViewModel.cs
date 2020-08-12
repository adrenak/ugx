using System;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class ViewModel {
        public string identifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
    }
}
