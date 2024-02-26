using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChangeOwnership : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AddListenerToJoinQuizServerRpc();
    }

    [ServerRpc]
    private void AddListenerToJoinQuizServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }
}
