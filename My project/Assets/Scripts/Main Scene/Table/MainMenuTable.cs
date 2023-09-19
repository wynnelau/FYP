using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;


/*
 *Location: Main Scene, attached to "Table"
 *Purpose: Get the user's identity and open the corresponding menu
 */
public class MainMenuTable : MonoBehaviour
{
    public GameObject mainMenuStudent, mainMenuStaff, mainMenuProf;
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
     * Purpose: Trigger the mainMenuUI when user comes into contact with "Table"
     * Outcomes: mainMenuUI is activated
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("MainMenuTable onTriggerEnter activateMainMenuUI");
        GetUserIdentityData();
        player.enabled = false;
    }

    /*
     * Purpose: To retrieve the user's identity from db
     * Outcomes: retrieves the user's identity
     */
    public void GetUserIdentityData()
    {
        Debug.Log("MainMenuTable GetUserIdentityData");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = getUserIdentity
        }, GetUserIdentitySuccess, GetUserIdentityFail);
    }

    void GetUserIdentitySuccess(GetUserDataResult result)
    {
        userIdentity = result.Data["Identity"].Value;
        Debug.Log("MainMenuTable GetUserIdentitySuccess");
        if (userIdentity == "Student")
        {
            Debug.Log("MainMenuTable GetUserIdentitySuccess Student");
            mainMenuStudent.SetActive(true);
        }
        else if (userIdentity == "Staff")
        {
            Debug.Log("MainMenuTable GetUserIdentitySuccess Staff");
            mainMenuStaff.SetActive(true);
        }
        else if(userIdentity == "Professor/TA")
        {
            Debug.Log("MainMenuTable GetUserIdentitySuccess Prof");
            mainMenuProf.SetActive(true);
        }
    }

    void GetUserIdentityFail(PlayFabError error)
    {
        Debug.Log("MainMenuTable GetUserIdentityFail " + error);
    }
}
