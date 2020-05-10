using System;
using UnityEngine;
using System.Collections.Generic;
using UnityWeld.Binding;
using UnityEngine.UI;

namespace Adrenak.UPF {
    [Binding]
    public abstract class ListView<T, V> : View where T : ViewModel where V : ListItemView<T> {
        [SerializeField] ScrollRect _scrollRect;
        public ScrollRect ScrollRect => _scrollRect;

        [SerializeField] HorizontalOrVerticalLayoutGroup _layoutGroup;
        public HorizontalOrVerticalLayoutGroup LayoutGroup => _layoutGroup;

        [SerializeField] Image _bgImage;
        public Image BGImage => _bgImage;

        public event Action<ListItemView<T>> Clicked;
        [SerializeField] V prefab;

        [SerializeField] Transform _container;
        public Transform Container => _container;

        [SerializeField] List<T> _itemsSource;
        public List<T> ItemsSource {
            set {
                _itemsSource = value;
                Clear();
                Instantiate();
            }
        }

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
