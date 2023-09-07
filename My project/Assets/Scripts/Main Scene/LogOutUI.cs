using UnityEngine;

/*
 *Location: Main Scene, LogoutUI
 *Purpose: Logout when user clicks confirm in the logoutUI, attached to "StudentControls"
 */

public class LogOutUI : MonoBehaviour
{
    public GameObject logoutUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Purpose: Quit application when button pressed, used by "logoutConfirmButton"
     * Outcomes: quit application
     */
    public void logoutConfirm()
    {
        Application.Quit();
    }

    /*
     * Purpose: Deactivate logoutUI when button pressed, used by "logoutCancelButton"
     * Outcomes: deactivate logoutUI
     */
    public void logoutCancel()
    {
        logoutUI.SetActive(false);
    }
}
