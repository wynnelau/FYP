
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
        Debug.Log("Identity is " +  result);
    }

    void SaveIdentityFail(PlayFabError error)
    {
        Debug.Log(error);
    }

    public string identity()
    {
        if (identityDropdown.value == 0) return "Student";
        else if (identityDropdown.value == 1) return "Professor/TA";
        else if (identityDropdown.value == 2) return "Staff";
        else return "";
    }

    // Register for an account
    public void Register()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = registerEmail.text, Password = registerPassword.text, RequireBothUsernameAndEmail = false };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterFail);
    }

    public void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        registerError.text = " ";
        SaveIdentity(identity());
        KnowMore(identity());
    }

    public void RegisterFail(PlayFabError error)
    {
        registerError.text = error.GenerateErrorReport();
    }

    // Login to existing account
    public void Login()
    {
        var request = new LoginWithEmailAddressRequest { Email = loginEmail.text, Password = loginPassword.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginFail);
    }

    public void LoginSuccess(LoginResult result)
    {
        loginError.text = " ";
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
        SceneManager.LoadScene("Main Scene");
        /*SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main Scene Student"));*/
    }
    
}
