using System;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [Header("Grenade Prefab")]
    [SerializeField] private GameObject grenadePrefab;

    [Header("Grenade Setting")]
    [SerializeField] private KeyCode throwKey = KeyCode.Mouse0;
    [SerializeField] private Transform throwPosition;
    [SerializeField] private Vector3 throwDirection = new Vector3(0, 1, 0);

    [Header("Grenade Force")]
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private float maxForce = 15f;


    [Header("Trajectory Setting")]
    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField]
    [Range(10, 100)]
    private int linePoints = 25;
    [SerializeField]
    [Range(10, 100)]
    private float timeBetweenPoint = 0.1f;


    private bool isCharging = false;
    private float chargeTime = 0.1f;
    //private Camera mainCamera;


    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = Camera.main;
        //InvokeRepeating("OnStartThrowing", 2.0f, 0.3f);
        Invoke("OnStartThrowing", 3);
        //OnStartThrowing();
    }

    // Update is called once per frame
    //void Update()
    //{


        //if (Input.GetKeyDown(throwKey))
        //{
        //    OnStartThrowing();
        //}
        //if (isCharging)
        //{
        //    ChargeThrow();
        //}
        //if (Input.GetKeyUp(throwKey))
        //{
        //    ReleaseThrow();
        //}
    //}

    public void OnStartThrowing()
    {
        //Debug.Log("StartThowing");

        isCharging = true;
        chargeTime = .1f;

        //trajectoryLine.enabled = true;

        //ReleaseThrow();
        ChargeThrow();
    }

    void ChargeThrow()
    {
        chargeTime += Time.deltaTime;

        //Vector3 grenadeVelocity = (this.transform.forward + throwDirection).normalized * Mathf.Min(chargeTime * throwForce, maxForce);

        //Debug.Log("grenadeVelocity =" + grenadeVelocity);

        //ShowTrajectory(throwPosition.position + throwPosition.forward, grenadeVelocity);

        ReleaseThrow();
    }
    void ReleaseThrow()
    {
        ThrowGrenade(Mathf.Min(chargeTime * throwForce, maxForce));
        //ThrowGrenade();
        isCharging = false;

        //trajectoryLine.enabled = false;


        Invoke("OnStartThrowing", UnityEngine.Random.Range(3,15));

    }
    //void ThrowGrenade(float force)
    //{
    //    Vector3 playerpos = SceneManagerScript.Instance.minimapBlipController.player.position;

    //    //Debug.Log("ThrowGrenade");
    //    Vector3 spawnPosition = throwPosition.position + this.transform.forward;

    //    GameObject grenade = Instantiate(grenadePrefab, spawnPosition, this.transform.rotation);

    //    Rigidbody rb = grenade.GetComponent<Rigidbody>();

    //    var dir = playerpos - transform.position;

    //    Vector3 finalThrowDiration = (dir + throwDirection).normalized;

    //    rb.AddForce(finalThrowDiration * force, ForceMode.VelocityChange);

    //    //Debug.Log("end throw");
    //}



    void ThrowGrenade(float force)
    {
        Vector3 playerpos = SceneManagerScript.Instance.minimapBlipController.player.position;

        Vector3 spawnPosition = throwPosition.position + this.transform.forward;

        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, this.transform.rotation);

        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        // Basic direction
        var dir = (playerpos - transform.position).normalized;

        // Add upward angle WITHOUT changing total distance
        float upwardFactor = 0.5f; // grenade arc mate (adjustable)
        Vector3 finalThrowDirection = new Vector3(dir.x, dir.y + upwardFactor, dir.z).normalized;

        // Apply force
        rb.AddForce(finalThrowDirection * force, ForceMode.VelocityChange);
    }









    private void ShowTrajectory(Vector3 origin, Vector3 speed)
    {


        trajectoryLine.enabled = true;
        trajectoryLine.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoint) + 1;
        Vector3 startPosition = throwPosition.position;
        Vector3 startVelocity = speed;
        int i = 0;
        trajectoryLine.SetPosition(i,startPosition);
        for (float time = 0; time < linePoints; time+= timeBetweenPoint)
        {

        }



        /*Vector3[] points = new Vector3[100];
        trajectoryLine.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = origin * time + 0.5f * Physics.gravity * time * time;
        }*/
        //trajectoryLine.SetPositions(points);
    }




}
