
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabLoginRegister : MonoBehaviour
{
    public GameObject registerPage, loginPage, getUserProfileDataPageStudent, getUserProfileDataPageOthers;
    public TMP_Text registerEmail, registerPassword, registerError, loginEmail, loginPassword, loginError;
    public TMP_Dropdown identityDropdown;
    // Update is called once per frame
    void Update()
    {
        // Enable "enter" as send data
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

    // Send the identity of the user
    public void SaveIdentity(string identity)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Identity", identity}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, SaveIdentitySuccess, SaveIdentityFail);
    }

    void SaveIdentitySuccess(UpdateUserDataResult result)
    {
        Debug.Log("Identity saved" +  result);
    }

    void SaveIdentityFail(PlayFabError error)
    {
        Debug.Log(error);
    }

    public string identity()
    {
        if (identityDropdown.value == 1) return "Student";
        else if (identityDropdown.value == 2) return "Professor/TA";
        else if (identityDropdown.value == 3) return "Staff";
        else return "";
    }

    // Register for an account
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
        // Save the user's identity
        SaveIdentity(userIdentity);
        // Go to the user profile set up page according to their identity;
        KnowMore(userIdentity);
    }

    public void RegisterFail(PlayFabError error)
    {
        registerError.text = error.GenerateErrorReport();
    }

    // Login to existing account
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

    // Go to set up user profile page after registering
    void KnowMore(string identity)
    {
        loginPage.SetActive(false);
        registerPage.SetActive(false);
        // Set the different profile pages as active
        if (identity == "Student")
        {
            getUserProfileDataPageStudent.SetActive(true);
        } 
        else if (identity == "Professor/TA" || identity == "Staff")
        {
            getUserProfileDataPageOthers.SetActive(true);
        }
        
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
