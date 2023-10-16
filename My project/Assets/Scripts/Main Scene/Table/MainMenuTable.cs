using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;


/*
 *Location: "Table"  in "MainScene" scene
 *Purpose: Get the user's identity and open the corresponding mainMenu UI when the user comes into contact with "Table"
 */
public class MainMenuTable : MonoBehaviour
{
    public GameObject mainMenuStudent, mainMenuStaff, mainMenuProf;
    public PlayerControls player;
    private string userIdentity;
    List<string> getUserIdentity = new List<string> { "Identity" };

    /*
     * Purpose: Call GetUserIdentityData() to get the user's identity to open the corresponding mainMenu UI when user comes into contact with "Table"
     * Input: User comes into contact with "Table"
     * Output: Call GetUserIdentityData() 
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("MainMenuTable onTriggerEnter: activateMainMenuUI");
        GetUserIdentityData();
        player.enabled = false;
    }

    /*
     * Purpose: Attempt to retrieve the user's identity from the PlayFab database when called by OnTriggerEnter()
     * Input: Called by OnTriggerEnter() when user comes into contact with "Table"
     * Output: Attempt to retrieve the user's identity from the PlayFab database
     */
    public void GetUserIdentityData()
    {
        Debug.Log("MainMenuTable GetUserIdentityData");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = getUserIdentity
        }, GetUserIdentitySuccess, GetUserIdentityFail);
    }

    /*
     * Purpose: Successful attempt to retrieve the user's identity and changes to the corresponding mainMenu UI
     * Input: Called by GetUserIdentityData() when attempt to retrieve the user's identity is successful
     * Output: If user is Student, opens "mainMenuStudent" UI
     *         else if user is Staff, opens "mainMenuStaff" UI
     *         else if user is Prof/TA, opens "mainMenuProf" UI
     */
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

    /*
     * Purpose: Failed attempt to retrieve the user's identity
     * Input: Called by OnTriggerEnter() when attempt to retrieve the user's identity failed
     * Output: Debug.Log("MainMenuTable GetUserIdentityFail " + error);
     */
    void GetUserIdentityFail(PlayFabError error)
    {
        Debug.Log("MainMenuTable GetUserIdentityFail " + error);
    }
}
