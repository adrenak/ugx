﻿using UnityEngine;

namespace Adrenak.UPF {
    public class UIBehaviour : MonoBehaviour {
        public View view => GetComponent<View>();
        public Window window => GetComponent<Window>();
        public PositionTransitioner positionTransitioner => GetComponent<PositionTransitioner>();
        public OpacityTransitioner opacityTransitioner => GetComponent<OpacityTransitioner>();
    }
}
