using UnityEngine;

public class ActivateOnApproach : MonoBehaviour
{
    public GameObject objectToActivate; // Assign the GameObject you want to activate in the inspector
    public float activationDistance = 5f; // Adjust the distance as needed
    public GameObject TVUI;
    private Transform playerTransform;

    void Start()
    {
        // Assuming the player has a tag "Player"
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Start with the object deactivated
        objectToActivate.SetActive(false);
    }

    void Update()
    {
        // Check the distance between the player and this GameObject

        float distance = Vector3.Distance(transform.position, playerTransform.position);
       // Debug.Log(distance);

        // If the player is within the activation distance, activate the object
        if (distance < activationDistance)
        {
            objectToActivate.SetActive(true);
            TVUI.SetActive(false);
        }
        else
        {
            objectToActivate.SetActive(false);
            TVUI.SetActive(true);
        }
    }
}
