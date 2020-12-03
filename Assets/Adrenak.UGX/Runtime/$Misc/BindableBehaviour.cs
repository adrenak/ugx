﻿using System;
using UnityEngine;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Adrenak.UGX {
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute {
        public NotifyPropertyChangedInvocatorAttribute() { }

        public NotifyPropertyChangedInvocatorAttribute(string parameterName) {
            ParameterName = parameterName;
        }

        public string ParameterName { get; private set; }
    }

    [Serializable]
    public class Bindable : INotifyPropertyChanged {
        readonly ConcurrentDictionary<string, object> _properties = new ConcurrentDictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

            _properties.AddOrUpdate(name, value, (s, o) => value);

            OnPropertyChanged(name);

            return true;
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string name = null) {
            field = value;
            return Set(value, name);
        }
    }

    public class BindableBehaviour : MonoBehaviour, INotifyPropertyChanged {
        readonly ConcurrentDictionary<string, object> _properties = new ConcurrentDictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

            _properties.AddOrUpdate(name, value, (s, o) => value);

            OnPropertyChanged(name);

            return true;
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string name = null) {
            field = value;
            return Set(value, name);
        }
    }
}