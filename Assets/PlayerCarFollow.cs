using UnityEngine;
using JUTPS.JUInputSystem;

namespace JUTPS.VehicleSystem
{
    public class PlayerCarFollow : MonoBehaviour
    {
        public Transform playerCar; // Reference to the player car's transform
        public float followDistance = 20f; // Distance at which to start following the player car
        private bool isFollowingPlayer = false; // Whether or not this car is currently following the player car

        private CarController carController; // Reference to the CarController script

        void Start()
        {
            carController = GetComponent<CarController>(); // Get the CarController script attached to this game object

            if (playerCar == null)
            {
                playerCar = GameObject.FindGameObjectWithTag("PlayerCar").transform; // Find the player car dynamically
            }
        }

        void Update()
        {
            // Check the distance to the player car
            if (playerCar != null)
            {
                float distanceToPlayerCar = Vector3.Distance(transform.position, playerCar.position);
                if (distanceToPlayerCar < followDistance)
                {
                    isFollowingPlayer = true;
                }
                else
                {
                    isFollowingPlayer = false;
                }
            }
        }

        void FixedUpdate()
        {
            // If following the player, set the steering and throttle to move towards the player
            if (isFollowingPlayer && carController != null)
            {
                Vector3 localTarget = transform.InverseTransformPoint(playerCar.position);
                float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
                float steer = Mathf.Clamp(targetAngle * 0.1f, -1, 1); // Adjust 0.1f to a value that gives good steering behavior

                carController.SetEngineInputs(steer, 1f, false); // Set throttle to 1 to move forward, and brake to false
            }
        }
    }
}
