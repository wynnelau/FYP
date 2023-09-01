using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterHerePressed : MonoBehaviour
{
    public GameObject registerPage;
    public GameObject loginPage;
    public void ChangeToRegister()
    {
        loginPage.SetActive(false);
        registerPage.SetActive(true);
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
