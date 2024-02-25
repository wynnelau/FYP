using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SyncObjectVisibility : NetworkBehaviour
{
    private bool isVisible = false;

    // Called when the object is spawned on the network
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Set the initial visibility state when the object is spawned
        SetVisibility(isVisible);
    }

    // Method to toggle visibility
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        SetVisibility(isVisible);

        // Call ServerRpc method to synchronize visibility across the network
        ToggleVisibilityServerRpc(isVisible);
    }

    // Method to set visibility locally
    private void SetVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }

    // ServerRpc method to synchronize visibility across the network
    [ServerRpc]
    private void ToggleVisibilityServerRpc(bool visible)
    {
        SetVisibility(visible);
    }
}
