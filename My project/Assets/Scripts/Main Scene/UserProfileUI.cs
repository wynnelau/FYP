using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/*
 *Location: Main Scene, UserProfileUI
 *Purpose: read user profile data from database and display, attached to "StudentControls"
 */

public class UserProfileUI : MonoBehaviour
{
    public GameObject userProfileStudent, userProfileOthers;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Purpose: To close the User profile UI, attached to "closeUserProfileButton"
     * Outcomes: deactivate user profile
     */
    public void closeUserProfile()
    {
        if (userProfileStudent.activeSelf == true) 
        {
            userProfileStudent.SetActive(false);
        }
        else if (userProfileOthers.activeSelf == true) 
        { 
            userProfileOthers.SetActive(false); 
        }
    }

    
}
