using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/*
 *Location: Main Scene, attached to "Wardrobe"
 *Purpose: Trigger the different profile pages according to their identity
 */

public class UserProfileWardrobe : MonoBehaviour
{
    public GameObject userProfileStudent, userProfileOthers;
    public InputField displayNameStudent, schoolStudent, courseStudent, yearStudent, descriptionStudent;
    public InputField displayNameOthers, schoolOthers, descriptionOthers;
    public PlayerControls player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Purpose: Display user profile UI
     * Outcomes: if successful, data read
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("UserProfileWardrobe onTriggerEnter activateUserProfileUI");
        GetUserProfileData();
        player.enabled = false;
    }

    /*
     * Purpose: Read user identity data from database
     * Outcomes: if successful, data read
     */
    public void GetUserProfileData()
    {
        Debug.Log("UserProfileWardrobe GetUserProfileData");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, GetUserProfileDataSuccess, GetUserProfileDataFail);
    }

    /*
     * Purpose: Display the User profile UI according to their identity
     * Outcomes: if "student", display the student ver 
     *           else, display others ver
     */
    void GetUserProfileDataSuccess(GetUserDataResult result)
    {
        Debug.Log("UserProfileWardrobe GetUserProfileDataSuccess");
        if (result.Data["Identity"].Value == "Student")
        {
            Debug.Log("UserProfileWardrobe GetUserProfileDataSuccess student");
            displayNameStudent.text = result.Data["DisplayName"].Value;
            schoolStudent.text = result.Data["School"].Value;
            courseStudent.text = result.Data["Course"].Value;
            yearStudent.text = result.Data["Year"].Value;
            descriptionStudent.text = result.Data["Description"].Value;
            userProfileStudent.SetActive(true);
        } 
        else if (result.Data["Identity"].Value == "Staff" || result.Data["Identity"].Value == "Professor/TA")
        {
            Debug.Log("UserProfileWardrobe GetUserProfileDataSuccess others");
            displayNameOthers.text = result.Data["DisplayName"].Value;
            schoolOthers.text = result.Data["School"].Value;
            descriptionOthers.text = result.Data["Description"].Value;
            userProfileOthers.SetActive(true);
        }
    }

    void GetUserProfileDataFail(PlayFabError error)
    {
        Debug.Log("UserProfileWardrobe GetUserProfileDataFail " + error);
    }

}
