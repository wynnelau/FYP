using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLbuildingFirstPerson : MonoBehaviour
{
    private static DDOLbuildingFirstPerson instance;

    /*
     * Purpose: Make sure the gameObject is persistent across scenes
     * Input: NA
     * Output: If instance is null, let instance = this and DontDestroyOnLoad
     *         else destroy the gameObject
     */
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("DDOLbuildingFirstPerson Awake InstanceIsNull");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("DDOLbuildingFirstPerson Awake DestroyGameObject");
        }
    }
}
