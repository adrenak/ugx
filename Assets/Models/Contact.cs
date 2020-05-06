using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using PropertyChanged;

namespace SomeNamespace {
    public class Contact : BindingModel {
        public string name;
        public string description;
        public Sprite displayPic;
        public Color fontColor;
    }
}

public abstract class BindingModel {
    public event Action<string> Updated;

    public void Update(params string[] ids) {
        foreach(var id in ids)
            Updated?.Invoke(id);
    }
}