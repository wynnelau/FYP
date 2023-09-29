using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

/*
 * Location: Login UI, used by UIControls
 * Purpose: Manage the register and login using PlayFab API calls
 * Tutorial used: https://www.youtube.com/watch?v=XPTPRaF2pd4
 */
public class PlayFabLoginRegister : MonoBehaviour
{
    public GameObject registerPage, loginPage, setUserProfileDataPageStudent, setUserProfileDataPageOthers;
    public TMP_Text registerEmail, registerPassword, registerError, loginEmail, loginPassword, loginError;
    public TMP_Dropdown identityDropdown;
    // Update is called once per frame
    /*
     * Purpose: Whenever the "enter" key is pressed, it tries to send the data to the database
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (registerPage.activeSelf)
            {
                Register();
            }
            if (loginPage.activeSelf)
            {
                Login();
            }
        }
    }


    /*
     * Purpose: Tries to send register data to database, tied to the "registerButton"
     * Outcomes: if there are missing inputs or PlayFabError, unable to send register data 
     *           else, sends all data to the database
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
        string userIdentity = identity();
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
    public string identity()
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
        loginPage.SetActive(false);
        registerPage.SetActive(false);
        // Set the different profile pages as active
        if (identity == "Student")
        {
            setUserProfileDataPageStudent.SetActive(true);
        }
        else if (identity == "Professor/TA" || identity == "Staff")
        {
            setUserProfileDataPageOthers.SetActive(true);
        }

    }

    /*
     * Purpose: Tries to send login data to database, tied to the "loginButton"
     * Outcomes: if there is PlayFabError, unable to send login data 
     *           else, sends all data to the database and load "Main Scene
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

    // Go to Main Scene after login
    void MainScene()
    {
        loginPage.SetActive(false);
        registerPage.SetActive(false);
        // Load the Main Scene
        SceneManager.LoadScene("Main Scene");
    }





}
