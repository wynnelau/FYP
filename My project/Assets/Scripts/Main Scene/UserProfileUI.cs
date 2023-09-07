using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UserProfileUI : MonoBehaviour
{
    public GameObject userProfileStudent;
    public Text displayNameStudent, schoolStudent, courseStudent, yearStudent, descriptionStudent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // for the close button
    public void closeUserProfile()
    {
        userProfileStudent.SetActive(false);
    }

    public void GetUserProfileData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, GetUserProfileDataSuccess, GetUserProfileDataFail);
    }

    void GetUserProfileDataSuccess(GetUserDataResult result)
    {
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
