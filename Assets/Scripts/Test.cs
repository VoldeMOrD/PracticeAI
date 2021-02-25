using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ships;
public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera mainc = Camera.main;
        ShipPlayer test = new ShipPlayer();
        mainc.gameObject.AddComponent(typeof(SmoothCamera));
        mainc.gameObject.GetComponent<SmoothCamera>().Target = test.Prefab.transform;
        mainc.gameObject.GetComponent<SmoothCamera>().SetValueFromBounds(test.GetBounds(),3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
