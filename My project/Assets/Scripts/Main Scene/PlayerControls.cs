using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float speed = 10.0f;
    private Vector3 offset;
    private float firstPersonBoundary = -22.5f;

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

