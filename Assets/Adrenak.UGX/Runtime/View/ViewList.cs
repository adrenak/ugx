using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

#if UGX_NAUGHTY_AVAILABLE
using NaughtyAttributes;
#endif

namespace Adrenak.UGX {
    /// <summary>
    /// A component that populates view templates used solely on state data. ViewList implements
    /// allows editing the UI using List-list methods such as .Add, .Remove.
    /// </summary>
    /// <typeparam name="T">The ViewState type that is used by the child Views</typeparam>
    [Serializable]
    public abstract class ViewList<T> : View, IReadOnlyList<T>, IReadOnlyCollection<T>, IEnumerable<T>, ICollection<T>, IList<T> where T : ViewState {
        /// <summary>
        /// The List may often need resizing due to the child element sizes often being dynamic over time.
        /// Large values may lead to late correction in size, but are performance friendly.
        /// Small values will lead to near instantanous correction, but requires more frequent checking, making it heavier.
        /// Set as 0 to disable. 
        /// </summary>
        [Tooltip("The List may often need resizing due to the child element sizes often being dynamic over time." +
        "Large values may lead to late correction in size, but are performance friendly." +
        "Small values will lead to near instantanous correction, but requires more frequent checking, making it heavier." +
        "Set as 0 to disable.")]
        public int resizingCheckFrameStep = 5;

        /// <summary>
        /// The parent under which the elements must be instantiated.
        /// </summary>
        [Tooltip("The parent under which the elements must be instantiated.")]
#if UGX_NAUGHTY_AVAILABLE
        [BoxGroup("Instantiation")]
#endif
        public Transform container = null;

        /// <summary>
        /// Template of the element View. Can be prefab or GameObject.
        /// </summary>
        [Tooltip("Template of the element View. Can be prefab or GameObject.")]
#if UGX_NAUGHTY_AVAILABLE
        [BoxGroup("Instantiation")]
#endif
        public View template = null;

        /// <summary>
        /// Whether the list uses the states list in the Start() method to
        /// populate the elements. This can be used to author predefined UI.
        /// </summary>
        [Tooltip("Whether the list uses the states list in the Start() method to" +
        "populate the elements. This can be used to author predefined UI.")]
        public bool populateOnStart = false;

        [Tooltip("The states of the current children views")]
#if UGX_NAUGHTY_AVAILABLE
        [BoxGroup("State")]
#endif
        [SerializeField] List<T> currentStates = new List<T>();

        List<StatefulView<T>> Instantiated { get; } = new List<StatefulView<T>>();
        List<UnityAction<StatefulView<T>>> childViewMethods = new List<UnityAction<StatefulView<T>>>();

        // ================================================
        // API / PUBLIC
        // ================================================
        public int Count => currentStates.Count;
        public bool IsReadOnly => false;
        public T this[int index] {
            get {
                if (index < 0 || index > currentStates.Count - 1)
                    throw new Exception("Index out of bounds");
                return currentStates[index];
            }
            set {
                if (index < 0 || index > currentStates.Count)
                    throw new Exception("Index out of bounds");
                Insert(index, value);
            }
        }

        /// <summary>
        /// Use to inject an action that runs on every view that has been instantiated.
        /// </summary>
        /// <param name="method">The action to run</param>
        /// <param name="invokeOnFutureChildViews">Whether the action should be on future elements as well upons being created</param>
        public void InvokeAllChildViews(UnityAction<StatefulView<T>> method, bool invokeOnFutureChildViews = true) {
            Instantiated.ForEach(x => method(x));
            if (invokeOnFutureChildViews)
                childViewMethods.Add(method);
        }

        /// <summary>
        /// Clears the state and instantiated child views
        /// </summary>
        public void Clear() {
            foreach (var state in currentStates)
                Destroy(state);
            currentStates.Clear();
        }

        /// <summary>
        /// Repopulates all the instances using the current state
        /// </summary>
#if UGX_NAUGHTY_AVAILABLE
        [Button]
#endif
        public void Refresh() {
            foreach (var state in currentStates)
                Destroy(state);
            PopulateFromCurrentStates();
        }

        /// <summary>
        /// Returns if the List contains and element for the given state
        /// </summary>
        public bool Contains(T item) => currentStates.Contains(item);

        /// <summary>
        /// Adds a state to the list and instantiates a view for it
        /// </summary>
        public void Add(T item) => Insert(currentStates.Count, item);

        /// <summary>
        /// Adds a list of states and instantiate views for it
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(List<T> items) {
            items.ForEach(x => Add(x));
        }

        /// <summary>
        /// This method isn't implemented yet.
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex) =>
            throw new NotImplementedException("ViewList doesn't support CopyTo yet.");

