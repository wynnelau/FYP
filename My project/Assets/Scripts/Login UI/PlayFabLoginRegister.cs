using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

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
     * Purpose: Update is called once per frame. Whenever the "enter" key is pressed, it will try to call either Register() or Login() accordingly
     * Outcome: If in register page, pressing "enter" would result in calling the Register() function
     *          If in login page, pressing "enter" woudld result in calling the Login() function
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
     * Purpose: Tries to send register data to database, tied to the "registerButton"
     * Outcomes: if there are missing inputs or PlayFabError, unable to send register data 
     *           else, sends email, password and identity to the database
     */
    public void Register()
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

    public void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        string userIdentity = Identity();
        // Send user's identity to db
        SaveIdentity(userIdentity);
        // Go to the user profile set up page according to their identity;
        KnowMore(userIdentity);
    }

    public void RegisterFail(PlayFabError error)
    {
        registerError.text = error.GenerateErrorReport();
    }

    /*
     * Purpose: Gets the identity of the user through the dropdown 
     * Outcomes: returns string of user's identity
     */
    public string Identity()
    {
        if (identityDropdown.value == 1) return "Student";
        else if (identityDropdown.value == 2) return "Professor/TA";
        else if (identityDropdown.value == 3) return "Staff";
        else return "";
    }

    /*
     * Purpose: Tries to send identity and email of user to database, called after registration is successful
     * Outcomes: if there are missing inputs or PlayFabError, unable to send user data 
     *           else, sends identity data to the database
     */
    public void SaveIdentity(string identity)
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

    void SaveIdentitySuccess(UpdateUserDataResult result)
    {
        Debug.Log("SaveIdentitySuccess");
    }

    void SaveIdentityFail(PlayFabError error)
    {
        Debug.Log(error);
    }

    /*
     * Purpose: Opens the setUserProfileDataPage according to their identity, called after registration is successful
     * Outcomes: sets the different versions according to their identity;
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
     * Purpose: Tries to send login data to database, tied to the "loginButton"
     * Outcomes: if there is PlayFabError, rmail or login is incorrect 
     *           else, sends email and password to the database to verify and load "Main Scene"
     */
    public void Login()
    {
        loginError.text = " ";
        var request = new LoginWithEmailAddressRequest { Email = loginEmail.text, Password = loginPassword.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginFail);
    }

    public void LoginSuccess(LoginResult result)
    {
        MainScene();
    }

    public void LoginFail(PlayFabError error)
    {
        loginError.text = error.GenerateErrorReport();
    }

    void MainScene()
    {
        loginPageUI.SetActive(false);
        registerPageUI.SetActive(false);
        // Load the Main Scene
        SceneManager.LoadScene("Main Scene");
    }





}
