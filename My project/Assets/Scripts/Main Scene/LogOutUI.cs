using UnityEngine;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage the LogOutUI
 */

public class LogOutUI : MonoBehaviour
{
    public GameObject logoutUI;
    public PlayerControls player;

    /*
     * Purpose: Quit application when the "logoutConfirmButton" button is clicked
     * Input: Click on the "logoutConfirmButton" button
     * Output: Quit application
     */
    public void logoutConfirm()
    {
        Application.Quit();
    }

    /*
     * Purpose: Close the "logoutUI" UI when the "logoutCancelButton" button is clicked
     * Input: Click on the "logoutCancelButton" button
     * Output: Set the "logoutUI" UI to inactive and enable player controls
     */
    public void logoutCancel()
    {
        logoutUI.SetActive(false);
        player.enabled = true;
    }
}
