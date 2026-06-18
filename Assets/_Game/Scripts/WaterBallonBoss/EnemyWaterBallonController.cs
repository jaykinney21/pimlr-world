using JUTPS.ItemSystem;
using JUTPSEditor.JUHeader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaterBallonController : HoldableItem
{
   /* public Transform player;
    public GameObject grenadePrefab;
    public Transform throwPoint;
    public float throwForce = 10f;
    public float throwInterval = 3f;

    private void Start()
    {
        InvokeRepeating("ThrowGrenadeAtPlayer", 0f, throwInterval);
    }

    private void ThrowGrenadeAtPlayer()
    {
        // Ensure player and throw point are valid
        if (player == null || throwPoint == null)
        {
            Debug.LogWarning("Player or throwPoint not set");
            return;
        }

        // Calculate the direction to the player's position
        Vector3 toPlayer = player.position - throwPoint.position;

        // Calculate the throw direction based on the player's position
        Vector3 throwDirection = toPlayer.normalized;

        // Instantiate and throw the grenade
        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        // Optional: Rotate the grenade to face the throw direction
        grenade.transform.rotation = Quaternion.LookRotation(throwDirection, Vector3.up);
        IsThrowed = true;

    }


    public GameObject ExplosionPrefab;
    public float TimeToExplode;
    public float TimeToDestroyExplosionPrefab = 5;
    private float currentTimeToExplode;
    private bool IsThrowed;
    public  void Update()
    {

        if (IsThrowed == true)
        {
            Debug.Log("IS THROWED");
            //Timer
            currentTimeToExplode += Time.deltaTime;
            if (currentTimeToExplode >= TimeToExplode)
            {
                //Spawn a explosion Prefab
                GameObject explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

                //Destroy explosion prefab at seconds
                Destroy(explosion, TimeToDestroyExplosionPrefab);

                Debug.Log("explosion prefab at seconds");
                //Destroy granade imediately
                Destroy(gameObject);
            }
        }
    }*/



    [JUHeader("Throw Settings")]
    public string AnimationTriggerParameterName = "Throw";
    public float ThrowForce = 10, ThrowUpForce = 10, RotationForce = 10;
    //public float ItemMass;
    public float SecondsToDestroy = 5;
    public Vector3 PositionToThrow = new Vector3(0, 1, 0.8f);
    public Vector3 DirectionToThrow = Vector3.forward;

    [HideInInspector] public bool IsThrowed = false;
    public override void UseItem()
    {
        if (ItemQuantity <= 0 || CanUseItem == false || IsThrowed == true) return;

        ThrowThis(ThrowForce, ThrowUpForce, PositionToThrow, DirectionToThrow, RotationForce);


        base.UseItem();

    }
    public virtual GameObject ThrowThis(float forceToThrow, float ThrowUpForce, Vector3 positionToThrow, Vector3 directionToThrow, float angularForce = 0)
    {
        RemoveItem();
        Vector3 throwPosition = Owner.transform.TransformPoint(positionToThrow);
        Debug.Log("throwPosition= " + throwPosition);

        Vector3 throwDirection = Owner.transform.rotation * directionToThrow;
        Debug.Log("throwDirection= " + throwDirection);

        Vector3 throwedScale = transform.lossyScale;
        Debug.Log("throwDirection= " + throwDirection);

        GameObject throwedGameobject = Instantiate(gameObject, throwPosition, transform.rotation);
        throwedGameobject.transform.localScale = throwedScale;

        throwedGameobject.GetComponent<ThrowableItem>().IsThrowed = true;

        if (SecondsToDestroy > 0) Destroy(throwedGameobject, SecondsToDestroy);

        if (throwedGameobject.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.AddForce(throwDirection * forceToThrow, ForceMode.Impulse);
            rb.AddForce(((Owner != null) ? Owner.transform.up : Vector3.up) * ThrowUpForce, ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(-angularForce, angularForce), Random.Range(-angularForce, angularForce), Random.Range(-angularForce, angularForce)), ForceMode.Impulse);
        }

        if (throwedGameobject.TryGetComponent(out Collider col))
        {
            col.enabled = true;
            col.isTrigger = false;
        }

        return throwedGameobject;
    }


    private void OnDrawGizmos()
    {
        if (Owner == null) { RefreshItemDependencies(); return; }

        Vector3 throwPosition = Owner.transform.TransformPoint(PositionToThrow);
        Vector3 throwDirection = Owner.transform.rotation * DirectionToThrow;

        Gizmos.DrawSphere(throwPosition, 0.05f);
        Gizmos.DrawRay(throwPosition, throwDirection);
    }
}


