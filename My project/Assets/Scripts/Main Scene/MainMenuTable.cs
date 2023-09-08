using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;


/*
 *Location: Main Scene, attached to "table"
 *Purpose: Get the user's identity and open the corresponding menu
 */
public class MainMenuTable : MonoBehaviour
{
    public GameObject mainMenuStudent, mainMenuOthers;
    public PlayerControls player;
    private string userIdentity;
    List<string> getUserIdentity = new List<string> { "Identity" };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Purpose: Display main menu UI
     * Outcomes: 
     */
    private void OnTriggerEnter(Collider other)
    {
        GetUserIdentityData();
        player.enabled = false;
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
        userIdentity = result.Data["Identity"].Value;
        Debug.Log("GetUserIdentitySuccess " + userIdentity);
        if (userIdentity == "Student")
        {
            Debug.Log("GetUserIdentity Student ");
            mainMenuStudent.SetActive(true);
        }
        else if (userIdentity == "Staff" || userIdentity == "Professor/TA")
        {
            mainMenuOthers.SetActive(true);
        }
    }

    void GetUserIdentityFail(PlayFabError error)
    {
        Debug.Log("GetUserIdentityFail " + error);
    }
}
