using System;
using UnityEngine;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Adrenak.UPF {
    [Serializable]
    public class BindableBehaviour : MonoBehaviour, INotifyPropertyChanged, INotifyPropertyChanging {
        readonly ConcurrentDictionary<string, object> _properties = new ConcurrentDictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected bool CallPropertyChangeEvent { get; set; } = true;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanging([CallerMemberName] string propertyName = null) {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }


        protected T Get<T>(string name, T defValue = default(T)) {
            return !string.IsNullOrEmpty(name) && _properties.TryGetValue(name, out var value)
                ? (T)value
                : defValue;
        }

        protected bool Set<T>(T value, [CallerMemberName] string name = null) {
            if (string.IsNullOrEmpty(name))
                return false;

            var isExists = _properties.TryGetValue(name, out var getValue);
            if (isExists && Equals(value, getValue))
                return false;

            if (CallPropertyChangeEvent)
                OnPropertyChanging(name);

            _properties.AddOrUpdate(name, value, (s, o) => value);

            if (CallPropertyChangeEvent)
                OnPropertyChanged(name);

            return true;
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string name = null) {
            field = value;
            return Set(value, name);
        }
    }
}