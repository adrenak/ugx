using System;

namespace Adrenak.UGX{
    [Serializable]
    public abstract class State {
        public string ID = Guid.NewGuid().ToString();
    }
}