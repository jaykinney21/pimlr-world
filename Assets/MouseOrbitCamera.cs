using UnityEngine;

public class MouseOrbitCamera : MonoBehaviour
{
    public Transform target;  // The target object to orbit around
    public float rotateSpeed = 5.0f;  // The speed of rotation

    private Vector3 offset;
    private float currentRotationY;

    private void Start()
    {
        // Calculate the initial offset between the camera and target.
        offset = target.position - transform.position;
    }

    private void LateUpdate()
    {
        // Capture horizontal mouse movement
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        currentRotationY += horizontal;

        // Compute the desired position
        Quaternion rotation = Quaternion.Euler(0, currentRotationY, 0);
        Vector3 position = target.position - (rotation * offset);

        // Update the camera's position and rotation to face the target
        transform.position = position;
        transform.LookAt(target);
    }
}
