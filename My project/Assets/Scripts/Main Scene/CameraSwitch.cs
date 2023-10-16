using UnityEngine;

/*
 * Location: MainSceneControls
 * Purpose: Switch between cameras when 'C' key is pressed
 */

public class CameraSwitch : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public GameObject buildingFirstPerson, middleWall;
    public PlayerControls player;

    /*
     * Purpose: Ensures that user is in third person when "MainScene" scene is loaded
     *          Start is called before the first frame update
     * Input: NA
     * Output: Call SwitchToThirdPerson()
     */
    void Start()
    {
        SwitchToThirdPerson();
    }

    /*
     * Purpose: Switch between cameras when the 'C' key is pressed
     * Input: Press the 'C' key
     * Output: If currently using first person camera, switch to third person camera
     *         else if currently using third person camera, switch to first person camera
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !firstPersonCamera.enabled && player.enabled)
        {
            SwitchToFirstPerson();
        }
        else if (Input.GetKeyDown(KeyCode.C) && !thirdPersonCamera.enabled && player.enabled)
        {
            SwitchToThirdPerson();
        }
    }

    /*
     * Purpose: Enables the firstPersonCamera with respective settings
     * Input: Called by Update() when the 'C' key is pressed
     * Output: Enable firstPersonCamera and set buildingFirstPerson as active
     *         Disable thirdPersonCamera and set middleWall as inactive
     */
    void SwitchToFirstPerson()
    {
        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
        buildingFirstPerson.SetActive(true);
        middleWall.SetActive(false);
    }
    /*
     * Purpose: Enables the thirdPersonCamera with respective settings
     * Input: Called by Update() when the 'C' key is pressed
     * Output: Enable thirdPersonCamera, set middleWall as active and move user accordingly
     *         Disable firstPersonCamera and set buildingFirstPerson as inactive
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
