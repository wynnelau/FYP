using UnityEngine;
using TMPro;

/*
 * Location: LoginUI Scene/ LoginUIControls
 * Purpose: Manage the "RegisterPage" UI, "LoginPage" UI, "SetUserProfilePageStudent" UI and "SetUserProfilePageOthers" UI
 */
public class RegisterLoginUIManager : MonoBehaviour
{
    public GameObject registerUI, loginUI, setUserProfileStudentUI, setUserProfileOthersUI;
    public TMP_InputField registerEmail, registerPassword, loginEmail, loginPassword;
    public TMP_Dropdown identityDropdown;
    public TMP_Text registerError, loginError;

    /*
     * Purpose: Ensures that user sees "Login" UI when application opens, and other UIs are inactive
     *          Start is called before the first frame update
     * Input: NA
     * Output: Set all UIs to inactive and call ChangeToLogin()
     */
    void Start()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        setUserProfileStudentUI.SetActive(false);
        setUserProfileOthersUI.SetActive(false);
        ChangeToLogin();
    }

    /*
     * On Click: LoginUI Scene/ LoginUISceneCanvas/ Register/ LogInHereButton
     * Purpose: Change to "Login" UI when the "LogInHereButton" button in "Register" UI is clicked
     * Input: Click the "LogInHereButton" button in "Register" UI
     * Output: Opens "Login" UI, clears previous texts for loginEmail, loginPassword and loginError
     */
    public void ChangeToLogin()
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
        loginEmail.text = "";
        loginPassword.text = "";
        loginError.text = " ";
    }

    /*
     * On Click: LoginUI Scene/ LoginUISceneCanvas/ Login/ RegisterHereButton
     * Purpose: Change to "Register" UI when the "RegisterHereButton" button in "Login" UI is clicked
     * Input: Click the "RegisterHereButton" button in "Login" UI
     * Output: Opens "Register" UI, clears previous texts for registerEmail, registerPassword, registerError and sets idenityDropdown to 0
     */
    public void ChangeToRegister()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
        registerEmail.text = "";
        registerPassword.text = "";
        registerError.text = " ";
        identityDropdown.value = 0;
    }

}