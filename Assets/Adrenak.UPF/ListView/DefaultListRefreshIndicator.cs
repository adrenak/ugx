﻿using UnityEngine;
using UnityEngine.UI;

namespace Adrenak.UPF {
    public class DefaultListRefreshIndicator : ListRefreshIndicator {
        [SerializeField] Text message;
        [SerializeField] Image spinner;

        void Start() {
            SetValue(0);
            SetRefreshing(false);
        }

        public override void SetRefreshing(bool state) {
            if (state)
                message.text = "Refreshing...";
            else
                message.text = "Pull to refresh...";

            spinner.enabled = state;
        }

        public override void SetValue(float value) {
            if (value > 1)
                message.text = "Now let go to refresh";
            else
                message.text = "Pull to refresh";

            message.color = new Color(
                message.color.r,
                message.color.g,
                message.color.b,
                value
            );            
        }
    }
}