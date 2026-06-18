using UnityEngine;
using UnityEngine.AI;

public class CarFollower : MonoBehaviour
{
    public Transform target;  // Assign the player car transform here
    private NavMeshAgent agent;
    private bool shouldFollow = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("PlayerCar").transform;
        }
    }

    void Update()
    {
        if (shouldFollow)
        {
            agent.SetDestination(target.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCar"))
        {
            shouldFollow = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerCar"))
        {
            shouldFollow = false;
            agent.ResetPath();
        }
    }
}
