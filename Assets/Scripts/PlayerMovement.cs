using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    public float speed = 5.0f;
    private float angleAdjustment = 0.0f;
    private float forwardInput;
    private float previousForwardInput;

    public Transform rampTransform;
    public Transform cameraTransform; // Reference to the camera
    public Vector3 cameraOffset = new Vector3(0, 12, -10); // Desired offset of the camera from the player
    public float cameraFollowSpeed = 5.0f; // Speed at which the camera follows

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // Find the ramp object if rampTransform is not assigned
        if (rampTransform == null)
        {
            GameObject rampObject = GameObject.FindGameObjectWithTag("Ramp1");
            if (rampObject != null)
            {
                rampTransform = rampObject.transform;
            }
            else
            {
                Debug.LogError("Ramp object not found. Please make sure it's tagged as 'Ramp1'.");
            }
        }

        // Ensure the camera is assigned
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Assign main camera if not assigned
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Store the previous forwardInput for swap detection
        previousForwardInput = forwardInput;

        // Get the current forwardInput
        forwardInput = Input.GetAxis("Vertical");

        // Calculate the ramp angle
        float height = rampTransform.localScale.y;
        float baseLength = rampTransform.localScale.x;
        float rampAngle = Mathf.Atan2(height, baseLength) * Mathf.Rad2Deg;

        // Move the player in the correct direction
        Vector3 moveDirection = Quaternion.Euler(-18f + angleAdjustment, 0, 0) * Vector3.forward;
        transform.Translate(moveDirection * Time.deltaTime * speed * forwardInput);

        // Set animation states
        animator.SetBool("isPushing", forwardInput > 0);
        animator.SetBool("isHolding", forwardInput < 0);
        animator.SetBool("isIdle", forwardInput == 0);

        // Detect if the player is swapping directions
        bool isSwapping = (previousForwardInput > 0 && forwardInput < 0) || (previousForwardInput < 0 && forwardInput > 0);
        animator.SetBool("isSwapping", isSwapping);

        // Update the camera position to follow the player
        cameraOffset = new Vector3(0, 10, -10); // Desired offset of the camera from the player
        Vector3 desiredCameraPosition = transform.position + transform.TransformDirection(cameraOffset);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredCameraPosition, cameraFollowSpeed * Time.deltaTime);

        // Optionally, adjust the camera's rotation to look at the player
        cameraTransform.LookAt(transform.position + new Vector3(0, 2, 0));
    }
}