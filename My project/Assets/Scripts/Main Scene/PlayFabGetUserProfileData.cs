using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/*
 *Location: Main Scene, userProfileUI
 *Purpose: Trigger the different profile pages according to their identity, attached to "Wardrobe"
 */

public class PlayFabGetUserProfileData : MonoBehaviour
{
    public GameObject userProfileStudent, userProfileOthers;
    public InputField displayNameStudent, schoolStudent, courseStudent, yearStudent, descriptionStudent;
    public InputField displayNameOthers, schoolOthers, descriptionOthers;
    /*List<string> getUserIdentity = new List<string> { "Identity" };*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Purpose: 
     * Outcomes: if successful, data read
     */
    private void OnTriggerEnter(Collider other)
    {
        GetUserProfileData();
    }

    /*
     * Purpose: Read user identity data from database
     * Outcomes: if successful, data read
     */
    public void GetUserProfileData()
    {
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
        if (result.Data["Identity"].Value == "Student")
        {
            Debug.Log("GetUserIdentityDataSuccess student");
            displayNameStudent.text = result.Data["DisplayName"].Value;
            schoolStudent.text = result.Data["School"].Value;
            courseStudent.text = result.Data["Course"].Value;
            yearStudent.text = result.Data["Year"].Value;
            descriptionStudent.text = result.Data["Description"].Value;
            userProfileStudent.SetActive(true);
        } 
        else if (result.Data["Identity"].Value == "Staff" || result.Data["Identity"].Value == "Professor/TA")
        {
            Debug.Log("GetUserIdentityDataSuccess others");
            displayNameOthers.text = result.Data["DisplayName"].Value;
            schoolOthers.text = result.Data["School"].Value;
            descriptionOthers.text = result.Data["Description"].Value;
            userProfileOthers.SetActive(true);
        }
    }

    void GetUserProfileDataFail(PlayFabError error)
    {
        Debug.Log(error);
    }
}
