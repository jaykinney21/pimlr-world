using UnityEngine;

public class PlmrLookAtCamera : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    void Awake()
    {
        cam = Camera.main.gameObject;
    }

    private void Update()
    {
            transform.forward = cam.transform.forward;
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
    
}
