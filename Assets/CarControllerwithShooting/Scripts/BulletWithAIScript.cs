using System.Collections;
using UnityEngine;

namespace CarControllerwithShooting
{
    public class BulletWithAIScript : MonoBehaviour
    {
        public GameObject explosionPrefab;
        public int DamagePower = 5;
        public Rigidbody rb;
        
        
        IEnumerator Start()
        {
            //rb = GetComponent<Rigidbody>();
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
        //else if (collision.collider.CompareTag("Enemy"))
        //{
        //    collision.collider.GetComponent<EnemyAI>().GetDamage(DamagePower);
        //}
        //else if (collision.collider.CompareTag("Natural"))
        //{
        //    collision.collider.GetComponent<NaturalAI>().GetDamage(DamagePower);
        //}
        //Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        //if (rb != null)
        //{
        //    rb.isKinematic = false;
        //    rb.useGravity = true;
        //    rb.AddExplosionForce(10, transform.position, 1, 1f);
        //    Destroy(collision.collider.gameObject, 10);
        //}

        private void OnCollisionEnter(Collision collision)
        {
           // if ((collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Natural") || collision.collider.CompareTag("Collapsable")))
           // {
                GameObject muzzle = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                muzzle.transform.eulerAngles = new Vector3(Random.Range(0, -180), 0, 0);
            //Debug.Log("destroy" + collision.gameObject.name);
            //if (collision.gameObject.tag == "ignoreSensor")
            //    {
            //        AiCarContrtoller aiCarComponent = collision.gameObject.GetComponent<AiCarContrtoller>();

            //        aiCarComponent.Health -= 10f;

            //        //Debug.Log("inside of the colllision enter ");
            //        Debug.Log("this is " + collision.gameObject.name + " has now a health of " + aiCarComponent.Health);

            //    }

            Destroy(gameObject);
           // }
        }
    }
}
