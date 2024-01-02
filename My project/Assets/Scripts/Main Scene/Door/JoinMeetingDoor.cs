using UnityEngine;

/*
 * Location: Main Scene/ Door
 * Purpose: Open the "joinMeeting" UI when user comes into contact with "Door"
 *          "joinMeeting" UI is used to join a meeting using a join code
 */
public class JoinMeetingDoor : MonoBehaviour
{
    private static JoinMeetingDoor instance;
    public GameObject joinMeeting;
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
            Debug.Log("JoinMeetingDoor Awake InstanceIsNull");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("JoinMeetingDoor Awake DestroyGameObject");
        }
    }

    /*
     * Purpose: Open the "joinMeeting" UI when user comes into contact with "Door"
     * Input: User comes into contact with "Door"
     * Output: Open the "joinMeeting" UI and disable player movements
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("JoinMeetingDoor OnTriggerEnter");
        joinMeeting.SetActive(true);
        player.enabled = false;
    }

    
}
