using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*
 *Location: Login UI
 *Purpose: Get user data from user once they register for an account (Sudent and others) and write to database
 */
public class SetUserProfileData : MonoBehaviour
{
    public GameObject setUserProfileDataPageStudent, setUserProfileDataPageOthers;
    public Text displayNameStudent, schoolStudent, courseStudent, yearStudent, descriptionStudent;
    public Text displayNameOthers, schoolOthers, descriptionOthers;
    public Text errorStudent, errorOthers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*
     * Purpose: Whenever the "enter" key is pressed, it tries to send the data to the database
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // for student
            if (setUserProfileDataPageStudent.activeSelf)
            {
                SendStudentInfo();
            }
            // for others
            else if (setUserProfileDataPageOthers.activeSelf)
            {
                SendOthersInfo();
            }
        }

    }

    /*
     * Purpose: Tries to send user data to database, tied to the "continueButtonStudent"
     * Outcomes: if there are missing inputs or PlayFabError, unable to send user data 
     *           else, sends all data (including empty descriptions) to the database and load next scene
     */
    public void SendStudentInfo()
    {
        Debug.Log("Entered SendStudentsInfo");
        if (CheckMissingInputsStudent())
        {
            Debug.Log("Missing inputs student");
            errorStudent.text = "Unable to continue. Missing input(s).";
        }
        else
        {
            Debug.Log("Update student data");
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

    void SaveStudentInfoSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Saved student info success");
        setUserProfileDataPageStudent.SetActive(false);
        SceneManager.LoadScene("Main Scene");
    }

    void SaveStudentInfoFail(PlayFabError error)
    {
        Debug.Log(error);
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
    * Purpose: Tries to send user data to database, tied to the "continueButtonOthers"
    * Outcomes: if there are missing inputs or PlayFabError, unable to send user data 
    *           else, sends all data (including empty descriptions) to the database and load next scene
    */
    public void SendOthersInfo()
    {

        Debug.Log("Entered SendOthersInfo");
        if (CheckMissingInputsOthers())
        {
            Debug.Log("Missing inputs others");
            errorOthers.text = "Unable to continue. Missing input(s).";
        }
        else
        {
            Debug.Log("Update others data");
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

    void SaveOthersInfoSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Saved others info success");
        setUserProfileDataPageOthers.SetActive(false);
        SceneManager.LoadScene("Main Scene");
    }

    void SaveOthersInfoFail(PlayFabError error)
    {
        Debug.Log(error);
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
