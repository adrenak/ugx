using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using Adrenak.Unex;

#if UGX_NAUGHTY_AVAILABLE
using NaughtyAttributes;
#endif

namespace Adrenak.UGX {
    public abstract class Navigator : MonoBehaviour {
        [System.Serializable]
        public class WindowUnityEvent : UnityEvent<Window> { }

        static Dictionary<string, Navigator> map = new Dictionary<string, Navigator>();
        public static Navigator Get(string browserID = null) {
            if (map.ContainsKey(browserID))
                return map[browserID];
            return null;
        }

        public WindowUnityEvent onPush = new WindowUnityEvent();
        public WindowUnityEvent onPop = new WindowUnityEvent();

#pragma warning disable 0649
        [SerializeField] string browserID;
        [SerializeField] bool canPopAll;

#if UGX_NAUGHTY_AVAILABLE
        [ReadOnly]
#endif
        [SerializeField] protected Window current = null;
        public Window Current => current;

#if UGX_NAUGHTY_AVAILABLE
        [ReadOnly] [ReorderableList]
#endif
        [SerializeField] protected List<Window> history = new List<Window>();
        public List<Window> History => history;

        [SerializeField] protected bool useInitialWindow;
#if UGX_NAUGHTY_AVAILABLE
        [ShowIf("useInitialWindow")]
#endif
        [SerializeField] protected Window initialWindow;
#pragma warning restore 0649

        // ================================================
        // UNITY LIFECYCLE
        // ================================================
        void Awake() => RegisterInstance();
        void Update() => CheckBackPress();
        void OnDestroy() => UnregisterInstance();

        void RegisterInstance() {
            if (map.ContainsKey(browserID)) {
                Debug.LogError("There is already a Browser with ID " + browserID + ". IDs should be unique");
                return;
            }
            map.Add(browserID, this);
        }

        void UnregisterInstance() {
            if (map.ContainsKey(browserID))
                map.Remove(browserID);
        }

        void CheckBackPress() {
            if (!Input.GetKeyUp(KeyCode.Escape)) return;
            Pop();
        }

        // ================================================
        // API / PUBLIC
        // ================================================
        /// <summary>
        /// Push a Window to the navigation and open it.
        /// </summary>
        public void Push(Window window) {
            PushImpl(window);
        }

        /// <summary>
        /// Pops a Window from the navigation and closes it
        /// </summary>
        public void Pop() {
            if (History.Count == 1 && !canPopAll)
                return;
            PopImpl();
        }

        /// <summary>
        /// Pops all Windows
        /// </summary>
        public void Clear() {
            while (History.Count > 0)
                PopImpl();
        }

        /// <summary>
        /// Pops a Window if it's active, Push it if it's not.
        /// </summary>
        public void Toggle(Window window) {
            if (History.Count > 0 && History.Last() == window)
                Pop();
            else
                Push(window);
        }

        // ================================================
        // CONTRACT
        // ================================================
        /// <summary>
        /// Actual Push logic to be implemented by subclass
        /// </summary>
        /// <param name="window"></param>
        protected abstract void PushImpl(Window window);

        /// <summary>
        /// Actual Pop logic to be implemented by subclass
        /// </summary>
        protected abstract void PopImpl();

        // ================================================
        protected void SetAsCurrent(Window window) {
            window.OpenWindow();
            if (current != null)
                current.CloseWindow();
            current = window;
        }
    }
}
