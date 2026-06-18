using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

public class RotateAroundObject : MonoBehaviour
{
    public Transform targetObject; // The object around which the camera will rotate
    public float rotationSpeed = 10.0f;
    private float rotationY = 0f;

    private void Update()
    {
        // Get the current mouse delta
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        if (Mouse.current.leftButton.isPressed) // Check if the left mouse button is pressed
        {
            // Capture horizontal mouse movement
            rotationY += mouseDelta.x * rotationSpeed * Time.deltaTime;

            // Rotate the camera around the target object
            transform.RotateAround(targetObject.position, Vector3.up, rotationY);
        }
    }
}
