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

    [ClientRpc]
    public void ToggleSampleClassClientRpc(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    [ServerRpc]
    public void ToggleSampleClassServerRpc(bool isActive)
    {
        ToggleSampleClassClientRpc(isActive);
    }
}
