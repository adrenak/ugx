using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public RectTransform rt;

    private void Update() {
        //Debug.Log(rt.localPosition);
        Debug.Log(rt.GetLeftExit());
        Debug.Log(rt.GetRightExit());
        Debug.Log(rt.GetTopExit());
        Debug.Log(rt.GetBottomExit());
    }
}