        /// <summary>
        /// Removes the state as well as its corresponding View instance
        /// </summary>
        public bool Remove(T item) {
            if (Contains(item)) {
                Destroy(item);
                currentStates.Remove(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the index of the state in the list
        /// </summary>
        public int IndexOf(T item) {
            return currentStates.IndexOf(item);
        }

        /// <summary>
        /// Inserts a state in the list, also moves its corersponding View instance
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item) {
            if (item == null)
                throw new Exception("Inserted item cannot be null");

            if (index < 0)
                throw new IndexOutOfRangeException("Insert method index cannot be negative");

            if (index > 0 && index > currentStates.Count)
                throw new IndexOutOfRangeException("Insert method index out of bounds");

            var instance = Instantiate(item);
            if (instance != null) {
                if (index > 0) {
                    currentStates.Insert(index, item);
                    Instantiated.Insert(index, instance);
                    if (instance.transform.GetSiblingIndex() != index)
                        instance.transform.SetSiblingIndex(index);
                }
                else {
                    currentStates.Add(item);
                    Instantiated.Add(instance);
                    instance.transform.SetSiblingIndex(0);
                }
            }
        }

        /// <summary>
        /// Removes the state from the state list and destroys the corresponding View instance
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index) {
            if (index < 0 || index >= currentStates.Count)
                throw new IndexOutOfRangeException("RemoveAt method index was out of range");

            var vm = currentStates[index];
            Destroy(vm);
            currentStates.RemoveAt(index);
        }

        public IEnumerator<T> GetEnumerator() {
            return (IEnumerator<T>)new ViewListEnumerator<T>(currentStates.ToArray());
        }

        // ================================================
        // UNITY ENIGNE LOOP
        // ================================================
        void Start() {
            if (populateOnStart)
                PopulateFromCurrentStates();
        }

        void PopulateFromCurrentStates(){
            currentStates.ForEach(x => {
                var instance = Instantiate(x);
                if (instance != null)
                    Instantiated.Add(instance);
            });
        }

        void Update() {
            TryResize();
        }

        int lastHeight, lastWidth, height, width;
        RectTransform childRT;
        void TryResize() {
            if (resizingCheckFrameStep <= 0 || Time.frameCount % resizingCheckFrameStep != 0) return;

            height = width = 0;
            foreach (Transform child in container) {
                childRT = child.GetComponent<RectTransform>();
                height += (int)childRT.sizeDelta.y;
                width += (int)childRT.sizeDelta.x;
            }

            if (height != lastHeight || width != lastWidth)
                LayoutRebuilder.MarkLayoutForRebuild(container.GetComponent<RectTransform>());

            lastHeight = height;
            lastWidth = width;
        }

        // ================================================
        // INSTANCE MANAGEMENT
        // ================================================
        StatefulView<T> Instantiate(T t) {
            // We don't initialize when the application isn't playing.
            // This sometimes happens with requests that are fullfilled after
            // play mode exits in the editor and end up instantiation in editor mode.
            if (!Application.isPlaying)
                return null;

            if (template == null)
                throw new Exception("No ViewTemplate assigned! Cannot instantiate elements in ViewGroup.");

            if (!(template is StatefulView<T>))
                throw new Exception("The template View must be of type View<" + typeof(T) + ">");

            var instance = MonoBehaviour.Instantiate(template, container);
            instance.gameObject.SetActive(true);
            instance.hideFlags = HideFlags.DontSave;
            (instance as StatefulView<T>).State = t;

            childViewMethods.ForEach(x => x?.Invoke(instance as StatefulView<T>));

            return instance as StatefulView<T>;
        }

        void Destroy(T t) {
            foreach (var instance in Instantiated) {
                if (instance != null && instance.State.Equals(t) && instance.gameObject != null) {
                    Instantiated.Remove(instance);
                    MonoBehaviour.Destroy(instance.gameObject);
                    break;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (IEnumerator<T>)new ViewListEnumerator<T>(currentStates.ToArray());
        }

        [Obsolete("Use .RunPerChildView instead")]
        public void SubscribeToChildren(UnityAction<StatefulView<T>> subscription) =>
            InvokeAllChildViews(subscription, true);
    }

    // When you implement IEnumerable, you must also implement IEnumerator.
    public class ViewListEnumerator<T> : IEnumerator where T : ViewState {
        public T[] array;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public ViewListEnumerator(T[] list) {
            array = list;
        }

        public bool MoveNext() {
            position++;
            return (position < array.Length);
        }

        public void Reset() {
            position = -1;
        }

        object IEnumerator.Current {
            get {
                return Current;
            }
        }

        public T Current {
            get {
                try {
                    return array[position];
                }
                catch (IndexOutOfRangeException) {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
