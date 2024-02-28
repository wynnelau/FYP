using UnityEngine;

/*
 * Location: Main Scene/ MiddleWall
 * Purpose: Make sure MiddleWall is persistent across scenes
 */

public class DDOLmiddlewall : MonoBehaviour
{
    private static DDOLmiddlewall instance;

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
            Debug.Log("DDOLmiddlewall Awake InstanceIsNull");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("DDOLmiddlewall Awake DestroyGameObject");
        }
    }
}
