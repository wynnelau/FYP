using UnityEngine;

/*
 * Location: Main Scene/ Player
 * Purpose: Control the player movements
 */

public class PlayerControls : MonoBehaviour
{
    private static PlayerControls instance;
    public Camera firstPersonCamera;

    private float horizontalInput;
    private float verticalInput;
    private float translateSpeed = 10.0f;

    private float rotationSpeed = 45f;

    private float firstPersonBoundary = -23f;

    /*
     * Purpose: Make sure the gameObject is persistent across scenes
     * Input: NA
     * Output: If instance is null, let instance = this and DontDestroyOnLoad
     *         else destroy the gameObject
     */
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("PlayerControls Awake InstanceIsNull");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("PlayerControls Awake DestroyGameObject");
        }

    }

    /*
     * Purpose: Allow user to move around, and if user is in firstPerson mode, allow to look left and right
     * Input: Click on keys 'W', 'A', 'S', 'D' to move around, and 'Q', 'E' to rotate camera
     * Output: User can move "Player" around, and rotate the camera when in firstPerson by calling TurnLeft() or TurnRight()
     */
    void Update()
    {
        // Allow player to move around
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(-Vector3.right * horizontalInput * translateSpeed * Time.deltaTime);
        transform.Translate(-Vector3.forward * verticalInput * translateSpeed * Time.deltaTime);

        // Allow player to look left and right
        if (firstPersonCamera.enabled)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                TurnLeft();
            }
            else if (Input.GetKey(KeyCode.E))
            {
                TurnRight();
            }
        }
        
    }

    /*
     * Purpose: Allow user to look left by rotating camera
     * Input: Called by update when 'Q' key is pressed
     * Output: Rotate camera to left
     */
    void TurnLeft()
    {
        Quaternion currentRotation = transform.rotation;
        float newYRotation = currentRotation.eulerAngles.y - rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, newYRotation , 0);
    }

    /*
     * Purpose: Allow user to look right by rotating camera
     * Input: Called by update when 'E' key is pressed
     * Output: Rotate camera to right
     */
    void TurnRight()
    {
        Quaternion currentRotation = transform.rotation;
        float newYRotation = currentRotation.eulerAngles.y + rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, newYRotation, 0);
    }
    
    /*
     * Purpose: Make sure "Player" is not out of bounds when we switch from firstPerson to thirdPerson
     * Input: Called by CameraSwitch when SwitchToThirdPerson() is called
     * Output: "Player" translates in the z direction
     */
    public void BottomLimit()
    {
        // Limit bottom
        if (transform.position.z < firstPersonBoundary)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, firstPersonBoundary);
        }
    }

    
}

