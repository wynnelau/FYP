using UnityEngine;
using TMPro;

/*
 * Location: Login UI/ UIControls
 * Purpose: Manage the "RegisterPage" UI, "LoginPage" UI, "SetUserProfilePageStudent" UI and "SetUserProfilePageOthers" UI
 * Tutorial used: https://www.youtube.com/watch?v=XPTPRaF2pd4
 */
public class RegisterLoginUIManager : MonoBehaviour
{
    public GameObject registerPageUI, loginPageUI, setUserProfilePageStudentUI, setUserProfileDataOthersUI;
    public TMP_InputField registerEmail, registerPassword, loginEmail, loginPassword;
    public TMP_Dropdown identityDropdown;
    public TMP_Text registerError, loginError;

    /*
     * Purpose: Ensures that user sees "LoginPage" UI when application opens, and other UIs are inactive
     *          Start is called before the first frame update
     * Input: NA
     * Output: Set all UIs to inactive and call ChangeToLogin()
     */
    void Start()
    {
        loginPageUI.SetActive(false);
        registerPageUI.SetActive(false);
        setUserProfilePageStudentUI.SetActive(false);
        setUserProfileDataOthersUI.SetActive(false);
        ChangeToLogin();
    }

    /*
     * Purpose: Change to "LoginPage" UI when the "LoginHereButton" button in "RegisterPage" UI is clicked
     * Input: Click the "LoginHereButton" button in "RegisterPage" UI
     * Output: Opens "LoginPage" UI, clears previous texts for loginEmail, loginPassword and loginError
     */
    public void ChangeToLogin()
    {
        loginPageUI.SetActive(true);
        registerPageUI.SetActive(false);
        loginEmail.text = "";
        loginPassword.text = "";
        loginError.text = " ";
    }

    /*
     * Purpose: Change to "RegisterPage" UI when the "RegisterHereButton" button in "LoginPage" UI is clicked
     * Input: Click the "RegisterHereButton" button in "LoginPage" UI
     * Output: Opens "RegisterPage" UI, clears previous texts for registerEmail, registerPassword, registerError and sets idenityDropdown to 0
     */
    public void ChangeToRegister()
    {
        loginPageUI.SetActive(false);
        registerPageUI.SetActive(true);
        registerEmail.text = "";
        registerPassword.text = "";
        registerError.text = " ";
        identityDropdown.value = 0;
    }

}