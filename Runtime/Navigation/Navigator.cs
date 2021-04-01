using NaughtyAttributes;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Adrenak.UGX {
    public abstract class Navigator : MonoBehaviour {
        Stack<Navigator> navigatorStack = new Stack<Navigator>();

        static Dictionary<string, Navigator> map = new Dictionary<string, Navigator>();
        public static Navigator Get(string navigatorID = null) {
            if (map.ContainsKey(navigatorID))
                return map[navigatorID];
            return null;
        }

#pragma warning disable 0649
        [SerializeField] string navigatorID;
        [SerializeField] bool clearOnClose;
        [SerializeField] bool canPopAll;

        [ReadOnly] [SerializeField] protected Window active = null;
        public Window Current => active;

        [ReadOnly] [ReorderableList] [SerializeField] protected List<Window> history = new List<Window>();
        public List<Window> History => history;

        public UnityEvent onPush;
        public UnityEvent onPop;

        [SerializeField] protected bool useInitialWindow;
        [ShowIf("useInitialWindow")] [SerializeField] protected Window initialWindow;
#pragma warning restore 0649

        // ================================================
        // UNITY LIFECYCLE
        // ================================================
        void Awake() => RegisterInstance();
        void Update() => CheckBackPress();
        void OnDestroy() => UnregisterInstance();

        void RegisterInstance() {
            if (map.ContainsKey(navigatorID)) {
                Debug.LogError("There is already a Navigator with ID " + navigatorID + ". IDs should be unique");
                return;
            }
            map.Add(navigatorID, this);
        }

        void UnregisterInstance() {
            if (map.ContainsKey(navigatorID))
                map.Remove(navigatorID);
        }

        void CheckBackPress() {
            if (!Input.GetKeyUp(KeyCode.Escape)) return;
            if (navigatorStack.Peek() != this) return;
            if (!active.autoPopOnBack) return;
            active.GoBack();
        }

        protected void SetAsActive(Window window) {
            window.OpenWindow();
            if (active != null)
                active.CloseWindow();
            active = window;
        }

        public void Push(Window window) {
            if (navigatorStack.Peek() != this) 
                navigatorStack.Peek().Clear();

            if (!navigatorStack.Contains(this))
                navigatorStack.Push(this);
            
            PushImpl(window);
        }

        public void Pop() {
            if (navigatorStack.Peek() != this)
                return;

            if (History.Count == 1 && !canPopAll)
                return;

            PopImpl();
            if (History.Count == 0)
                navigatorStack.Pop();
        }

        public void Clear() {
            while (History.Count > 0)
                PopImpl();
        }

        protected abstract void PushImpl(Window window);

        protected abstract void PopImpl();
    }
}
