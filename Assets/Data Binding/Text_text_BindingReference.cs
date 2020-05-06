using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Text_text_BindingReference : BindingReference<Text> {
    public override void OnPulled(object obj) {
        destination.text = (string)obj;
    }
}
