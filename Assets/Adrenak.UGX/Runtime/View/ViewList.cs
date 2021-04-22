using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using NaughtyAttributes;
using UnityEngine.UI;

namespace Adrenak.UGX {
    [Serializable]
    public abstract class ViewList<T> : View, ICollection<T>, IList<T> where T : ViewState {
        public int resizingCheckFrameStep = 2;
        [BoxGroup("Instantiation")] public Transform container = null;
        [BoxGroup("Instantiation")] public View template = null;

        [BoxGroup("State")] [SerializeField] List<T> statesList = new List<T>();

        List<StatefulView<T>> Instantiated { get; } = new List<StatefulView<T>>();
        public int Count => statesList.Count;
        public bool IsReadOnly => false;
        public T this[int index] {
            get {
                if (index < 0 || index > statesList.Count - 1)
                    throw new Exception("Index out of bounds");
                return statesList[index];
            }
            set {
                if (index < 0 || index > statesList.Count)
                    throw new Exception("Index out of bounds");
                Insert(index, value);
            }
        }

        bool populateOnStart;
        new void Awake() {
            base.Awake();
            populateOnStart = !(statesList.Count == 0);
		}

        void Start() {
			if (populateOnStart) {
                statesList.ForEach(x => {
                    var instance = Instantiate(x);
                    if (instance != null)
                        Instantiated.Add(instance);
                });
			}
        }

        new void Update() {
            base.Update();

            if(Time.frameCount % resizingCheckFrameStep == 0)
                ResizeIfRequired();
        }

        int lastHeight;
        int lastWidth;
        RectTransform childRT;
        int height;
        int width;
        void ResizeIfRequired(){
            height = 0;
            width = 0;
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

        UnityAction<StatefulView<T>> subscription;
        public void SubscribeToChildren(UnityAction<StatefulView<T>> subscription) {
            this.subscription = subscription;
            Instantiated.ForEach(x => this.subscription(x));
        }

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
            (instance as StatefulView<T>).CurrentState = t;

            subscription?.Invoke(instance as StatefulView<T>);

            return instance as StatefulView<T>;
        }

        void Destroy(T t) {
            foreach (var instance in Instantiated) {
                if (instance != null && instance.CurrentState.Equals(t) && instance.gameObject != null) {
                    Instantiated.Remove(instance);
                    MonoBehaviour.Destroy(instance.gameObject);
                    break;
                }
            }
        }

        public void Clear() {
            foreach (var state in statesList)
                Destroy(state);
            statesList.Clear();
        }

        public bool Contains(T item) => statesList.Contains(item);

        public void Add(T item) => Insert(statesList.Count, item);

        public void CopyTo(T[] array, int arrayIndex) =>
            throw new NotImplementedException("ViewList doesn't support CopyTo yet.");

        public bool Remove(T item) {
            if (Contains(item)) {
                Destroy(item);
                statesList.Remove(item);
                return true;
            }
            return false;
        }

        public int IndexOf(T item) {
            return statesList.IndexOf(item);
        }

        public void Insert(int index, T item) {
            if (item == null)
                throw new Exception("Inserted item cannot be null");

            if (index < 0)
                throw new IndexOutOfRangeException("Insert method index cannot be negative");

            if(index > 0 && index > statesList.Count)
                throw new IndexOutOfRangeException("Insert method index out of bounds");

            var instance = Instantiate(item);
            if (instance != null) {
                if(index > 0) {
                    statesList.Insert(index, item);
                    Instantiated.Insert(index, instance);
                    if (instance.transform.GetSiblingIndex() != index)
                        instance.transform.SetSiblingIndex(index);
				}
				else {
                    statesList.Add(item);
                    Instantiated.Add(instance);
				}
            }
        }

        public void RemoveAt(int index) {
            if (index < 0 || index >= statesList.Count)
                throw new IndexOutOfRangeException("RemoveAt method index was out of range");

            var vm = statesList[index];
            Destroy(vm);
            statesList.RemoveAt(index);
        }

        public IEnumerator<T> GetEnumerator() {
            return (IEnumerator<T>)new ViewListEnumerator<T>(statesList.ToArray());
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (IEnumerator<T>)new ViewListEnumerator<T>(statesList.ToArray());
        }
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
