using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GetUserProfileDataControls : MonoBehaviour
{
    public GameObject getUserProfileDataPageStudent, getUserProfileDataPageOthers;
    public Text displayNameStudent, schoolStudent, courseStudent, yearStudent, descriptionStudent;
    public Text displayNameOthers, schoolOthers, descriptionOthers;
    public Text errorStudent, errorOthers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if press "enter", send data
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (getUserProfileDataPageStudent.activeSelf)
            {
                SendStudentInfo();
            }
            else if (getUserProfileDataPageOthers.activeSelf)
            {
                SendOthersInfo();
            }
        }

    }

    bool CheckMissingInputsStudent()
    {
        if (displayNameStudent.text == "" || schoolStudent.text == "" || courseStudent.text == "" || yearStudent.text == "")
        {
            return true;
        }
        return false;
    }

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
            SceneManager.LoadScene("Main Scene");
        }
    }

    void SaveStudentInfoSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Saved student info success");
    }

    void SaveStudentInfoFail(PlayFabError error)
    {
        Debug.Log(error);
    }

    bool CheckMissingInputsOthers()
    {
        if (displayNameOthers.text == "" || schoolOthers.text == "")
        {
            return true;
        }
        return false;
    }

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
            SceneManager.LoadScene("Main Scene");
        }
        
    }

    void SaveOthersInfoSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Saved others info success");
    }

    void SaveOthersInfoFail(PlayFabError error)
    {
        Debug.Log(error);
    }

    
}
