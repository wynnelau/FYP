using UnityEngine;

/*
 * Location: "Player" in "MainScene" scene
 * Purpose: Control the player movements
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
        leftRotationInput = -rotateRight;
        Quaternion rotationTargetLeft = Quaternion.Euler(0, leftRotationInput, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTargetLeft, rotationSpeed * Time.deltaTime);
    }

    /*
     * Purpose: Allow user to look right by rotating camera
     * Input: Called by update when 'E' key is pressed
     * Output: Rotate camera to right
     */
    void TurnRight()
    {
        rightRotationInput = rotateRight;
        Quaternion rotationTargetRight = Quaternion.Euler(0, rightRotationInput, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTargetRight, rotationSpeed * Time.deltaTime);
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

