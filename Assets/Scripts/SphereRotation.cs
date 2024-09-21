using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // Rotation speed in degrees per second

    void Update()
    {
        // Get input from the horizontal axis (A/D keys or Left/Right arrows)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Get input from the vertical axis (W/S keys or Up/Down arrows)
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate rotation vector based on input
        Vector3 rotation = new Vector3(verticalInput, horizontalInput, 0);

    if (Input.GetAxis("Vertical") < 0) {
        rotationSpeed = 33f;
    }
// Apply rotation to the sphere
transform.Rotate(rotation * rotationSpeed * Time.deltaTime);
    }
}

