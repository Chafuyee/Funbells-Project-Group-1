using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    public AudioSource rockSound;

    public float rotationSpeed = 100f;
    public float speed = 1f;
    private float angleAdjustment = 0.0f;
    private float forwardInput;
    private float previousForwardInput;
    public bool started = false;

    public Transform UITransform;

    public Transform rampTransform;
    public Transform rockTransform;
    public Transform cameraTransform; // Reference to the camera
    public Vector3 cameraOffset = new Vector3(0, 12, -10); // Desired offset of the camera from the player
    public float cameraFollowSpeed = 5.0f; // Speed at which the camera follows

    public Transform startPosition;
    public Transform endPosition;

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
        forwardInput = GameManager.instance.GetForwardInput();

        //if (forwardInput == 0) {
        //    rockSound.Play();
        //}

        // Calculate the ramp angle
        float height = rampTransform.localScale.y;
        float baseLength = rampTransform.localScale.x;
        float rampAngle = Mathf.Atan2(height, baseLength) * Mathf.Rad2Deg;


        // Move the player in the correct direction
        Vector3 moveDirection = Quaternion.Euler(-18.5f + angleAdjustment, 0, 0) * Vector3.forward;
        transform.Translate(moveDirection * Time.deltaTime * speed * forwardInput);
        
        // MOVE UI
        UITransform.Translate(moveDirection * Time.deltaTime * speed * forwardInput);

        // Set animation states
        animator.SetBool("isPushing", forwardInput > 0);
        animator.SetBool("isHolding", forwardInput < 0);
        animator.SetBool("isIdle", forwardInput == 0);
        if (forwardInput > 0)
        {
            speed = 3f;
            rotationSpeed = 100f;
        } else if (forwardInput < 0) {
            speed = 1f;
            rotationSpeed = 33f;
        }

        // ROCK MOVEMENT

        Vector3 rotation = new Vector3(forwardInput, 0, 0);
        rockTransform.Rotate(rotation * rotationSpeed * Time.deltaTime);

        // Detect if the player is swapping directions
        bool isSwapping = (previousForwardInput > 0 && forwardInput < 0) || (previousForwardInput < 0 && forwardInput > 0);
        animator.SetBool("isSwapping", isSwapping);

        if (started == true) {
            // Update the camera position to follow the player
            cameraOffset = new Vector3(0, 8f, -10); // Desired offset of the camera from the player
            Vector3 desiredCameraPosition = transform.position + transform.TransformDirection(cameraOffset);
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredCameraPosition, cameraFollowSpeed * Time.deltaTime);

            startPosition.position = Vector3.Lerp(cameraTransform.position, desiredCameraPosition, cameraFollowSpeed * Time.deltaTime);
            endPosition.position = Vector3.Lerp(cameraTransform.position, desiredCameraPosition, cameraFollowSpeed * Time.deltaTime);

            // Optionally, adjust the camera's rotation to look at the player
            // OLD VALUE 10
            cameraTransform.LookAt(transform.position + new Vector3(0, 14, 6.5f));
            startPosition.LookAt(transform.position + new Vector3(0, 14, 6.5f));
            endPosition.LookAt(transform.position + new Vector3(0, 14, 6.5f));

        }
    }

    public void setStarted()
    {
        started = true;
    }
}