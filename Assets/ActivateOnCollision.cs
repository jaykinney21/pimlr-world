using UnityEngine;

public class ActivateOnCollision : MonoBehaviour
{
    public GameObject objectToActivate; // Drag the GameObject you want to activate in the inspector

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a BoxCollider (or you can check by tag or layer if needed)
        if (collision.collider is BoxCollider)
        {
            objectToActivate.SetActive(true);
            Invoke("DeactivateObject", 15f); // Deactivate after 15 seconds
        }
    }

    void DeactivateObject()
    {
        objectToActivate.SetActive(false);
    }
}
