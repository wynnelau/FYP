using UnityEngine;

/*
 * Location: ClassRoom Scene/ Player prefab
 * Purpose: Switch to first person camera
 */

public class CameraManager : MonoBehaviour
{
    public Camera firstPersonCamera;

    /*
     * Purpose: Enables the firstPersonCamera 
     * Input: Called by JoinRelay and CreateRelay
     * Output: Enable firstPersonCamera
     */
    public void SwitchToFirstPerson()
    {
        Debug.Log("CameraManager SwitchToFirstPerson");
        firstPersonCamera.enabled = true;
    }
    
}
