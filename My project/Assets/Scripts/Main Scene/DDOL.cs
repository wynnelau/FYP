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
            Debug.Log("DDOL Awake InstanceIsNull " + gameObject.name);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("DDOL Awake DestroyGameObject " + gameObject.name);
        }
    }

}
