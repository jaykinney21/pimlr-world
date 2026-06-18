using UnityEngine;
using System.Collections;

namespace FORGE3D
{
    public class F3DMissile : MonoBehaviour
    {
        public enum MissileType
        {
            Unguided,
            Guided,
            Predictive
        }

        public MissileType missileType;
        public PlayerID target;
        public LayerMask layerMask;

        public float detonationDistance;

        public float lifeTime = 5f; // Missile life time
        public float despawnDelay; // Delay despawn in ms
        public float velocity = 300f; // Missile velocity
        public float alignSpeed = 1f;
        public float RaycastAdvance = 2f; // Raycast advance multiplier

        public bool DelayDespawn = false; // Missile despawn flag

        public ParticleSystem[] delayedParticles; // Array of delayed particles
        private ParticleSystem[] particles; // Array of Missile particles

        private new Transform transform; // Cached transform

        private bool isHit = false; // Missile hit flag
        private bool isFXSpawned = false; // Hit FX prefab spawned flag

        private float timer = 0f; // Missile timer

        private Vector3 targetLastPos;
        private Vector3 step;

        private MeshRenderer meshRenderer;
        public F3DMissileLauncher launcher;

        [SerializeField] internal PlayerID playerID;

        int DamagePower = 50;

        private void Awake()
        {
            // Cache transform and get all particle systems attached
            transform = GetComponent<Transform>();
            particles = GetComponentsInChildren<ParticleSystem>();
            meshRenderer = GetComponent<MeshRenderer>();
        }


        PlayerID tempPlayer = null;
        private void OnTriggerEnter(Collider other)
        {
            if (playerID && target == null)
            {
                //Debug.Log("::::>" + other.transform.name);
                if (tempPlayer == null)
                {
                    other.TryGetComponent<PlayerID>(out tempPlayer);
                    if (tempPlayer != null && playerID)
                    {
                        //Debug.Log("tempPlayer::::::>>>" + tempPlayer.name + "::::" + tempPlayer.helixPlayerInfo.teamA + ":::B::::" + tempPlayer.helixPlayerInfo.teamB);
                        //Debug.Log("playerID::::::>>>" + playerID.name + "::::" + playerID.helixPlayerInfo.teamA + ":::B::::" + playerID.helixPlayerInfo.teamB);
                        //Debug.Log("--------------------------------------------------------------------------------------------------------------------------------------------");
                        if ((tempPlayer.helixPlayerInfo.teamA && !playerID.helixPlayerInfo.teamA) || (!tempPlayer.helixPlayerInfo.teamA && playerID.helixPlayerInfo.teamA) || (tempPlayer.helixPlayerInfo.teamB && !playerID.helixPlayerInfo.teamB) || (!tempPlayer.helixPlayerInfo.teamB && playerID.helixPlayerInfo.teamB))
                        {
                            target = tempPlayer;
                        }
                        else
                        {
                            tempPlayer = null;
                        }
                    }


                }
            }




        }
        private void OnEnable()
        {
            target = null;

        }



        // OnSpawned called by pool manager 
        public void OnSpawned()
        {
            isHit = false;
            isFXSpawned = false;
            timer = 0f;
            targetLastPos = Vector3.zero;
            step = Vector3.zero;
            meshRenderer.enabled = true;

        }

        // OnDespawned called by pool manager 
        public void OnDespawned()
        {
        }

        // Stop attached particle systems emission and allow them to fade out before despawning
        private void Delay()
        {
            if (particles.Length > 0 && delayedParticles.Length > 0)
            {
                bool delayed;

                for (var i = 0; i < particles.Length; i++)
                {
                    delayed = false;

                    for (var y = 0; y < delayedParticles.Length; y++)
                        if (particles[i] == delayedParticles[y])
                        {
                            delayed = true;
                            break;
                        }

                    particles[i].Stop(false);

                    if (!delayed)
                        particles[i].Clear(false);
                }
            }
        }

        // Despawn routine
        private void OnMissileDestroy()
        {
            meshRenderer.enabled = false;
            F3DPoolManager.Pools["GeneratedPool"].Despawn(transform);
        }

