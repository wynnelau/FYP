using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainScene : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMain()
    {
        Relay relay = GetComponent<Relay>();
        relay.StopRelay();
        SceneManager.LoadScene("Main Scene");
    }
}
