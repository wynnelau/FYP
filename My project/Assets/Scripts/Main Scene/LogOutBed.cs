using UnityEngine;

/*
 *Location: Main Scene
 *Purpose: Trigger the logoutUI when user comes into contact with "Bed", attached to "Bed"
 */

public class LogOutBed : MonoBehaviour
{
    public GameObject logoutUI;
    public PlayerControls player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Purpose: Trigger the logoutUI when user comes into contact with "Bed"
     * Outcomes: logoutUI is activated
     */
    private void OnTriggerEnter(Collider other)
    {
        logoutUI.SetActive(true);
        player.enabled = false;
    }
}
