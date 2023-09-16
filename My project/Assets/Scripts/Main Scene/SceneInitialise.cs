using UnityEngine;

/*
 *Location: Main Scene, attached to "StudentControls"
 *Purpose: deactivate the UIs when user enters
 */

public class SceneInitialise : MonoBehaviour
{
    public GameObject logoutUI, userProfileStudentUI, userProfileOthersUI, mainMenuStudentUI, mainMenuOthersUI;
    public GameObject resourceReservationUI;
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
        mainMenuOthersUI.SetActive(false);
        resourceReservationUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
