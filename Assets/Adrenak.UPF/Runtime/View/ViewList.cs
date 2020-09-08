using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;

namespace Adrenak.UPF {
    [Serializable]
    public class ViewList<TModel, TView> : MonoBehaviour, ICollection<TModel>, IList<TModel> where TModel : ViewModel where TView : View<TModel> {
#pragma warning disable 0649
        public Transform container;
        public TView template;
        [ReadOnly] [SerializeField] List<TModel> viewModels = new List<TModel>();
#pragma warning restore 0649

        public List<View<TModel>> Instantiated { get; private set; } = new List<View<TModel>>();
        public int Count => viewModels.Count;
        public bool IsReadOnly => false;
        public TModel this[int index] {
            get => viewModels[index];
            set {
                viewModels[index] = value;
                Instantiated[index].ViewModel = value;
            }
        }

        Action<TModel> onInit, onDeinit;
        public void Setup(Action<TModel> onInit, Action<TModel> onDeinit) {
            this.onInit = onInit;
            this.onDeinit = onDeinit;
        }

        View<TModel> Instantiate(TModel t) {
            if (template == null)
                throw new Exception("No ViewTemplate assigned! Cannot instantiate elements in ViewGroup.");

            // We don't initialize when the application isn't playing.
            // This sometimes happens with requests that are fullfilled after
            // play mode exits in the editor and end up instantiation in editor mode.
            if (!Application.isPlaying)
                return null;

            var instance = Instantiate(template, container);
            instance.hideFlags = HideFlags.DontSave;
            instance.ViewModel = t;

            onInit?.Invoke(t);

            return instance;
        }

        void Destroy(TModel t) {
            foreach (var instance in Instantiated) {
                if (instance != null && instance.ViewModel.Equals(t) && instance.gameObject != null) {
                    onDeinit?.Invoke(instance.ViewModel);
                    Instantiated.Remove(instance);
                    Destroy(instance.gameObject);
                    break;
                }
            }
        }

        public void Clear() {
            foreach (var model in viewModels)
                Destroy(model);
            viewModels.Clear();
        }

        public bool Contains(TModel item) {
            return viewModels.Contains(item);
        }

        public void Add(TModel item) {
            viewModels.Add(item);
            var instance = Instantiate(item);
            if (instance != null)
                Instantiated.Add(instance);
        }

        public void CopyTo(TModel[] array, int arrayIndex) {
            throw new NotImplementedException("ViewList doesn't support CopyTo yet.");
        }

        public bool Remove(TModel item) {
            if (Contains(item)) {
                Destroy(item);
                viewModels.Remove(item);
                return true;
            }
            return false;
        }

        public int IndexOf(TModel item) {
            return viewModels.IndexOf(item);
        }

        public void Insert(int index, TModel item) {
            if (item == null)
                throw new Exception("Inserted item cannot be null");

            if (index < 0 || index >= viewModels.Count)
                throw new IndexOutOfRangeException("Insert method index was out of range");

            viewModels.Insert(index, item);
            var instance = Instantiate(item);
            Instantiated.Insert(index, instance);
        }

        public void RemoveAt(int index) {
            if (index < 0 || index >= viewModels.Count)
                throw new IndexOutOfRangeException("RemoveAt method index was out of range");

            var vm = viewModels[index];
            Destroy(vm);
            viewModels.RemoveAt(index);
        }

        public IEnumerator<TModel> GetEnumerator() {
            return (IEnumerator<TModel>)new ViewGroupEnum<TModel>(viewModels.ToArray());
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (IEnumerator<TModel>)new ViewGroupEnum<TModel>(viewModels.ToArray());
        }
    }

    // When you implement IEnumerable, you must also implement IEnumerator.
    public class ViewGroupEnum<T> : IEnumerator where T : ViewModel {
        public T[] array;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public ViewGroupEnum(T[] list) {
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
