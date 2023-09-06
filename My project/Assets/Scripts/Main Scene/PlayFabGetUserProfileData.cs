using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;

public class PlayFabGetUserProfileData : MonoBehaviour
{
    public GameObject userProfileStudent;
    List<string> getUserIdentity = new List<string> { "Identity" };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GetUserData();
    }

    public void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = getUserIdentity
        }, GetUserDataSuccess, GetUserDataFail);
    }

    void GetUserDataSuccess(GetUserDataResult result)
    {
        if (result.Data["Identity"].Value == "Student")
        {
            Debug.Log("GetUserDataSuccess student");
            userProfileStudent.SetActive(true);
        }
    }

    void GetUserDataFail(PlayFabError error)
    {
        Debug.Log(error);
    }
}
