using UnityEngine;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage the LogOutUI
 */

public class LogOutUI : MonoBehaviour
{
    public GameObject logOut;
    public PlayerControls player;

    /*
     * Purpose: Quit application when the "logoutConfirmButton" button is clicked
     * Input: Click on the "logoutConfirmButton" button
     * Output: Quit application
     */
    public void LogoutConfirm()
    {
        Application.Quit();
    }

    /*
     * Purpose: Close the "logOut" UI when the "logoutCancelButton" button is clicked
     * Input: Click on the "logoutCancelButton" button
     * Output: Set the "logOut" UI to inactive and enable player controls
     */
    public void LogoutCancel()
    {
        logOut.SetActive(false);
        player.enabled = true;
    }
}
