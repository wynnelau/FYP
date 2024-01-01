using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    private static DDOL instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Awake Canvas created: " + gameObject.name);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Awake Canvas destroyed: " + gameObject.name);
        }
    }

}
