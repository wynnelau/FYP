using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera orthographicCamera;
    // Start is called before the first frame update

    void SwitchToFirstPerson()
    {
        firstPersonCamera.enabled = true;
        orthographicCamera.enabled = false;
    }

    void SwitchToOrthographic()
    {
        firstPersonCamera.enabled = false;
        orthographicCamera.enabled = true;
    }
    void Start()
    {
        SwitchToOrthographic();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchToFirstPerson();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchToOrthographic();
        }
    }
}
