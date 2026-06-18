using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClone : MonoBehaviour
{
    public GameObject explosionPrefab;
    public int DamagePower = 5;
    public Rigidbody rb;


    /*IEnumerator Start()
    {
        //rb = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }*/

    /*private void OnTriggerEnter(Collider other)
    {
        //GameObject muzzle = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        //muzzle.transform.eulerAngles = new Vector3(Random.Range(0, -180), 0, 0);

        if (other.CompareTag("Enemy"))
        {
            //Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
            //if (rb != null)
            //{
            //    rb.isKinematic = false;
            //    rb.useGravity = true;
            //    rb.AddExplosionForce(10, transform.position, 1, 1f);
            //    Destroy(collision.collider.gameObject, 10);
            //}

            Debug.Log("inside of the colllision enter ");
        }
        //else if (collision.collider.CompareTag("Enemy"))
        //{
        //    collision.collider.GetComponent<EnemyAI>().GetDamage(DamagePower);
        //}
        //else if (collision.collider.CompareTag("Natural"))
        //{
        //    collision.collider.GetComponent<NaturalAI>().GetDamage(DamagePower);
        //}
        Debug.Log("destroy" + other.gameObject.name);
       // Destroy(gameObject);
        // }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if ((collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Natural") || collision.collider.CompareTag("Collapsable")))
        // {
      
    }*/
}
