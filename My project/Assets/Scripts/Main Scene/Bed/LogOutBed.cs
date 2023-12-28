using UnityEngine;

/*
 * Location: Main Scene/ Bed
 * Purpose: Open the "logoutUI" UI when user comes into contact with "Bed"
 */

public class LogOutBed : MonoBehaviour
{
    public GameObject logoutUI;
    public PlayerControls player;

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
