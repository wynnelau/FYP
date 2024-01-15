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
        if (!IsOwner) return;

        if (firstPersonCamera.transform.IsChildOf(transform))
        {
            firstPersonCamera.enabled = true;
        }
    }
    
}
