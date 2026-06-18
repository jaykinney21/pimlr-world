using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBarrel : MonoBehaviour
{
    [SerializeField] ParticleSystem blastPartical;
    [SerializeField] MeshRenderer barrelMesh;
    [SerializeField] MeshCollider barrelCollider;

    internal string shotterId;

    private void OnEnable()
    {
        barrelMesh.enabled = true;
        barrelCollider.enabled = true;
    }
    private void OnCollisionEnter(Collision other)
    {

        //Debug.Log("Collision Blast Barrel::>" + other.collider.tag);
        //if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Missile"))
        //{
        //    blastPartical.Play();
        //    barrelMesh.enabled = false;
        //    barrelCollider.enabled = false;
        //    Debug.Log("Collision Blast Barrel");
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision Blast Barrel::>" + other.tag);
        //if (other.CompareTag("Bullet") || other.CompareTag("Missile"))
        //{
        //    blastPartical.Play();
        //    barrelMesh.enabled = false;
        //    barrelCollider.enabled = false;
        //    Debug.Log("Trigger Blast Barrel");
        //}
    }

    public void Blast(string shooter)
    {
        blastPartical.Play();
        barrelMesh.enabled = false;
        barrelCollider.enabled = false;
        shotterId = shooter;
        Debug.Log("Collision Blast Barrel");
    }
}
