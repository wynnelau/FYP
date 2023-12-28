using UnityEngine;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Deactivate the UIs when user enters "MainScene" scene
 */

public class SceneInitialise : MonoBehaviour
{
    public GameObject logoutUI, userProfileStudentUI, userProfileOthersUI, mainMenuStudentUI, mainMenuStaffUI, mainMenuProfUI;
    public GameObject resourceReservationProfUI, resourceReservationStaffUI, dateDetailsProfUI, dateDetailsStaffUI;
    public GameObject timeDetailsProf, timeDetailsStaff, joinMeetingUI;
    public GameObject manageSlotsUI, meetingScheduleUI, meetingDetailsUI, newMeetingUI;
    
    /*
     * Purpose: Deactivates all the UIs when the user enters "MainScene" scene
     *          Start is called before the first frame update
     * Input: NA
     * Output: deactivates all the UIs
     */
    void Start()
    {
        logoutUI.SetActive(false);
        userProfileStudentUI.SetActive(false);
        userProfileOthersUI.SetActive(false);
        mainMenuStudentUI.SetActive(false);
        mainMenuStaffUI.SetActive(false);
        mainMenuProfUI.SetActive(false);
        resourceReservationStaffUI.SetActive(false);
        resourceReservationProfUI.SetActive(false);
        dateDetailsProfUI.SetActive(false);
        dateDetailsStaffUI.SetActive(false);
        manageSlotsUI.SetActive(false);
        timeDetailsProf.SetActive(false);
        timeDetailsStaff.SetActive(false);
        joinMeetingUI.SetActive(false);
        meetingScheduleUI.SetActive(false);
        meetingDetailsUI.SetActive(false);
        newMeetingUI.SetActive(false);
    }

}
