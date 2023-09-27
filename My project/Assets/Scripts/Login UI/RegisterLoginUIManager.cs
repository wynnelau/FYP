using UnityEngine;
using TMPro;

/*
 * Location: Login UI, used by UIControls
 * Purpose: Manage the register and login UI pages
 * Tutorial used: https://www.youtube.com/watch?v=XPTPRaF2pd4
 */
public class RegisterLoginUIManager : MonoBehaviour
{
    public GameObject registerPage, loginPage, setUserProfileDataPageStudent, setUserProfileDataPageOthers;
    // Cannot clear input field when declared as Text or TMP_Text
    public TMP_InputField registerEmail, registerPassword, loginEmail, loginPassword;
    public TMP_Dropdown identityDropdown;
    public TMP_Text registerError, loginError;

    // Start is called before the first frame update
    /*
     * Purpose: User sees login page when they open application
     */
    void Start()
    {
        ChangeToLogin();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     * Purpose: Change the UI to the login UI, tied to the "loginHereButton"
     * Outcomes: activates the loginUI and deactivates every other UI, clears the input for the loginUI
     */
    public void ChangeToLogin()
    {
        loginPage.SetActive(true);
        registerPage.SetActive(false);
        setUserProfileDataPageStudent.SetActive(false);
        setUserProfileDataPageOthers.SetActive(false);
        loginEmail.text = "";
        loginPassword.text = "";
        loginError.text = " ";
    }

    /*
     * Purpose: Change the UI to the register UI, tied to the "registerHereButton"
     * Outcomes: activates the registerUI and deactivates every other UI, clears the input for the registerUI
     */
    public void ChangeToRegister()
    {
        loginPage.SetActive(false);
        registerPage.SetActive(true);
        setUserProfileDataPageStudent.SetActive(false);
        setUserProfileDataPageOthers.SetActive(false);
        registerEmail.text = "";
        registerPassword.text = "";
        registerError.text = " ";
        identityDropdown.value = 0;
    }

}