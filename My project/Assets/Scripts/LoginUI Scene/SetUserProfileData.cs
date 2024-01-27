using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*
 * Location: Login UI/ UIControls
 * Purpose: Get profile data from user once they register (Student and Others) and send to PlayFab database
 */
public class SetUserProfileData : MonoBehaviour
{
    public GameObject setUserProfilePageStudentUI, setUserProfilePageOthersUI;
    public Text displayNameStudent, schoolStudent, courseStudent, yearStudent, descriptionStudent;
    public Text displayNameOthers, schoolOthers, descriptionOthers;
    public Text errorStudent, errorOthers;

    /*
     * Purpose: Call either SendStudentInfo() or SendOthersInfo() when "enter" key is pressed in "SetUserProfilePageStudent" UI or "SetUserProfilePageOthers" UI accordingly
     *          Update is called once per frame
     * Input: Press the "enter" key
     * Output: If in "SetUserProfilePageStudent" UI, call SendStudentInfo() 
     *          If in "SetUserProfilePageOthers" UI, call SendOthersInfo() 
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (setUserProfilePageStudentUI.activeSelf)
            {
                SendStudentInfo();
            }
            else if (setUserProfilePageOthersUI.activeSelf)
            {
                SendOthersInfo();
            }
        }

    }

    /*
     * Purpose: Attempt to send student info to the PlayFab database when "enter" key is pressed or when "ContinueButtonStudent" button in "SetUserProfilePageStudent" UI
     * Input: Called by Update() when "enter" is pressed in "SetUserProfilePageStudent" UI
     *        Click the "ContinueButtonStudent" button in "SetUserProfilePageStudent" UI
     * Output: If there are missing inputs, return a errorStudent message
     *         else attempt to send student info using displayNameStudent, schoolStudent, courseStudent, yearStudent and descriptionStudent  
     */
    public void SendStudentInfo()
    {
        Debug.Log("SetUserProfileData SendStudentsInfo");
        if (CheckMissingInputsStudent())
        {
            Debug.Log("SetUserProfileData SendStudentsInfo MissingInputs");
            errorStudent.text = "Unable to continue. Missing input(s).";
        }
        else
        {
            Debug.Log("SetUserProfileData SendStudentsInfo AttemptToUpdateData");
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
            PlayFabClientAPI.UpdateUserData(request, SaveStudentInfoSuccess, SaveStudentInfoFail);
        }
    }

    /*
     * Purpose: Successful attempt to send student info and changes to the "MainScene" scene
     * Input: Called by SendStudentInfo() when attempt to send student info is successful
     * Output: Loads the "MainScene" scene      
     */
    void SaveStudentInfoSuccess(UpdateUserDataResult result)
    {
        Debug.Log("SetUserProfileData SaveStudentInfoSuccess");
        setUserProfilePageStudentUI.SetActive(false);
        SceneManager.LoadScene("Main Scene");
    }

    /*
     * Purpose: Failed attempt to send student info
     * Input: Called by SendStudentInfo() when attempt to send student info failed
     * Output: Return a errorStudent message
     */
    void SaveStudentInfoFail(PlayFabError error)
    {
        Debug.Log("SetUserProfileData SaveStudentInfoFail");
        errorStudent.text = error.GenerateErrorReport();
    }

    /*
     * Purpose: Checks whether the user (student) has inputs for all except for description
     * Input: Called by SendStudentInfo()
     * Output: If there are missing inputs, return true
     *         else return false
     */
    bool CheckMissingInputsStudent()
    {
        Debug.Log("SetUserProfileData CheckMissingInputsStudent");
        if (displayNameStudent.text == "" || schoolStudent.text == "" || courseStudent.text == "" || yearStudent.text == "")
        {
            return true;
        }
        return false;
    }

    /*
     * Purpose: Attempt to send others info to the PlayFab database when "enter" key is pressed or when "ContinueButtonOthers" button in "SetUserProfilePageOthers" UI
     * Input: Called by Update() when "enter" is pressed in "SetUserProfilePageOthers" UI
     *        Click the "ContinueButtonOthers" button in "SetUserProfilePageOthers" UI
     * Output: If there are missing inputs, return a errorOthers message
     *         else attempt to send others info using displayNameOthers, schoolOthers and descriptionOthers  
     */
    public void SendOthersInfo()
    {

        Debug.Log("SetUserProfileData SendOthersInfo");
        if (CheckMissingInputsOthers())
        {
            Debug.Log("SetUserProfileData SendOthersInfo MissingInputs");
            errorOthers.text = "Unable to continue. Missing input(s).";
        }
        else
        {
            Debug.Log("SetUserProfileData SendOthersInfo AttemptToUpdateData");
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
            PlayFabClientAPI.UpdateUserData(request, SaveOthersInfoSuccess, SaveOthersInfoFail);
            
        }
        
    }

    /*
     * Purpose: Successful attempt to send others info and changes to the "MainScene" scene
     * Input: Called by SendOthersInfo() when attempt to send others info is successful
     * Output: Loads the "MainScene" scene      
     */
    void SaveOthersInfoSuccess(UpdateUserDataResult result)
    {
        Debug.Log("SetUserProfileData SaveOthersInfoSuccess");
        setUserProfilePageOthersUI.SetActive(false);
        SceneManager.LoadScene("Main Scene");
    }

    /*
     * Purpose: Failed attempt to send others info
     * Input: Called by SendOthersInfo() when attempt to send others info failed
     * Output: Return a errorOthers message
     */
    void SaveOthersInfoFail(PlayFabError error)
    {
        Debug.Log("SetUserProfileData SaveOthersInfoFail");
        errorOthers.text = error.GenerateErrorReport();
    }

    /*
     * Purpose: Checks whether the user (others) has inputs for all except for description
     * Input: Called by SendOthersInfo()
     * Output: If there are missing inputs, return true
     *         else return false
     */
    bool CheckMissingInputsOthers()
    {
        Debug.Log("SetUserProfileData CheckMissingInputsOthers");
        if (displayNameOthers.text == "" || schoolOthers.text == "")
        {
            return true;
        }
        return false;
    }



}
