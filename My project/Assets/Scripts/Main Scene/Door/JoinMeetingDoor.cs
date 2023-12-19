using UnityEngine;

public class JoinMeetingDoor : MonoBehaviour
{
    public GameObject joinMeetingUI;
    public PlayerControls player;
    /*
     * Purpose: Open the JoinMeetingUI when user comes into contact with "Door"
     * Input: User comes into contact with "Door"
     * Output: Open the JoinMeetingUI 
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("JoinMeetingDoor OnTriggerEnter: activateJoinMeetingUI");
        joinMeetingUI.SetActive(true);
        player.enabled = false;
    }

    
}
