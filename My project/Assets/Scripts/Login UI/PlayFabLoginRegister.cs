using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

/*
 * Location: Login UI/ UIControls
 * Purpose: Manage the register and login using PlayFab API calls
 * Tutorial used: https://www.youtube.com/watch?v=XPTPRaF2pd4
 */
public class PlayFabLoginRegister : MonoBehaviour
{
    public GameObject registerPageUI, loginPageUI, setUserProfilePageStudentUI, setUserProfilePageOthersUI;
    public TMP_Text registerEmail, registerPassword, registerError, loginEmail, loginPassword, loginError;
    public TMP_Dropdown identityDropdown;
    private string userIdentity;

    /*
     * Purpose: Call either Register() or Login() when "enter" key is pressed in "RegisterPage" UI or "LoginPage" UI accordingly
     *          Update is called once per frame
     * Input: Press the "enter" key
     * Output: If in "RegisterPage" UI, call Register() 
     *          If in "LoginPage" UI, call Login() 
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (registerPageUI.activeSelf)
            {
                Register();
            }
            if (loginPageUI.activeSelf)
            {
                Login();
            }
        }
    }

    /*
     * Purpose: Attempt to register the user when "enter" key is pressed or when "RegisterButton" button is clicked in "RegisterPage" UI
     * Input: Called by Update() when "enter" is pressed in the "RegisterPage" UI
     *        Click the "RegisterButton" button in the "RegisterPage" UI
     * Output: If the identity of the user is not specified in "IdentityDropdown" dropdown, return a registerError message
     *         else attempt to register the user using registerEmail and registerPassword
     */
    public void Register()
    {
        if (identityDropdown.value == 0)
        {
            Debug.Log("PlayFabLoginRegister Register IdentityNotGiven");
            registerError.text = "Missing input(s). Unable to register.";
        }
        else
        {
            Debug.Log("PlayFabLoginRegister Register AttemptToRegisterUser");
            registerError.text = " ";
            var registerRequest = new RegisterPlayFabUserRequest { Email = registerEmail.text, Password = registerPassword.text, RequireBothUsernameAndEmail = false };
            PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterFail);
        }
    }

    /*
     * Purpose: Successful attempt to register the user, retrieves user's identity and saves it to the PlayFab database
     * Input: Called by Register() when attempt to register the user is successful
     * Output: Calls GetIdentity() and SaveIdentity()       
     */
    void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("PlayFabLoginRegister RegisterSuccess");
        GetIdentity();
        SaveIdentity();
    }

    /*
     * Purpose: Failed attempt to register the user
     * Input: Called by Register() when attempt to register the user failed
     * Output: Return a registerError message    
     */
    void RegisterFail(PlayFabError error)
    {
        Debug.Log("PlayFabLoginRegister RegisterFail");
        registerError.text = error.GenerateErrorReport();
    }

    /*
     * Purpose: Retrieve the user's identity using "IdentityDropdown" dropdown
     * Input: Called by RegisterSuccess()
     * Output: Set the value of userIdentity according to "IdentityDropdown"     
     */
    void GetIdentity()
    {
        Debug.Log("PlayFabLoginRegister GetIdentity");
        if (identityDropdown.value == 1) userIdentity = "Student";
        else if (identityDropdown.value == 2) userIdentity = "Professor/TA";
        else if (identityDropdown.value == 3) userIdentity = "Staff";
    }

    /*
     * Purpose: Attempt to send the identity and email of the user to the PlayFab database
     * Input: Called by RegisterSuccess()
     * Output: Attempt to save the user's data using userIdentity and registerEmail
     */
    void SaveIdentity()
    {
        Debug.Log("PlayFabLoginRegister SaveIdentity " + userIdentity);
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Identity", userIdentity},
                {"Email",  registerEmail.text}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, SaveIdentitySuccess, SaveIdentityFail);
    }

    /*
     * Purpose: Successful attempt to send the identity of the user to the PlayFab database, and open the correct setUserProfilePage UI
     * Input: Called by SaveIdentity() when attempt to send identity is successful
     * Output: Call KnowMore() to open the "SetUserProfilePageStudent" UI or "SetUserProfilePageOthers" UI accordingly
     */
    void SaveIdentitySuccess(UpdateUserDataResult result)
    {
        Debug.Log("PlayFabLoginRegister SaveIdentitySuccess");
        KnowMore();
    }

    /*
     * Purpose: Failed attempt to send the identity of the user to the PlayFab database
     * Input: Called by SaveIdentity() when attempt to send identity failed
     * Output: Attempt to save the identity again by calling SaveIdentity()
     */
    void SaveIdentityFail(PlayFabError error)
    {
        Debug.Log("PlayFabLoginRegister SaveIdentityFail");
        SaveIdentity();
    }

    /*
     * Purpose: Open the "SetUserProfilePageStudent" UI or "SetUserProfilePageOthers" UI according to the identity of the user
     * Input: Called by SaveIdentitySuccess() 
     * Output: If identity is Student, open "SetUserProfilePageStudent" UI  
     *         else if identity is Professor/TA or Staff, open "SetUserProfilePageOthers" UI
     */
    void KnowMore()
    {
        Debug.Log("PlayFabLoginRegister KnowMore");
        loginPageUI.SetActive(false);
        registerPageUI.SetActive(false);
        if (userIdentity == "Student")
        {
            Debug.Log("PlayFabLoginRegister KnowMore Student");
            setUserProfilePageStudentUI.SetActive(true);
        }
        else if (userIdentity == "Professor/TA" || userIdentity == "Staff")
        {
            Debug.Log("PlayFabLoginRegister KnowMore Others");
            setUserProfilePageOthersUI.SetActive(true);
        }

    }

    /*
     * Purpose: Attempt to login when "enter" key is pressed or when "LoginButton" button is clicked in "LoginPage" UI
     * Input: Called by Update() when "enter" is pressed in the "LoginPage" UI
     *        Click the "LoginButton" button in the "LoginPage" UI
     * Output: Attempt to login using loginEmail and loginPassword
     */
    public void Login()
    {
        Debug.Log("PlayFabLoginRegister Login");
        loginError.text = " ";
        var request = new LoginWithEmailAddressRequest { Email = loginEmail.text, Password = loginPassword.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginFail);
    }

    /*
     * Purpose: Successful attempt to login, opens "MainScene" scene
     * Input: Called by Login() when attempt to login is successful
     * Output: Loads the "MainScene" scene  
     */
    void LoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFabLoginRegister LoginSuccess");
        loginPageUI.SetActive(false);
        registerPageUI.SetActive(false);
        SceneManager.LoadScene("Main Scene");
    }

    /*
     * Purpose: Failed attempt to login
     * Input: Called by Login() when attempt to login failed
     * Output: Return a loginError message    
     */
    void LoginFail(PlayFabError error)
    {
        Debug.Log("PlayFabLoginRegister LoginFail");
        loginError.text = error.GenerateErrorReport();
    }

    





}
