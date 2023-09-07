using UnityEngine;

/*
 *Location: Main Scene
 *Purpose: deactivate the UIs when user enters, attached to "StudentControls"
 */

public class SceneInitialise : MonoBehaviour
{
    public GameObject logoutUI, userProfileStudentUI, userProfileOthersUI;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