        private void OnHit(PlayerID targetEnemy)
        {
            meshRenderer.enabled = false;
            isHit = true;

            // Invoke delay routine if required
            if (DelayDespawn)
            {
                // Reset missile timer and let particles systems stop emitting and fade out correctly
                timer = 0f;
                Delay();
            }

            if (playerID && playerID.isLocalPlayer || (playerID.isAIPlayer && SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser))
            {
                if (targetEnemy)
                    targetEnemy.helixPlayerHealth.DoDamage(50, Vector3.one, playerID.playerID);
            }
        }
        RaycastHit hitPoint;
        private void Update()
        {
            // If something was hit
            if (isHit)
            {
                // Execute once
                if (!isFXSpawned)
                {
                    // Put your calls to effect manager that spawns explosion on hit
                    // .....

                    launcher.SpawnExplosion(transform.position);

                    isFXSpawned = true;
                }

                // Despawn current missile 
                if (!DelayDespawn || (DelayDespawn && (timer >= despawnDelay)))
                    OnMissileDestroy();
            }




            // No collision occurred yet
            else
            {
                // Projectile step per frame based on velocity and time
                Vector3 step = transform.forward * Time.deltaTime * velocity;

                //Ray collidObjct;
                // Raycast for targets with ray length based on frame step by ray cast advance multiplier
                if (Physics.Raycast(transform.position, transform.forward, out hitPoint,
                    step.magnitude * RaycastAdvance,
                    layerMask))
                {
                    isHit = true;

                    if (hitPoint.collider != null)
                    {
                        ColliderWithObject(hitPoint.collider.gameObject);
                    }





                    // Invoke delay routine if required
                    if (DelayDespawn)
                    {
                        // Reset projectile timer and let particles systems stop emitting and fade out correctly
                        timer = 0f;
                        Delay();
                    }
                }
                // Nothing hit
                else
                {
                    // Projectile despawn after run out of time
                    if (timer >= lifeTime)
                        OnMissileDestroy();
                }

                // Advances projectile forward
                transform.position += step;


                // Updates projectile timer

                // No collision occurred yet

                // Navigate
                if (target != null)
                {
                    if (missileType == MissileType.Predictive)
                    {
                        var hitPos = F3DPredictTrajectory.Predict(transform.position, target.transform.position, targetLastPos,
                            velocity);
                        targetLastPos = target.transform.position;

                        transform.rotation = Quaternion.Lerp(transform.rotation,
                            Quaternion.LookRotation(hitPos - transform.position), Time.deltaTime * alignSpeed);
                    }
                    else if (missileType == MissileType.Guided)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation,
                            Quaternion.LookRotation(target.transform.position - transform.position), Time.deltaTime * alignSpeed);
                    }
                }

                // Missile step per frame based on velocity and time
                step = transform.forward * Time.deltaTime * velocity;

                if (target != null && missileType != MissileType.Unguided &&
                    Vector3.SqrMagnitude(transform.position - target.transform.position) <= detonationDistance)
                {
                    OnHit(target);
                }
                else if (missileType == MissileType.Unguided &&
                         Physics.Raycast(transform.position, transform.forward, step.magnitude * RaycastAdvance, layerMask))
                {
                    OnHit(target);
                }
                // Nothing hit
                else
                {
                    // Despawn missile at the end of life cycle
                    if (timer >= lifeTime)
                    {
                        // Do not detonate
                        isFXSpawned = true;
                        OnHit(target);
                    }
                }

                // Advances missile forward
                transform.position += step;
            }

            // Updates missile timer
            timer += Time.deltaTime;
        }
        public void ColliderWithObject(GameObject other)
        {
            //Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$" + other.gameObject.tag);
            if (other.CompareTag("Enemy"))
            {
                PlayerID playerIDComponent = other.GetComponent<PlayerID>();

                //   Debug.Log(" ENEMY::  Player ID: " + playerIDComponent.helixPlayerInfo.userName + "::::" + SocketPlayerManager.Instance.allPlayers[shooterID].helixPlayerInfo.userName);
                if (playerIDComponent != null && playerIDComponent.playerID != SocketNetworkManager.Instance.playerID)
                {

                    if ((playerIDComponent.helixPlayerInfo.teamA && !playerID.helixPlayerInfo.teamA) || (!playerIDComponent.helixPlayerInfo.teamA && playerID.helixPlayerInfo.teamA) || (playerIDComponent.helixPlayerInfo.teamB && !playerID.helixPlayerInfo.teamB) || (!playerIDComponent.helixPlayerInfo.teamB && playerID.helixPlayerInfo.teamB))
                        SocketNetworkManager.Instance.EmitHealth(playerID.playerID, playerIDComponent.playerID, DamagePower.ToString());
                    else
                    {
                        //Debug.Log(playerIDComponent.helixPlayerInfo.userName +"::Get damage:::::>>>" + playerIDComponent.helixPlayerInfo.teamA + "::::::" + teamA + "BBBBB" + playerIDComponent.helixPlayerInfo.teamB + "::::::" + teamB);
                    }
                }
            }

            else if (other.CompareTag("Player"))
            {
                PlayerID playerIDComponent = other.GetComponent<PlayerID>();
                //  Debug.Log("PLAYER::  Player ID: " + playerIDComponent.helixPlayerInfo.userName + "::::" + SocketPlayerManager.Instance.allPlayers[shooterID].helixPlayerInfo.userName);
                if (playerIDComponent != null /*&& playerIDComponent.playerID != SocketNetworkManager.Instance.playerID*/)
                {

                    if ((playerIDComponent.helixPlayerInfo.teamA && !playerID.helixPlayerInfo.teamA) || (!playerIDComponent.helixPlayerInfo.teamA && playerID.helixPlayerInfo.teamA) || (playerIDComponent.helixPlayerInfo.teamB && !playerID.helixPlayerInfo.teamB) || (!playerIDComponent.helixPlayerInfo.teamB && playerID.helixPlayerInfo.teamB))
                        SocketNetworkManager.Instance.EmitHealth(playerID.playerID, playerIDComponent.playerID, DamagePower.ToString());
                    else
                    {
                        //Debug.Log(  playerIDComponent.helixPlayerInfo.userName +":::Get damage:::::>>>" + playerIDComponent.helixPlayerInfo.teamA + "::::::" + teamA + "BBBBB" + playerIDComponent.helixPlayerInfo.teamB + "::::::" + teamB);
                    }
                }
            }

            if (other.gameObject.CompareTag("OilBarrel"))
            {
                if (playerID)
                    other.gameObject.GetComponent<OilBarrel>().Blast(playerID.playerID);
            }

        }
    }

}
