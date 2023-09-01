using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterLoginChange : MonoBehaviour
{

    public GameObject registerPage;
    public GameObject loginPage;
    public void ChangeToRegister()
    {
        loginPage.SetActive(false);
        registerPage.SetActive(true);
    }
    public void ChangeToLogin()
    {
        loginPage.SetActive(true);
        registerPage.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
