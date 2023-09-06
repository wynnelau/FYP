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
        GetUserIdentityData();
    }

    public void GetUserIdentityData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = getUserIdentity
        }, GetUserIdentityDataSuccess, GetUserIdentityDataFail);
    }

    void GetUserIdentityDataSuccess(GetUserDataResult result)
    {
        if (result.Data["Identity"].Value == "Student")
        {
            Debug.Log("GetUserIdentityDataSuccess student");
            userProfileStudent.SetActive(true);
        }
    }

    void GetUserIdentityDataFail(PlayFabError error)
    {
        Debug.Log(error);
    }
}
