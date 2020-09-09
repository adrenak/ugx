using System;

namespace Adrenak.UGX {
    public class ViewEvent<T> {
        public event EventHandler<T> Handler;
        public void Invoke(object sender, T t) => Handler?.Invoke(sender, t);
    }

    public class ViewEvent {
        public event EventHandler Handler;
        public void Invoke(object sender) => Handler?.Invoke(sender, EventArgs.Empty);
    }
}