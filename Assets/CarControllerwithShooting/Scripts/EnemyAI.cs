using UnityEngine;

namespace CarControllerwithShooting
{
    public class EnemyAI : MonoBehaviour
    {
        public int Health = 100;
        public float Range = 200;
        public float Firing_Interval = 5;
        //private float LastFiring_Time = 0;
        public GameObject EnemyMissile;
        public Transform Firing_Point;
        public Transform MissileLauncher;
        private bool isExploded = false;
        public GameObject SpritePointer;
        public Collider MainCollider;

        public Collider[] colliders;

        [Tooltip("Lower is Better for Better Accuracy!")]
        [Range(1, 10)]
        public float Accuracy = 5;

        void Update()
        {
            //if (CarController.Instance != null && Health > 0 && Vector3.Distance(CarController.Instance.transform.position, transform.position) < Range)
            //{
            //    FireProcess();
            //    FollowProcess();
            //}
        }

        private void FollowProcess()
        {
            //var rotation = Quaternion.LookRotation(CarController.Instance.transform.position - MissileLauncher.position);
            //MissileLauncher.rotation = Quaternion.Slerp(MissileLauncher.rotation, rotation, Time.deltaTime * 1);
        }

        void FireProcess()
        {
            //if (Time.time > LastFiring_Time + Firing_Interval && Vector3.Distance(transform.position, CarController.Instance.transform.position) > 30)
            //{
            //    // Helicopter is in Range!
            //    LastFiring_Time = Time.time;
            //    GameObject enemyMissile = Instantiate(EnemyMissile, Firing_Point.position, Quaternion.identity);
            //    Vector3 targettoShoot = new Vector3(CarController.Instance.transform.position.x + Random.Range(-1 * (10 - Accuracy), (10 - Accuracy)), CarController.Instance.transform.position.y + 3 + Random.Range(-1 * (10 - Accuracy), (10 - Accuracy)), CarController.Instance.transform.position.z + Random.Range(-1 * (10 - Accuracy), (10 - Accuracy)));
            //    enemyMissile.transform.LookAt(targettoShoot);
            //    enemyMissile.GetComponentInChildren<Rigidbody>().AddForce(enemyMissile.transform.forward * 120, ForceMode.Impulse);
            //}
        }

        public void GetDamage(int Damage)
        {
            Health = Health - Damage;
            if (Health <= 0 && !isExploded)
            {
                isExploded = true;
                SpritePointer.SetActive(false);
                MainCollider.isTrigger = true;
                // Let's Explode
                foreach (var item in colliders)
                {
                    item.enabled = true;
                }


                foreach (var rigidbody in GetComponentsInChildren<Rigidbody>())
                {
                    rigidbody.useGravity = true;
                    rigidbody.isKinematic = false;
                    rigidbody.AddExplosionForce(Random.Range(5, 20), rigidbody.transform.position, Random.Range(5, 15), Random.Range(2, 4), ForceMode.Impulse);
                    rigidbody.AddRelativeTorque(new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5)), ForceMode.Impulse);
                    Destroy(rigidbody.gameObject, 5);
                }

                if (RadarSystem.Instance != null)
                    RadarSystem.Instance.RemoveTarget(gameObject);
                // Let's Destroy itself
                Destroy(gameObject, 6);
            }
        }
    }
}
