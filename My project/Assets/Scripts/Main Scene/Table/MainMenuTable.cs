using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;

/*
 * Location: Main Scene/ Table
 * Purpose: Get the user's identity and open the corresponding mainMenu UI when the user comes into contact with "Table"
 */

public class MainMenuTable : MonoBehaviour
{
    private static MainMenuTable instance;
    public GameObject mainMenuStudent, mainMenuStaff, mainMenuProf;
    public PlayerControls player;
    private string userIdentity;

    /*
     * Purpose: Make sure the gameObject is persistent across scenes
     * Input: NA
     * Output: If instance is null, let instance = this and DontDestroyOnLoad
     *         else destroy the gameObject
     */
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MainMenuTable Awake InstanceIsNull");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("MainMenuTable Awake DestroyGameObject");
        }
    }

    /*
     * Purpose: Call GetUserIdentityData() to get the user's identity to open the corresponding mainMenu UI when user comes into contact with "Table"
     * Input: User comes into contact with "Table"
     * Output: Call GetUserIdentityData() 
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("MainMenuTable onTriggerEnter");
        GetUserIdentityData();
    }

    /*
     * Purpose: Attempt to retrieve the user's identity from the PlayFab database when called by OnTriggerEnter()
     * Input: Called by OnTriggerEnter() when user comes into contact with "Table"
     * Output: Attempt to retrieve the user's identity from the PlayFab database
     */
    void GetUserIdentityData()
    {
        Debug.Log("MainMenuTable GetUserIdentityData");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, GetUserIdentitySuccess, GetUserIdentityFail); ;
    }

    /*
     * Purpose: Successful attempt to retrieve the user's identity and changes to the corresponding mainMenu UI
     * Input: Called by GetUserIdentityData() when attempt to retrieve the user's identity is successful
     * Output: If user is Student, opens "mainMenuStudent" UI
     *         else if user is Staff, opens "mainMenuStaff" UI
     *         else if user is Prof/TA, opens "mainMenuProf" UI
     *         then disable player movements
     */
    void GetUserIdentitySuccess(GetUserDataResult result)
    {
        Debug.Log("MainMenuTable GetUserIdentitySuccess");
        userIdentity = result.Data["Identity"].Value;
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
        player.enabled = false;
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

    /*
     * Purpose: Getter function of the private userIdentity
     * Input: Called by MeetingScheduleUI when closing MeetingSchedule
     * Output: Returns the user's identity
     */
    public string GetUserIdentity
    {
        get
        {
            return userIdentity;
        }
    }
}
