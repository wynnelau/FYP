using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SampleClassNetwork : NetworkBehaviour
{

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
