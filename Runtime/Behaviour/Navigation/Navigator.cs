using NaughtyAttributes;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using Adrenak.Unex;

namespace Adrenak.UGX {
    public abstract class Navigator : MonoBehaviour {
        static Dictionary<string, Navigator> map = new Dictionary<string, Navigator>();
        public static Navigator Get(string navigatorID = null) {
            if (map.ContainsKey(navigatorID))
                return map[navigatorID];
            return null;
        }

#pragma warning disable 0649
        [SerializeField] string navigatorID;
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
            if (!active.CurrentState.autoPopOnBack) return;
            Pop();
        }

        protected void SetAsActive(Window window) {
            window.OpenWindow();
            if (active != null)
                active.CloseWindow();
            active = window;
        }

        async public void Push(Window window) {
            var interrupt = await window.ApprovePush();
            if(interrupt)
                PushImpl(window);
        }

        async public void Pop() {
            if (History.Count == 1 && !canPopAll)
                return;

            var interrupt = await History.Last().ApprovePop();
            if(interrupt)
                PopImpl();
        }

        public void Clear() {
            while (History.Count > 0)
                PopImpl();
        }

        protected abstract void PushImpl(Window window);

        protected abstract void PopImpl();
    }
}
