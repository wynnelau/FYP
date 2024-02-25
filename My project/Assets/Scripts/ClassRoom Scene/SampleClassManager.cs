using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;


public class SampleClassManager : NetworkBehaviour
{
    public Button enableSampleClass, disableSampleClass;
    
    public void EnableSampleClass()
    {
        gameObject.SetActive(true);
        enableSampleClass.gameObject.SetActive(false);
        disableSampleClass.gameObject.SetActive(true);
        ToggleSampleClassServerRpc(true);
    }

    public void DisableSampleClass()
    {
        gameObject.SetActive(false);
        enableSampleClass.gameObject.SetActive(true);
        disableSampleClass.gameObject.SetActive(false);
        ToggleSampleClassServerRpc(false);
    }

    [ServerRpc]
    private void ToggleSampleClassServerRpc(bool isActive)
    {
        gameObject.SetActive(isActive);

        // Call a client RPC to enable/disable the sampleClass GameObject on all clients
        ToggleSampleClassClientRpc(isActive);
    }

    [ClientRpc]
    private void ToggleSampleClassClientRpc(bool isActive)
    {
        if (!IsServer) // Only execute on clients
        {
            gameObject.SetActive(isActive);
        }
    }

}
