using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GetUserProfileDataControls : MonoBehaviour
{
    public GameObject getUserProfileDataPageStudent, getUserProfileDataPageOthers;
    public TMP_Text displayNameStudent, schoolStudent, courseStudent, yearStudent, descriptionStudent;
    public TMP_Text displayNameOthers, schoolOthers, descriptionOthers;
    /*public Text errorStudent, errorOthers;*/
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

    public void SendStudentInfo()
    {

        Debug.Log("Entered SendStudentInfo");
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
        SceneManager.LoadScene("Main Scene Student");
        /*SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main Scene Student"));*/
    }

    void SaveStudentInfoSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Saved student info success");
    }

    void SaveStudentInfoFail(PlayFabError error)
    {
        Debug.Log(error);
    }

    public void SendOthersInfo()
    {

        Debug.Log("Entered SendOthersInfo");
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

    void SaveOthersInfoSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Saved others info success");
    }

    void SaveOthersInfoFail(PlayFabError error)
    {
        Debug.Log(error);
    }
}
