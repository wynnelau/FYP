using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float speed = 10.0f;
    private float xRange = 23;
    private float leftBoundary = -7;
    private float rightBoundary = 22;
    private Vector3 offset;
    


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(-Vector3.right * horizontalInput * speed * Time.deltaTime);
        transform.Translate(-Vector3.forward * verticalInput * speed * Time.deltaTime);

        /*// Keep player in bounds

        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        // Limit left side
        if (transform.position.z < leftBoundary)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, leftBoundary);
        }
        // Limit right side
        else if (transform.position.z > rightBoundary)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, rightBoundary);
        }*/













    }
}

