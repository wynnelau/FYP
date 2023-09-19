using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*
 *Location: Main Scene, attached to "Controls"
 *Purpose: read user profile data from database and display
 */

public class UserProfileUI : MonoBehaviour
{
    public GameObject userProfileStudent, userProfileOthers;
    public Text displayNameStudent, schoolStudent, courseStudent, yearStudent, descriptionStudent;
    public Text displayNameOthers, schoolOthers, descriptionOthers;
    public Text errorStudent, errorOthers;
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
        player.enabled = true;
    }

    /*
     * Purpose: To close the User profile UI, attached to "updateProfileButton"
     * Outcomes: deactivate user profile
     */
    public void UpdateStudentProfile()
    {
        Debug.Log("Entered UpdateStudentProfile");
        if (userProfileStudent.activeSelf)
        {
            if (CheckMissingInputsStudent()) 
            {
                Debug.Log("Missing inputs student");
                errorStudent.text = "Error. Display name, School, Course and/or Year of study cannot be empty.";
            }
            else
            {
                Debug.Log("UpdateStudentProfile");
                errorStudent.text = "";
                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>
                    {
                        {"DisplayName", displayNameStudent.text},
                        {"School", schoolStudent.text},
                        {"Course", courseStudent.text},
                        {"Year", yearStudent.text},
                        {"Description", descriptionStudent.text}
                    }
                };
                PlayFabClientAPI.UpdateUserData(request, UpdateStudentProfileSuccess, UpdateStudentProfileFail);
            }

        }
    }

    void UpdateStudentProfileSuccess(UpdateUserDataResult result)
    {
        Debug.Log("SaveStudentInfoSuccess");
        errorStudent.text = "Saved successfully";
        
    }

    void UpdateStudentProfileFail(PlayFabError error)
    {
        Debug.Log(error);
        errorStudent.text = "Error. Try again";
    }

    /*
     * Purpose: Checks whether the user (student) has inputs for all except for description
     * Outcomes: returns true if there is missing inputs
     */
    bool CheckMissingInputsStudent()
    {
        if (displayNameStudent.text == "" || schoolStudent.text == "" || courseStudent.text == "" || yearStudent.text == "")
        {
            return true;
        }
        return false;
    }

    /*
     * Purpose: To close the User profile UI, attached to "updateProfileButton"
     * Outcomes: deactivate user profile
     */
    public void UpdateOthersProfile()
    {
        Debug.Log("Entered UpdateOthersProfile");
        if (userProfileOthers.activeSelf)
        {
            if(CheckMissingInputsOthers())
            {
                Debug.Log("Missing inputs others");
                errorOthers.text = "Error. Display name and/or School cannot be empty.";
            }
            else
            {
                Debug.Log("UpdateOthersProfile");
                errorOthers.text = "";
                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>
                    {
                        {"DisplayName", displayNameOthers.text},
                        {"School", schoolOthers.text},
                        {"Description", descriptionOthers.text}
                    }
                };
                PlayFabClientAPI.UpdateUserData(request, UpdateOthersProfileSuccess, UpdateOthersProfileFail);
            }
        }
    }

    void UpdateOthersProfileSuccess(UpdateUserDataResult result)
    {
        Debug.Log("SaveOthersInfoSuccess");
        errorOthers.text = "Saved successfully";

    }

    void UpdateOthersProfileFail(PlayFabError error)
    {
        Debug.Log(error);
        errorOthers.text = "Error. Try again";
    }

    /*
    * Purpose: Checks whether the user (others) has inputs for all except for description
    * Outcomes: returns true if there is missing inputs
    */
    bool CheckMissingInputsOthers()
    {
        if (displayNameOthers.text == "" || schoolOthers.text == "")
        {
            return true;
        }
        return false;
    }


}
