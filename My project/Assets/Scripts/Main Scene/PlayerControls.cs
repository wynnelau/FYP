using UnityEngine;

/*
 *Location: Main Scene, attached to "Player"
 *Purpose: Control the player
 */

public class PlayerControls : MonoBehaviour
{
    public Camera firstPersonCamera;
    private float horizontalInput;
    private float verticalInput;
    private float translateSpeed = 10.0f;

    private float leftRotationInput;
    private float rightRotationInput;
    private float rotationSpeed = 2.5f;
    private float rotateRight = -90f;

    private float firstPersonBoundary = -23f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    /*
     * Purpose: Allow user to move around, and if user is in firstPerson mode, allow to look left and right
     * Outcomes: 
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
     * Purpose: Allow user to look left
     * Outcomes: User rotates left with camera
     */
    void TurnLeft()
    {
        leftRotationInput = -rotateRight;
        Quaternion rotationTargetLeft = Quaternion.Euler(0, leftRotationInput, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTargetLeft, rotationSpeed * Time.deltaTime);
    }

    /*
     * Purpose: Allow user to look right
     * Outcomes: User rotates right with camera
     */
    void TurnRight()
    {
        rightRotationInput = rotateRight;
        Quaternion rotationTargetRight = Quaternion.Euler(0, rightRotationInput, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTargetRight, rotationSpeed * Time.deltaTime);
    }
    /*
     * Purpose: Translate the user when they change to thirdPerson mode, called by CameraSwitch
     * Outcomes: User translates in the z direction
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

