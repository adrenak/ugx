using System;

namespace Adrenak.UPF {
    public class ViewModel : BindableBehaviour {
        public string identifier = Guid.NewGuid().ToString();
    }
}
