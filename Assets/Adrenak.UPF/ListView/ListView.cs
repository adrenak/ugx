using System;
using UnityEngine;
using System.Collections.Generic;
using UnityWeld.Binding;

namespace Adrenak.UPF {
    [Binding]
    public abstract class ListView<T, V> : View where T : ViewModel where V : ListItemView<T> {
        public event Action<ListItemView<T>> Clicked;
        [SerializeField] V prefab;

        [SerializeField] List<T> _itemsSource;
        public List<T> ItemsSource {
            set {
                _itemsSource = value;
                Clear();
                Instantiate();
            }
        }

        [SerializeField] Transform _container;
        public Transform Container => _container;

        void Clear() {
            foreach (Transform child in _container)
                Destroy(child.gameObject);
        }

        void Instantiate() {
            foreach (var itemSource in _itemsSource) {
                var instance = Instantiate(prefab, _container);
                instance.Model = itemSource;
                instance.Clicked += obj => Clicked?.Invoke(obj);
            }
        }
    }
}
