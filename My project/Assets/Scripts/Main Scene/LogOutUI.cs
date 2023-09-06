using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogOutUI : MonoBehaviour
{
    public GameObject logoutUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void logoutConfirm()
    {
        Application.Quit();
    }

    public void logoutCancel()
    {
        logoutUI.SetActive(false);
    }
}
