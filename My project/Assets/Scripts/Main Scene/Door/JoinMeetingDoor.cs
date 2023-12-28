using UnityEngine;

/*
 * Location: Main Scene/ Door
 * Purpose: Open the "joinMeetingUI" UI when user comes into contact with "Door"
 */
public class JoinMeetingDoor : MonoBehaviour
{
    public GameObject joinMeetingUI;
    public PlayerControls player;
    /*
     * Purpose: Open the JoinMeetingUI when user comes into contact with "Door"
     * Input: User comes into contact with "Door"
     * Output: Open the JoinMeetingUI and disable player movements
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("JoinMeetingDoor OnTriggerEnter");
        joinMeetingUI.SetActive(true);
        player.enabled = false;
    }

    
}
