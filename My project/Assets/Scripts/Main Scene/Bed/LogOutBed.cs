using UnityEngine;

/*
 * Location: Main Scene/ Bed
 * Purpose: Open the "logoutUI" UI when user comes into contact with "Bed"
 */

public class LogOutBed : MonoBehaviour
{
    private static LogOutBed instance;
    public GameObject logoutUI;
    public PlayerControls player;

    /*
     * Purpose: Make sure the gameObject is persistent across scenes
     * Input: NA
     * Output: If instance is null, create gameObject and DontDestroyOnLoad
     *         else destroy the gameObject
     */
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("LogOutBed Awake InstanceIsNull");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("LogOutBed Awake DestroyGameObject");
        }
    }

    /*
     * Purpose: Open the "logoutUI" UI when user comes into contact with "Bed"
     * Input: User comes into contact with "Bed"
     * Output: Open the "logoutUI" UI and disable player movements
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("LogOutBed onTriggerEnter");
        logoutUI.SetActive(true);
        player.enabled = false;
    }
}
