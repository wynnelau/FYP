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
    [SerializeField] GameObject registerPage, loginPage;
    public TMP_Text registerEmail, registerPassword, registerError, loginEmail, loginPassword, loginError;
    public Text registerUsernameLegacy;
    private string encryptedPassword;

    public void ChangeToLogin()
    {
        loginPage.SetActive(true);
        registerPage.SetActive(false);
        loginError.text = " ";
        registerError.text = " ";
    }

    public void ChangeToRegister()
    {
        loginPage.SetActive(false);
        registerPage.SetActive(true);
        loginError.text = " ";
        registerError.text = " ";
    }

    string Encrypt(string pass)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        return s.ToString();
    }

    public void Register()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = registerEmail.text, Password = Encrypt(registerPassword.text), Username = registerUsernameLegacy.text };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterFail);
    }

    public void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        loginError.text = " ";
        registerError.text = " ";
        StartGame();
    }

    public void RegisterFail(PlayFabError error)
    {
        registerError.text = error.GenerateErrorReport();
    }

    public void LoginSuccess(LoginResult result)
    {
        loginError.text = " ";
        registerError.text = " ";
        StartGame();
    }

    public void LoginFail(PlayFabError error)
    {
        loginError.text = error.GenerateErrorReport();
    }

    public void Login()
    {
        var request = new LoginWithEmailAddressRequest { Email = loginEmail.text, Password = Encrypt(loginPassword.text) };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginFail);
    }

    void StartGame()
    {
        loginPage.SetActive(false);
        registerPage.SetActive(false);
    }
    
}
