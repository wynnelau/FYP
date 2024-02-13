using UnityEngine;
using Unity.Netcode;

/*
 * Location: ClassRoom Scene/ Player prefab
 * Purpose: Switch to first person camera and audio
 */

public class CameraManager : NetworkBehaviour
{
    public Camera firstPersonCamera;
    public AudioListener audioListener;


    /*
     * Purpose: Enables the firstPersonCamera and audio
     * Input: NA
     * Output: Enable firstPersonCamera and audio
     */
    void Start()
    {
        Debug.Log("CameraManager enter");
        if (!IsOwner) return;
        Debug.Log("CameraManager cameraEnabled");
        firstPersonCamera.enabled = true;
        audioListener.enabled = true;


    }
    
}
