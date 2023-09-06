using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitialise : MonoBehaviour
{
    public GameObject logoutUI, userProfileStudentUI;
    // Start is called before the first frame update
    void Start()
    {
        logoutUI.SetActive(false);
        userProfileStudentUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
