using UnityEngine;

/*
 * Location: "Bed" in "MainScene" scene
 * Purpose: Open the "logoutUI" UI when user comes into contact with "Bed"
 */

public class LogOutBed : MonoBehaviour
{
    public GameObject logoutUI;
    public PlayerControls player;

    /*
     * Purpose: Open the "logoutUI" UI when user comes into contact with "Bed"
     * Input: User comes into contact with "Bed"
     * Output: Open the "logoutUI" UI
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("LogOutBed onTriggerEnter: activateLogoutUI");
        logoutUI.SetActive(true);
        player.enabled = false;
    }
}
