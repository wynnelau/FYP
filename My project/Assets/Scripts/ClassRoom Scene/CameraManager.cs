using UnityEngine;
using Unity.Netcode;

/*
 * Location: ClassRoom Scene/ Player prefab
 * Purpose: Switch to first person camera
 */

public class CameraManager : NetworkBehaviour
{
    public Camera firstPersonCamera;

    /*
     * Purpose: Enables the firstPersonCamera 
     * Input: Called by JoinRelay and CreateRelay
     * Output: Enable firstPersonCamera
     */
    public void SwitchToFirstPerson()
    {
        Debug.Log("CameraManager enter");
        if (!IsOwner) return;
        Debug.Log("CameraManager cameraEnabled");
        firstPersonCamera.enabled = true;
            
        
    }
    
}
