using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Camera firstPersonCamera;
    private float horizontalInput;
    private float verticalInput;
    private float translateSpeed = 10.0f;

    private float firstPersonBoundary = -22.5f;

    private float leftRotationInput;
    private float rightRotationInput;
    private float rotationSpeed = 2.5f;
    private float rotateRight = -90f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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


    void TurnLeft()
    {
        leftRotationInput = -rotateRight;
        Quaternion rotationTargetLeft = Quaternion.Euler(0, leftRotationInput, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTargetLeft, rotationSpeed * Time.deltaTime);
    }

    void TurnRight()
    {
        rightRotationInput = rotateRight;
        Quaternion rotationTargetRight = Quaternion.Euler(0, rightRotationInput, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTargetRight, rotationSpeed * Time.deltaTime);
    }

    public void BottomLimit()
    {
        // Limit bottom
        if (transform.position.z < firstPersonBoundary)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, firstPersonBoundary);
        }
    }

    
}

