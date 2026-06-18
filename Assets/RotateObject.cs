using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 10.0f;
    private float rotationY = 180f;

    private void Update()
    {
        // Get the current mouse delta
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        if (Mouse.current.leftButton.isPressed) // Check if the left mouse button is pressed
        {
            // Capture horizontal mouse movement
            rotationY -= mouseDelta.x * rotationSpeed * Time.deltaTime;

            // Apply rotation around the Y-axis (upward axis in Unity by default)
            transform.rotation = Quaternion.Euler(0, rotationY, 0);
        }
    }
}
