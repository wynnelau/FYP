using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEditor.PackageManager;

public class PlayFabControls : MonoBehaviour
{
    public GameObject registerPage, loginPage;
    public TMP_Text registerEmail, registerPassword, registerError, loginEmail, loginPassword, loginError;
    public Text registerUsernameLegacy;

    // Update is called once per frame
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

    public void Register()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = registerEmail.text, Password = registerPassword.text, Username = registerUsernameLegacy.text };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterFail);
    }

    public void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        registerError.text = " ";
        StartGame();
    }

    public void RegisterFail(PlayFabError error)
    {
        registerError.text = error.GenerateErrorReport();
    }

    public void Login()
    {
        var request = new LoginWithEmailAddressRequest { Email = loginEmail.text, Password = loginPassword.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginFail);
    }

    public void LoginSuccess(LoginResult result)
    {
        loginError.text = " ";
        StartGame();
    }

    public void LoginFail(PlayFabError error)
    {
        loginError.text = error.GenerateErrorReport();
    }

    void StartGame()
    {
        loginPage.SetActive(false);
        registerPage.SetActive(false);
    }
    
}
