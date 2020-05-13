using System;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine.UI;
using System.Collections.ObjectModel;

namespace Adrenak.UPF {
    [Serializable]
    public abstract class ListView<ViewModelType, ViewType> : View where ViewModelType : ViewModel where ViewType : View<ViewModelType> {
        public class ItemSelectedEventArgs : EventArgs {
            public ViewModel Item { get; private set; }
            public ItemSelectedEventArgs(ViewModel item) {
                Item = item;
            }
        }

        public event EventHandler<ItemSelectedEventArgs> OnItemSelected;
        public event EventHandler OnPulledToRefresh;

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

        [Header("Instantiation")]
        [SerializeField] ViewType prefab;

        [SerializeField] Transform _container;
        public Transform Container => _container;

        // NOTE: This COULD be moved to a ListViewModel too but I'm not doing that right now -adrenak
        [SerializeField]
        ObservableCollection<ViewModelType> _itemsSource
            = new ObservableCollection<ViewModelType>();
        public ObservableCollection<ViewModelType> ItemsSource => _itemsSource;

        readonly List<ViewType> instantiated = new List<ViewType>();

        public Func<ViewType, string> InstanceNamer;
#pragma warning restore 0649

        void Awake() {
            ItemsSource.CollectionChanged += (sender, args) => {
                switch (args.Action) {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var newItem in args.NewItems)
                            Instantiate(newItem as ViewModelType);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var removed in args.OldItems)
                            Destroy(removed as ViewModelType);
                        break;
                }
            };
        }

        void Instantiate(ViewModelType t) {
            var instance = Instantiate(prefab, _container);
            instance.Context = t;

            instance.name = InstanceNamer != null ?
                InstanceNamer(instance) :
                "#" + instance.transform.GetSiblingIndex();

            instance.OnViewSelected += (sender, args) =>
                OnItemSelected?.Invoke(this, new ItemSelectedEventArgs(instance.Context));

            instantiated.Add(instance);
            Init(instance.Context);
        }

        void Destroy(ViewModelType t) {
            foreach (var instance in instantiated) {
                if (instance.Context == t) {
                    Deinit(instance.Context);
                    Destroy(instance.gameObject);
                }
            }
        }

        virtual protected void Init(ViewModelType cell) { }
        virtual protected void Deinit(ViewModelType cell) { }

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
                OnPulledToRefresh?.Invoke(this, EventArgs.Empty);
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
