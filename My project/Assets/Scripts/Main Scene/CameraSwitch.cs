using UnityEngine;

/*
 *Location: Main Scene
 *Purpose: Switch to the different cameras, attached to "StudentControls"
 */

public class CameraSwitch : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public GameObject buildingFirstPerson, middleWall;
    public PlayerControls player;
    // Start is called before the first frame update
    /*
     * Purpose: User uses third person camera when they go into the scene
     */
    void Start()
    {
        SwitchToThirdPerson();
    }

    // Update is called once per frame
    /*
     * Purpose: Different keys to toggle between the two cameras
     * Outcomes: if there are missing inputs or PlayFabError, unable to send register data 
     *           else, sends all data to the database
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !firstPersonCamera.enabled)
        {
            SwitchToFirstPerson();
        }
        else if (Input.GetKeyDown(KeyCode.C) && !thirdPersonCamera.enabled)
        {
            SwitchToThirdPerson();
        }
    }
    /*
     * Purpose: enables the firstPersonCamera
     * Outcomes: activates the other half of the room and deactivates the middle wall
     */
    void SwitchToFirstPerson()
    {
        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
        buildingFirstPerson.SetActive(true);
        middleWall.SetActive(false);
    }
    /*
     * Purpose: enables the thirdPersonCamera
     * Outcomes: deactivates the other half of the room and activates the middle wall, 
     *           translates user to be in the first half of the room and facing forward
     */
    void SwitchToThirdPerson()
    {
        firstPersonCamera.enabled = false;
        thirdPersonCamera.enabled = true;
        buildingFirstPerson.SetActive(false);
        middleWall.SetActive(true);
        player.BottomLimit();
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    
}
