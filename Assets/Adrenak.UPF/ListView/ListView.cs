using System;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using UnityWeld.Binding;
using UnityEngine.UI;

namespace Adrenak.UPF {
    [Binding]
    public abstract class ListView<T, V> : View where T : ViewModel where V : ListItemView<T> {
        public event Action<ListItemView<T>> Clicked;
        public event Action OnPullToRefresh;

        [SerializeField] bool pullToRefresh;
        [SerializeField] float pullRefreshDistance;

        [SerializeField] ScrollRect _scrollRect;
        public ScrollRect ScrollRect => _scrollRect;

        [SerializeField] HorizontalOrVerticalLayoutGroup _layoutGroup;
        public HorizontalOrVerticalLayoutGroup LayoutGroup => _layoutGroup;

        [SerializeField] Image _bgImage;
        public Image BGImage => _bgImage;

        [SerializeField] V prefab;

        [SerializeField] Transform _container;
        public Transform Container => _container;

        [SerializeField] ListRefreshIndicator indicator;

        [SerializeField] List<T> _itemsSource;
        public List<T> ItemsSource {
            get => _itemsSource;
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

        void Update() {
            TryPullRefresh();
        }

        // ================================================
        // PULL TO REFRESH
        // ================================================
        bool isRefreshing;
        bool markForRefresh;
        void TryPullRefresh() {
            if (!pullToRefresh) return;

            // Make the list move smoothly
            if (isRefreshing)
                TopPadding = (int)Mathf.Clamp(pullRefreshDistance - NegativeDragDistance, 0, Mathf.Infinity);
            else 
                TopPadding = (int)Mathf.Lerp(TopPadding, 0, Time.deltaTime / _scrollRect.decelerationRate);

            // If we're not refreshing, set the value that represents the fraction
            // of how much the list has been draged towards refresh. We only call 
            // this when not refreshing as when it IS refreshing, the Drag Distance
            // will go to 0 
            if(!isRefreshing)
                indicator.SetValue(NegativeDragDistance / pullRefreshDistance);

            // We need to do this false->true to force 
            _layoutGroup.enabled = false;
            _layoutGroup.enabled = true;

            // If we have dragged it beyond the distance and it's not refreshing 
            // then we mark it for refresh. It's like a refresh dirty flag
            if (InRefreshZone && !isRefreshing && IsDragging)
                markForRefresh = true;

            // If we have marked for refresh, are under refresh zone, not dragging
            // and not refreshing => then we start refreshing
            if (markForRefresh && !InRefreshZone && !IsDragging && !isRefreshing) {
                isRefreshing = true;
                markForRefresh = false;
                indicator.SetRefreshing(true);
                OnPullToRefresh?.Invoke();
            }
        }

        bool InRefreshZone {
            get => NegativeDragDistance > pullRefreshDistance;
        }

        // How many pixels has the scroll rect been dragged down beyond 0
        float NegativeDragDistance {
            get => -_container.GetComponent<RectTransform>().localPosition.y;
        }

        int TopPadding {
            get => _layoutGroup.padding.top;
            set => _layoutGroup.padding.top = value;
        }

        // Uses reflection to find out if the scroll rect is being dragged
        bool IsDragging {
            get => (bool)typeof(ScrollRect).GetField("m_Dragging", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_scrollRect);
        }

        public void StopRefresh() {
            isRefreshing = false;
            indicator.SetRefreshing(false);
        }
    }
}
