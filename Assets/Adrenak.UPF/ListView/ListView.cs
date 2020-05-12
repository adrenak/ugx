using System;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using UnityWeld.Binding;
using UnityEngine.UI;

namespace Adrenak.UPF {
    [Serializable]
    [Binding]
    public abstract class ListView<T, V> : View where T : ViewModel where V : View<T> {
        public event EventHandler OnPullToRefresh;

#pragma warning disable 0649
        [Header("Pull Down Refresh")]
        [SerializeField] bool pullToRefresh;
        [SerializeField] float pullRefreshDistance;
        [SerializeField] RefreshIndicator indicator;

        [Header("Unity UI Components")]
        [SerializeField] ScrollRect _scrollRect;
        public ScrollRect ScrollRect => _scrollRect;

        [SerializeField] HorizontalOrVerticalLayoutGroup _layoutGroup;
        public HorizontalOrVerticalLayoutGroup LayoutGroup => _layoutGroup;

        [SerializeField] Image _bgImage;
        public Image BGImage => _bgImage;

        [Header("Instantiation")]
        [SerializeField] V prefab;

        [SerializeField] Transform _container;
        public Transform Container => _container;

        // NOTE: This COULD be moved to a ListViewModel too but I'm not doing that right now -adrenak
        [SerializeField] ObservableList<T> _itemsSource = new ObservableList<T>();
        public ObservableList<T> ItemsSource => _itemsSource;

        readonly List<V> instantiated = new List<V>();
#pragma warning restore 0649

        public Func<V, string> InstanceNamer;

        void Awake() {
            ItemsSource.CollectionChanged += (sender, args) => {
                switch (args.Action) {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var newItem in args.NewItems)
                            Instantiate(newItem as T);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var removed in args.OldItems)
                            Destroy(removed as T);
                        break;
                }
            };
        }

        void Instantiate(T t) {
            var instance = Instantiate(prefab, _container);
            instance.BindingContext = t;

            instance.name = InstanceNamer != null ?
                InstanceNamer(instance) :
                "#" + instance.transform.GetSiblingIndex();

            instantiated.Add(instance);
            Init(instance.BindingContext);
        }

        void Destroy(T t) {
            foreach (var instance in instantiated) {
                if (instance.BindingContext == t) {
                    Deinit(instance.BindingContext);
                    Destroy(instance.gameObject);
                }
            }
        }

        virtual protected void Init(T cell) { }
        virtual protected void Deinit(T cell) { }

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
            if (!isRefreshing)
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
                OnPullToRefresh?.Invoke(this, EventArgs.Empty);
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
