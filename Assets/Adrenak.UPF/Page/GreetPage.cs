using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreetPage : Page {
    Text text;
    Slider slider;
    Button button;

    protected override void InitializePage() {
        text = GetPageElement<Text>("text");
        slider = GetPageElement<Slider>("slider");
        button = GetPageElement<Button>("button");

        slider.onValueChanged.AddListener(value => text.text = value.ToString("0.00"));
        button.onClick.AddListener(() => DisplayAlert("Button Clicked"));

        slider.value = .5f;
    }
}
