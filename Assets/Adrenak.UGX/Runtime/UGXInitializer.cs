using Adrenak.Unex;

using UnityEngine;

namespace Adrenak.UGX {
    public class UGXInitializer : MonoBehaviour {
        static UGXInitializer instance;

        void Awake() {
            Init();
        }

        static void EnsureInstance() {
            if (instance == null)
                instance = FindObjectOfType<UGXInitializer>();
            if (instance == null)
                instance = new GameObject("UGX Initializer").AddComponent<UGXInitializer>();
        }

        public static void Init() {
            EnsureInstance();
            Dispatcher.Init();
            Runnable.Init();
        }
    }
}
