using UnityEngine;

/*
 * Location: Main Scene/ MainSceneCanvas
 * Purpose: Make sure MainSceneCanvas is persistent across scenes
 */

public class DDOL : MonoBehaviour
{
    private static DDOL instance;

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
            Debug.Log("DDOL Awake InstanceIsNull " + gameObject.name);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("DDOL Awake DestroyGameObject " + gameObject.name);
        }
    }

}
