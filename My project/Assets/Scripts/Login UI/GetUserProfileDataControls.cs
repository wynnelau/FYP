using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;

public class GetUserProfileDataControls : MonoBehaviour
{
    public GameObject getUserProfileDataPageStudent, getUserProfileDataPageOthers;
    public TMP_Text displayNameStudent, schoolStudent, courseStudent, yearStudent, descriptionStudent;
    public Text errorStudent;
    // Start is called before the first frame update
    void Start()
    {
        if (getUserProfileDataPageStudent.activeSelf)
        {
            displayNameStudent.text = string.Empty;
            schoolStudent.text = string.Empty;
            courseStudent.text = string.Empty;
            yearStudent.text = string.Empty;
            descriptionStudent.text = string.Empty;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (getUserProfileDataPageStudent.activeSelf)
            {
                SendStudentInfo();
            }
        }
        
    }

    public void SendStudentInfo()
    {
        if (CheckMissingInputs()) 
        {
            errorStudent.text = "Unable to continue. Missing input(s)";
        }
        else
        {
            Debug.Log("Entered else");
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
        Debug.Log("Saved info " + result);
    }

    void SaveStudentInfoFail(PlayFabError error)
    {
        Debug.Log(error);
    }

    public bool CheckMissingInputs()
    {
        if (displayNameStudent.text == "" || schoolStudent.text == "" || courseStudent.text == "" || yearStudent.text == "")
        {
            return true;
        }
        else return false;
    }
}
