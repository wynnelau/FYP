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
    public GameObject userProfileStudent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Purpose: To close the User profile UI
     * Outcomes: deactivate user profile
     */
    public void closeUserProfile()
    {
        userProfileStudent.SetActive(false);
    }

    
}
