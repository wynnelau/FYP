using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

/*
 * Location: UIControls
 * Purpose: Manage the register and login using PlayFab API calls
 * Tutorial used: https://www.youtube.com/watch?v=XPTPRaF2pd4
 */
public class PlayFabLoginRegister : MonoBehaviour
{
    public GameObject registerPageUI, loginPageUI, setUserProfilePageStudentUI, setUserProfilePageOthersUI;
    public TMP_Text registerEmail, registerPassword, registerError, loginEmail, loginPassword, loginError;
    public TMP_Dropdown identityDropdown;

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
    void Register()
    {
        // if identity not given, do not register
        if (identityDropdown.value == 0)
        {
            Debug.Log("Missing identity register");
            registerError.text = "Unable to register. Missing input(s).";
        }
        else
        {
            registerError.text = " ";
            var registerRequest = new RegisterPlayFabUserRequest { Email = registerEmail.text, Password = registerPassword.text, RequireBothUsernameAndEmail = false };
            PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterFail);
        }
    }

    /*
     * Purpose: Successful attempt to register the user, retrieves user's identity and saves it to the PlayFab database
     *          Then, open the "SetUserProfilePageStudent" UI or "SetUserProfilePageOthers" UI accordingly
     * Input: Called by Register() when attempt to register the user is successful
     * Output: Calls Identity(), SaveIdentity() and then KnowMore()      
     */
    void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        string userIdentity = Identity();
        // Send user's identity to db
        SaveIdentity(userIdentity);
        // Go to the user profile set up page according to their identity;
        KnowMore(userIdentity);
    }

    /*
     * Purpose: Failed attempt to register the user
     * Input: Called by Register() when attempt to register the user failed
     * Output: Return a registerError message    
     */
    void RegisterFail(PlayFabError error)
    {
        registerError.text = error.GenerateErrorReport();
    }

    /*
     * Purpose: Retrieve the user's identity using "IdentityDropdown" dropdown
     * Input: Called by RegisterSuccess()
     * Output: Returns a string according to "IdentityDropdown"     
     */
    string Identity()
    {
        if (identityDropdown.value == 1) return "Student";
        else if (identityDropdown.value == 2) return "Professor/TA";
        else if (identityDropdown.value == 3) return "Staff";
        else return "";
    }

    /*
     * Purpose: Attempt to send the identity of the user to the PlayFab database
     * Input: Called by RegisterSuccess() and identity string is passed in
     * Output: Attempt to save the user's identity using identity string     
     */
    void SaveIdentity(string identity)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Identity", identity},
                {"Email",  registerEmail.text}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, SaveIdentitySuccess, SaveIdentityFail);
    }

    /*
     * Purpose: Successful attempt to send the identity of the user to the PlayFab database
     * Input: Called by SaveIdentity when attempt to send identity is successful
     * Output: Debug.Log("PlayFabLoginRegister SaveIdentitySuccess");
     */
    void SaveIdentitySuccess(UpdateUserDataResult result)
    {
        Debug.Log("PlayFabLoginRegister SaveIdentitySuccess");
    }

    /*
     * Purpose: Failed attempt to send the identity of the user to the PlayFab database
     * Input: Called by SaveIdentity when attempt to send identity failed
     * Output: Debug.Log("PlayFabLoginRegister SaveIdentityFail");
     */
    void SaveIdentityFail(PlayFabError error)
    {
        Debug.Log("PlayFabLoginRegister SaveIdentityFail");
    }

    /*
     * Purpose: Open the "SetUserProfilePageStudent" UI or "SetUserProfilePageOthers" UI according to the identity of the user
     * Input: Called by RegisterSuccess() and identity string is passed in
     * Output: If identity is Student, open "SetUserProfilePageStudent" UI  
     *         else if identity is Professor/TA or Staff, open "SetUserProfilePageOthers" UI
     */
    void KnowMore(string identity)
    {
        loginPageUI.SetActive(false);
        registerPageUI.SetActive(false);
        // Set the different profile pages as active
        if (identity == "Student")
        {
            setUserProfilePageStudentUI.SetActive(true);
        }
        else if (identity == "Professor/TA" || identity == "Staff")
        {
            setUserProfilePageOthersUI.SetActive(true);
        }

    }

    /*
     * Purpose: Attempt to login when "enter" key is pressed or when "LoginButton" button is clicked in "LoginPage" UI
     * Input: Called by Update() when "enter" is pressed in the "LoginPage" UI
     *        Click the "LoginButton" button in the "LoginPage" UI
     * Output: Attempt to login using loginEmail and loginPassword
     */
    void Login()
    {
        loginError.text = " ";
        var request = new LoginWithEmailAddressRequest { Email = loginEmail.text, Password = loginPassword.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginFail);
    }

    /*
     * Purpose: Successful attempt to login, opens "MainScene" scene
     * Input: Called by Login() when attempt to login is successful
     * Output: Calls MainScene()     
     */
    void LoginSuccess(LoginResult result)
    {
        MainScene();
    }

    /*
     * Purpose: Failed attempt to login
     * Input: Called by Login() when attempt to login failed
     * Output: Return a loginError message    
     */
    void LoginFail(PlayFabError error)
    {
        loginError.text = error.GenerateErrorReport();
    }

    /*
     * Purpose: Switch from "LoginUI" scene to "MainScene" scene
     * Input: Called by LoginSuccess() 
     * Output: Loads the "MainScene" scene  
     */
    void MainScene()
    {
        loginPageUI.SetActive(false);
        registerPageUI.SetActive(false);
        // Load the Main Scene
        SceneManager.LoadScene("Main Scene");
    }





}
