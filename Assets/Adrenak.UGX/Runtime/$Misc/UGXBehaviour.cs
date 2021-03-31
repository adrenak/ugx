using UnityEngine;

namespace Adrenak.UGX {
    public class UGXBehaviour : MonoBehaviour {
        public View view => GetComponent<View>();
        public Window window => GetComponent<Window>();
        public PositionTransitioner positionTransitioner => GetComponent<PositionTransitioner>();
        public OpacityTransitioner opacityTransitioner => GetComponent<OpacityTransitioner>();
    }
}
