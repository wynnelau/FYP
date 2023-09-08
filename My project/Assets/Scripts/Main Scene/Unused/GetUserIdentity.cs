using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PackageManager;

/*
 *Location: Main Scene, attached to "StudentControls"
 *Purpose: Get the user's identity once Main Scene is loaded, and store it in a variable
 */
public class GetUserIdentity : MonoBehaviour
{
    private string userIdentity;
    List<string> getUserIdentity = new List<string> { "Identity" };

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GetUserIdentity userIdentity == null");
        GetUserIdentityData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Purpose: To get the user's identity, used by other scripts
     * Outcomes: returns the value of userIdentity
     */
    public string GetIdentity()
    {
        Debug.Log("returning user identity");
        return userIdentity;
    }

    /*
     * Purpose: To retrieve the user's identity from db
     * Outcomes: retrieves the user's identity
     */
    public void GetUserIdentityData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = getUserIdentity
        }, GetUserIdentitySuccess, GetUserIdentityFail);
    }

    void GetUserIdentitySuccess(GetUserDataResult result)
    {
        Debug.Log("GetUserIdentitySuccess");
        userIdentity = result.Data["Identity"].Value;
    }

    void GetUserIdentityFail(PlayFabError error)
    {
        Debug.Log("GetUserIdentityFail " + error);
    }
}
