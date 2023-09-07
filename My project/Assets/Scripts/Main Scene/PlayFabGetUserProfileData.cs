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
    List<string> getUserIdentity = new List<string> { "Identity" };
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
        GetUserIdentityData();
    }

    /*
     * Purpose: Read user profile data from database
     * Outcomes: if successful, data read
     */

    public void GetUserIdentityData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = getUserIdentity
        }, GetUserIdentityDataSuccess, GetUserIdentityDataFail);
    }

    void GetUserIdentityDataSuccess(GetUserDataResult result)
    {
        if (result.Data["Identity"].Value == "Student")
        {
            Debug.Log("GetUserIdentityDataSuccess student");
            GetUserProfileData();
            userProfileStudent.SetActive(true);
        } 
        else if (result.Data["Identity"].Value == "Staff" || result.Data["Identity"].Value == "Professor/TA")
        {
            Debug.Log("GetUserIdentityDataSuccess others");
            userProfileOthers.SetActive(true);
        }
    }

    void GetUserIdentityDataFail(PlayFabError error)
    {
        Debug.Log(error);
    }

    /*
     * Purpose: Read user profile data from database
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
     * Purpose: Read user profile data from database
     * Outcomes: if successful, data read
     */
    void GetUserProfileDataSuccess(GetUserDataResult result)
    {
        Debug.Log("GetUserProfileDataSuccess student");
        if (result.Data["Identity"].Value == "Student")
        {
            displayNameStudent.text = result.Data["DisplayName"].Value;
            schoolStudent.text = result.Data["School"].Value;
            courseStudent.text = result.Data["Course"].Value;
            yearStudent.text = result.Data["Year"].Value;
            descriptionStudent.text = result.Data["Description"].Value;
        }
    }

    void GetUserProfileDataFail(PlayFabError error)
    {
        Debug.Log(error);
    }
}
