using System;

namespace Adrenak.UGX{
    [Serializable]
    public abstract class ViewState {
        public string ID = Guid.NewGuid().ToString();
    }
}