using UnityEngine;
using UnityEngine.UI;

public class TextListItem : ListItem {
    [SerializeField] Text mainText;
    [SerializeField] Text detailText;
    [SerializeField] Image icon;

    public string MainText {
        get => mainText.text;
        set => mainText.text = value;
    }

    public string DetailText {
        get => detailText.text;
        set => detailText.text = value;
    }

    public Sprite Icon {
        get => icon.sprite;
        set => icon.sprite = value;
    }
}
