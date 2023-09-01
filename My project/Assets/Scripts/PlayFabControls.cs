using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFabControls : MonoBehaviour
{
    [SerializeField] GameObject registerPage, loginPage;
    public TMP_Text registerEmail, registerPassword, registerError, loginEmail, loginPassword, loginError;
    private string encryptedPassword;

    public void ChangeToLogin()
    {
        loginPage.SetActive(true);
        registerPage.SetActive(false);
    }

    public void ChangeToRegister()
    {
        loginPage.SetActive(false);
        registerPage.SetActive(true);
    }
    
}
