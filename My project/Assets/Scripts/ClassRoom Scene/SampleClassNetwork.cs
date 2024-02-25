using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SampleClassNetwork : NetworkBehaviour
{
    public static SampleClassNetwork Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ServerRpc]
    public void ToggleSampleClassServerRpc(bool isActive)
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
