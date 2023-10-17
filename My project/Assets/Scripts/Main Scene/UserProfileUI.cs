using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*
 * Location: MainSceneControls
 * Purpose: Read user profile data from database and display in UserProfileUI
 */

public class UserProfileUI : MonoBehaviour
{
    public GameObject userProfileStudent, userProfileOthers;
    public Text displayNameStudent, schoolStudent, courseStudent, yearStudent, descriptionStudent;
    public Text displayNameOthers, schoolOthers, descriptionOthers;
    public Text errorStudent, errorOthers;
    public PlayerControls player;

    /*
     * Purpose:  To close the respective userProfile UI and enable "Player"
     * Input: Click on the "closeUserProfileButton" in the userProfile UI
     * Output: Set the respective userProfile UI as inactive
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
    * Purpose: Attempt to update the student's user profile in the "userProfileUIStudent" UI
    * Input: Click on the "updateProfileButton" button in the "userProfileUIStudent" UI
    * Output: if there are no missing inputs, attempt to update student's user profile
    *         else return an errorStudent message
    */
    public void UpdateStudentProfile()
    {
        Debug.Log("UserProfileUI UpdateStudentProfile");
        if (userProfileStudent.activeSelf)
        {
            if (CheckMissingInputsStudent()) 
            {
                Debug.Log("UserProfileUI UpdateStudentProfile MissingInputs");
                errorStudent.text = "Error. Display name, School, Course and/or Year of study cannot be empty.";
            }
            else
            {
                Debug.Log("UserProfileUI UpdateStudentProfile AttemptToUpdate");
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

   /*
    * Purpose: Successful attempt to update the student's user profile 
    * Input: Called by UpdateStudentProfile() when attempt to update is successful
    * Output: returns an errorStudent message
    */
    void UpdateStudentProfileSuccess(UpdateUserDataResult result)
    {
        Debug.Log("UserProfileUI UpdateStudentProfileSuccess");
        errorStudent.text = "Saved successfully";
        
    }

    /*
     * Purpose: Failed attempt to update the student's user profile 
     * Input: Called by UpdateStudentProfile() when attempt to update failed
     * Output: returns an errorStudent message
     */
    void UpdateStudentProfileFail(PlayFabError error)
    {
        Debug.Log("UserProfileUI UpdateStudentProfileFail" + error);
        errorStudent.text = "Error. Try again";
    }

    /*
     * Purpose: Checks whether the user (student) has inputs for all except for description
     * Input: Called by UpdateStudentProfile() 
     * Output: returns true if there is missing inputs
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
    * Purpose: Attempt to update other user profile in the "userProfileUIOthers" UI
    * Input: Click on the "updateProfileButton" button in the "userProfileUIOthers" UI
    * Output: if there are no missing inputs, attempt to update other user profile
    *         else return an errorOthers message
    */
    public void UpdateOthersProfile()
    {
        Debug.Log("UserProfileUI UpdateOthersProfile");
        if (userProfileOthers.activeSelf)
        {
            if(CheckMissingInputsOthers())
            {
                Debug.Log("UserProfileUI UpdateOthersProfile MissingInputs");
                errorOthers.text = "Error. Display name and/or School cannot be empty.";
            }
            else
            {
                Debug.Log("UserProfileUI UpdateOthersProfile AttemptToUpdate");
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

   /*
    * Purpose: Successful attempt to update the other user profile 
    * Input: Called by UpdateOthersProfile() when attempt to update is successful
    * Output: returns an errorOthers message
    */
    void UpdateOthersProfileSuccess(UpdateUserDataResult result)
    {
        Debug.Log("UserProfileUI UpdateOthersProfileSuccess");
        errorOthers.text = "Saved successfully";

    }

   /*
    * Purpose: Failed attempt to update the other user profile 
    * Input: Called by UpdateOthersProfile() when attempt to update failed
    * Output: returns an errorOthers message
    */
    void UpdateOthersProfileFail(PlayFabError error)
    {
        Debug.Log("UserProfileUI UpdateOthersProfileFail" + error);
        errorOthers.text = "Error. Try again";
    }

    /*
     * Purpose: Checks whether the user (others) has inputs for all except for description
     * Input: Called by UpdateOthersProfile() 
     * Output: returns true if there is missing inputs
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
