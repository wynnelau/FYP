using UnityEngine;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage the mainMenu UIs
 */
public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuStudent, mainMenuStaff, mainMenuProf, resourceReservationStaff, resourceReservationProf, meetingSchedule;
    public PlayerControls player;

    /*
     * Purpose: Close the respective mainMenu UIs when the "closeMainMenuButton" button is clicked
     * Input: Click on the "closeMainMenuButton" button
     * Output: Close the respective mainMenu UIs and enable player controls
     */
    public void closeMainMenu()
    {
        Debug.Log("MainMenuUI closeMainMenu");
        if (mainMenuStudent.activeSelf == true)
        {
            Debug.Log("MainMenuUI closeMainMenu Student");
            mainMenuStudent.SetActive(false);
        }
        else if (mainMenuStaff.activeSelf == true)
        {
            Debug.Log("MainMenuUI closeMainMenu Staff");
            mainMenuStaff.SetActive(false);
        }
        else if (mainMenuProf.activeSelf == true) 
        {
            Debug.Log("MainMenuUI closeMainMenu Prof");
            mainMenuProf.SetActive(false); 
        }
        player.enabled = true;
    }

    /*
     * Purpose: To open the respective resourceReservation UIs when the "resourceReservation" button is clicked in the respective mainMenu UIs 
     * Input: Click on the "resourceReservation" button in a mainMenu UI (Prof/TA or Staff)
     * Output: Open the respective resourceReservation UIs
     */
    public void openResourceReservation()
    {
        Debug.Log("MainMenuUI openResourceReservation");
        if (mainMenuStaff.activeSelf == true)
        {
            Debug.Log("MainMenuUI openResourceReservation Staff");
            resourceReservationStaff.SetActive(true);
            mainMenuStaff.SetActive(false);
        } 
        else if (mainMenuProf.activeSelf == true)
        {
            Debug.Log("MainMenuUI openResourceReservation Prof");
            resourceReservationProf.SetActive(true);
            mainMenuProf.SetActive(false);
        }
    }

    /*
     * Purpose: To open the meetingScheduleUI when the "meetingSchedule" button is clicked 
     * Input: Click on the "meetingSchedule" button in a mainMenu UI
     * Output: Open the meetingScheduleUI
     */
    public void openMeetingSchedule()
    {
        Debug.Log("MainMenuUI openMeetingSchedule");
        if (mainMenuStaff.activeSelf == true)
        {
            mainMenuStaff.SetActive(false);
        } 
        else if (mainMenuProf.activeSelf == true)
        {
            mainMenuProf.SetActive(false);
        }
        else if (mainMenuStudent.activeSelf == true)
        {
            mainMenuStudent.SetActive(false);
        }
        meetingSchedule.SetActive(true);
    }
}
