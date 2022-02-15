using System;
using System.Collections.Generic;

namespace Adrenak.UGX {
    public class Observable {
        private object value;
        private List<Action<object>> listeners = new List<Action<object>>();

        public Observable(object value) {
            this.value = value;
        }

        public object Value {
            get { return value; }
            set {
                try { if (this.value.Equals(value)) return; } catch { }
                this.value = value;
                Trigger();
            }
        }

        public void SetAndTrigger(object value) {
            this.value = value;
            Trigger();
        }

        public void Bind(Action<object> listener) {
            LazyBind(listener);
            listener(Value);
        }

        public void LazyBind(Action<object> listener) {
            listeners.Add(listener);
        }

        public void Unbind(Action<object> listener) {
            listeners.Remove(listener);
        }

        public void Trigger() {
            listeners.ForEach(listener => {
                try {
                    listener?.Invoke(Value);
                }
                catch (Exception e) {
                    UGX.Debug.LogError(e);
                    Unbind(listener);
                }
            });
        }
    }
}
