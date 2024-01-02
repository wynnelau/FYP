using UnityEngine;

/*
 * Location: Main Scene/ Bed
 * Purpose: Open the "logOut" UI when user comes into contact with "Bed"
 *          "logOut" UI is used to quit the application
 */

public class LogOutBed : MonoBehaviour
{
    private static LogOutBed instance;
    public GameObject logOut;
    public PlayerControls player;

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
            Debug.Log("LogOutBed Awake InstanceIsNull");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("LogOutBed Awake DestroyGameObject");
        }
    }

    /*
     * Purpose: Open the "logOut" UI when user comes into contact with "Bed"
     * Input: User comes into contact with "Bed"
     * Output: Open the "logOut" UI and disable player movements
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("LogOutBed onTriggerEnter");
        logOut.SetActive(true);
        player.enabled = false;
    }
}
