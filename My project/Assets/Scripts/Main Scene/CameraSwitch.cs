using UnityEngine; 

public class CameraSwitch : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public GameObject buildingFirstPerson, middleWall;
    public PlayerControls player;
    // Start is called before the first frame update

    void SwitchToFirstPerson()
    {
        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
        buildingFirstPerson.SetActive(true);
        middleWall.SetActive(false);
    }

    void SwitchToThirdPerson()
    {
        firstPersonCamera.enabled = false;
        thirdPersonCamera.enabled = true;
        buildingFirstPerson.SetActive(false);
        middleWall.SetActive(true);
        player.BottomLimit();
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    void Start()
    {
        SwitchToThirdPerson();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchToFirstPerson();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchToThirdPerson();
        }
    }
}
