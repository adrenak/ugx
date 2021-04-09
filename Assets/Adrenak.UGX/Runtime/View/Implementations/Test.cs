using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    List<int> list = new List<int>();
    void Start()
    {
        list = new List<int>{
            1, 2, 3
        };
        list.Insert(4, 89);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
