using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

/*
 * Location: "Wardrobe" in "MainScene" scene
 * Purpose: Get the user's profile when the user comes into contact with "Wardrobe" and display the userProfile UI accordingly
 */
public class UserProfileWardrobe : MonoBehaviour
{
    public GameObject userProfileStudent, userProfileOthers;
    public InputField displayNameStudent, schoolStudent, courseStudent, yearStudent, descriptionStudent;
    public InputField displayNameOthers, schoolOthers, descriptionOthers;
    public Text errorStudent, errorOthers;
    public PlayerControls player;

    /*
     * Purpose: Call GetUserProfileData() to get the user's profile to open the corresponding userProfile UI when user comes into contact with "Wardrobe"
     * Input: User comes into contact with "Wardrobe"
     * Output: Call GetUserProfileData() 
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("UserProfileWardrobe onTriggerEnter: activateUserProfileUI");
        GetUserProfileData();
        player.enabled = false;
    }

    /*
     * Purpose: Attempt to retrieve the user's profile from the PlayFab database when called by OnTriggerEnter()
     * Input: Called by OnTriggerEnter() when user comes into contact with "Wardrobe"
     * Output: Attempt to retrieve the user's profile from the PlayFab database
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
     * Purpose: Successful attempt to retrieve the user's profile and displays the corresponding userProfile UI
     * Input: Called by GetUserProfileData() when attempt to retrieve the user's profile is successful
     * Output: If user is Student, opens "userProfileStudent" UI 
     *         else if user is Staff or Prof/TA, opens "userProfileOthers" UI
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
            errorStudent.text = "";
            userProfileStudent.SetActive(true);
        } 
        else if (result.Data["Identity"].Value == "Staff" || result.Data["Identity"].Value == "Professor/TA")
        {
            Debug.Log("UserProfileWardrobe GetUserProfileDataSuccess others");
            displayNameOthers.text = result.Data["DisplayName"].Value;
            schoolOthers.text = result.Data["School"].Value;
            descriptionOthers.text = result.Data["Description"].Value;
            errorOthers.text = "";
            userProfileOthers.SetActive(true);
        }
    }

    /*
     * Purpose: Failed attempt to retrieve the user's profile
     * Input: Called by OnTriggerEnter() when attempt to retrieve the user's profile failed
     * Output: Debug.Log("UserProfileWardrobe GetUserProfileDataFail " + error);
     */
    void GetUserProfileDataFail(PlayFabError error)
    {
        Debug.Log("UserProfileWardrobe GetUserProfileDataFail " + error);
    }

}
