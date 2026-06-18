using UnityEngine;
namespace JUTPS.VehicleSystem
{
    public class PoliceCarAI : MonoBehaviour
    {
        public Transform playerCar; // Reference to the player car's transform
        public float detectionDistance = 20f; // Distance at which the police car starts following the player car

        public Transform[] wheelTransforms; // Array to hold the transforms of the wheel models
        public WheelCollider[] wheelColliders; // Array to hold the wheel colliders

        private CarController carController; // Reference to the CarController script

        void Start()
        {
            carController = GetComponent<CarController>(); // Get the CarController script attached to this game object

            if (playerCar == null)
            {
                // Dynamically find the player car at the start of the game
                GameObject playerCarObject = GameObject.FindGameObjectWithTag("PlayerCar");
                if (playerCarObject != null)
                {
                    playerCar = playerCarObject.transform;
                }
                else
                {
                    Debug.LogError("Player car not found. Please check the player car tag.");
                }
            }
        }

        void FixedUpdate()
        {
            if (playerCar != null)
            {
                // Calculate the distance to the player car
                float distanceToPlayerCar = Vector3.Distance(transform.position, playerCar.position);

                // If the player car is within the detection distance, start following it
                if (distanceToPlayerCar < detectionDistance)
                {
                    FollowPlayerCar();
                }

                // Update the wheel positions and rotations
                UpdateWheelPositionsAndRotations();
            }
        }

        void FollowPlayerCar()
        {
            // Calculate the direction to the player car
            Vector3 directionToPlayerCar = (playerCar.position - transform.position).normalized;

            // Calculate the steering angle needed to turn towards the player car
            Vector3 localTarget = transform.InverseTransformPoint(playerCar.position);
            float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
            float steer = Mathf.Clamp(targetAngle * 0.1f, -1, 1); // Adjust 0.1f to a value that gives good steering behavior

            // Set the car's steering and throttle to move towards the player car
            carController.SetEngineInputs(steer, 1f, false); // Set throttle to 1 to move forward, and brake to false
        }

        void UpdateWheelPositionsAndRotations()
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                // Get the position and rotation of the wheel collider
                Vector3 position;
                Quaternion rotation;
                wheelColliders[i].GetWorldPose(out position, out rotation);

                // Set the position and rotation of the wheel model to match the wheel collider
                wheelTransforms[i].position = position;
                wheelTransforms[i].rotation = rotation;
            }
        }
    }
}