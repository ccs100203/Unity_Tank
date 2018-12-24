using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class isInside : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    
    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.name == "Zoom(Clone)")
        {
            Debug.Log(c.gameObject.name + " Stay");
            Inside = true;
        }
    }
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "Zoom(Clone)")
        {
            Debug.Log(c.gameObject.name+ " Enter");
            Inside = true;
        }
    }
    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.name == "Zoom(Clone)")
        {
            Debug.Log(c.gameObject.name + " Exit");
            Inside = false;
        }
    }
    private bool Inside = true;
}