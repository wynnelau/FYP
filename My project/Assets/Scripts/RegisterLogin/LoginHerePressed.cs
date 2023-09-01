using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginHerePressed : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject registerPage;
    public GameObject loginPage;
    public void ChangeToLogin()
    {
        loginPage.SetActive(true);
        registerPage.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
