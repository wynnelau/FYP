using UnityEngine;

/*
 * Location: Main Scene/ MainCamera
 * Purpose: Make sure MainCamera is persistent across scenes
 */

public class DDOLmainCamera : MonoBehaviour
{
    private static DDOLmainCamera instance;

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
            Debug.Log("DDOLmainCamera Awake InstanceIsNull");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("DDOLmainCamera Awake DestroyGameObject");
        }
    }
}
