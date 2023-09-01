using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RegisterLoginInitialise : MonoBehaviour
{
    public GameObject registerPage;
    public GameObject loginPage;

    // Start is called before the first frame update
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