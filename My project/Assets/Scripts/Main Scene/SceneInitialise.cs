using UnityEngine;

/*
 *Location: Main Scene, attached to "StudentControls"
 *Purpose: deactivate the UIs when user enters
 */

public class SceneInitialise : MonoBehaviour
{
    public GameObject logoutUI, userProfileStudentUI, userProfileOthersUI, mainMenuStudentUI, mainMenuStaffUI, mainMenuProfUI;
    public GameObject resourceReservationProfUI, resourceReservationStaffUI, dateDetailsProfUI, dateDetailsStaffUI;
    public GameObject timeDetailsProf, timeDetailsStaff;
    public GameObject manageSlotsUI;
    // Start is called before the first frame update
    /*
     * Purpose: deactivates all the UIs when the user enters
     * Outcomes: 
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
