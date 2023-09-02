using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RegisterLoginManage : MonoBehaviour
{
    public GameObject registerPage, loginPage;
    // Cannot clear input field when declared as Text or TMP_Text
    public InputField registerUsernameLegacy;
    public TMP_InputField registerEmail, registerPassword, loginEmail, loginPassword;

    public TMP_Text registerError, loginError;

    // Change to login page
    public void ChangeToLogin()
    {
        loginPage.SetActive(true);
        registerPage.SetActive(false);
        loginEmail.text = "";
        loginPassword.text = "";
        loginError.text = " ";
    }

    // Change to register page
    public void ChangeToRegister()
    {
        loginPage.SetActive(false);
        registerPage.SetActive(true);
        registerUsernameLegacy.text = "";
        registerEmail.text = "";
        registerPassword.text = "";
        registerError.text = " ";
    }

    // Start is called before the first frame update
    // Start application at login page
    void Start()
    {
        registerPage.SetActive(false);
        loginPage.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    
}