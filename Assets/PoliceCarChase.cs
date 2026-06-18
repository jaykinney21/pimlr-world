using UnityEngine;
using JUTPS.VehicleSystem;

[RequireComponent(typeof(CarController))]
public class PoliceCarChase : MonoBehaviour
{
    public float chaseRadius = 10f; // The radius in which the police car will start chasing the player car.
    public Transform playerCar; // Reference to the player car's transform.
    public float chaseSpeed = 5f; // Speed at which the police car will chase the player car.

    private CarController carController;
    private bool isChasing = false;

    private void Start()
    {
        carController = GetComponent<CarController>();

        // Create a sphere collider for detecting the player car.
        SphereCollider detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = chaseRadius;
    }

    private void Update()
    {
        if (isChasing)
        {
            // Calculate the direction to the player car.
            Vector3 chaseDirection = (playerCar.position - transform.position).normalized;

            // Mimic player input by multiplying with chaseSpeed.
            float horizontalInput = chaseDirection.x * chaseSpeed;
            float verticalInput = chaseDirection.z * chaseSpeed;

            // Clamp the values between -1 and 1.
            horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);
            verticalInput = Mathf.Clamp(verticalInput, -1f, 1f);

            // Set the inputs for the car controller to chase the player car.
            carController.SetEngineInputs(horizontalInput, verticalInput, false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCar"))
        {
            Debug.Log("Player car detected!");
            isChasing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerCar"))
        {
            Debug.Log("Player car left detection area.");
            isChasing = false;
        }
    }

}
