using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    public float speed = 5.0f;
    private float angleAdjustment = 0.0f;
    private float forwardInput;

    public Transform rampTransform;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Check if rampTransform is still null
        if (rampTransform == null)
        {
            GameObject rampObject = GameObject.FindGameObjectWithTag("Ramp1");
            rampTransform = rampObject.transform;
        }
        forwardInput = Input.GetAxis("Vertical");

        float height = rampTransform.localScale.y;
        float baseLength = rampTransform.localScale.x;

        float rampAngle = Mathf.Atan2(height, baseLength) * Mathf.Rad2Deg;

        Vector3 moveDirection = Quaternion.Euler(-rampAngle/2 + angleAdjustment, 0, 0) * Vector3.forward;

        transform.Translate(moveDirection * Time.deltaTime * speed * forwardInput);
        
        animator.SetBool("isPushing", forwardInput > 0);
        animator.SetBool("isHolding", forwardInput < 0);
    }
}