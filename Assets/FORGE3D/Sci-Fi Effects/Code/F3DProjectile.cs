using UnityEngine;
using System.Collections;

namespace FORGE3D
{
    public class F3DProjectile : MonoBehaviour
    {
        public F3DFXType fxType; // Weapon type 
        public LayerMask layerMask;
        public float lifeTime = 5f; // Projectile life time
        public float despawnDelay; // Delay despawn in ms
        public float velocity = 300f; // Projectile velocity
        public float RaycastAdvance = 2f; // Raycast advance multiplier 
        public bool DelayDespawn = false; // Projectile despawn flag 
        public ParticleSystem[] delayedParticles; // Array of delayed particles
        ParticleSystem[] particles; // Array of projectile particles 
        new Transform transform; // Cached transform 
        RaycastHit hitPoint; // Raycast structure 
        bool isHit = false; // Projectile hit flag
        bool isFXSpawned = false; // Hit FX prefab spawned flag 
        float timer = 0f; // Projectile timer
        float fxOffset; // Offset of fxImpact




        internal F3DFXController f3DFXController;

        void Awake()
        {
            // Cache transform and get all particle systems attached
            transform = GetComponent<Transform>();
            particles = GetComponentsInChildren<ParticleSystem>();
        }

        // OnSpawned called by pool manager 
        public void OnSpawned()
        {
            // Reset flags and raycast structure
            isHit = false;
            isFXSpawned = false;
            timer = 0f;
            hitPoint = new RaycastHit();
        }

        // OnDespawned called by pool manager 
        public void OnDespawned()
        {
        }

        // Stop attached particle systems emission and allow them to fade out before despawning
        void Delay()
        {
            if (particles.Length > 0 && delayedParticles.Length > 0)
            {
                bool delayed;
                for (int i = 0; i < particles.Length; i++)
                {
                    delayed = false;
                    for (int y = 0; y < delayedParticles.Length; y++)
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

        // OnDespawned called by pool manager 
        void OnProjectileDestroy()
        {
            F3DPoolManager.Pools["GeneratedPool"].Despawn(transform);
        }

        // Apply hit force on impact
        void ApplyForce(float force)
        {
            //if (hitPoint.rigidbody != null)
            //    hitPoint.rigidbody.AddForceAtPosition(transform.forward * force, hitPoint.point,
            //        ForceMode.VelocityChange);
        }

        void Update()
        {
            // If something was hit
            if (isHit)
            {
                // Execute once
                if (!isFXSpawned)
                {
                    // Invoke corresponding method that spawns FX
                    switch (fxType)
                    {
                        case F3DFXType.Vulcan:
                            f3DFXController.VulcanImpact(hitPoint.point + hitPoint.normal * fxOffset);
                            ApplyForce(2.5f);
                            break;

                        case F3DFXType.SoloGun:
                            f3DFXController.SoloGunImpact(hitPoint.point + hitPoint.normal * fxOffset);
                            ApplyForce(25f);
                            break;

                        case F3DFXType.Seeker:
                            f3DFXController.SeekerImpact(hitPoint.point + hitPoint.normal * fxOffset);
                            ApplyForce(30f);
                            break;

                        case F3DFXType.PlasmaGun:
                            f3DFXController.PlasmaGunImpact(hitPoint.point + hitPoint.normal * fxOffset);
                            ApplyForce(25f);
                            break;

                        case F3DFXType.LaserImpulse:
                            f3DFXController.LaserImpulseImpact(hitPoint.point + hitPoint.normal * fxOffset);
                            ApplyForce(25f);
                            break;
                    }

                    isFXSpawned = true;
                }

                // Despawn current projectile 
                if (!DelayDespawn || (DelayDespawn && (timer >= despawnDelay)))
                    OnProjectileDestroy();
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
                        OnProjectileDestroy();
                }

                // Advances projectile forward
                transform.position += step;
            }

            // Updates projectile timer
            timer += Time.deltaTime;
        }

        //Set offset
        public void SetOffset(float offset)
        {
            fxOffset = offset;
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

                    if ((playerIDComponent.helixPlayerInfo.teamA && !f3DFXController.playerid.helixPlayerInfo.teamA) || (!playerIDComponent.helixPlayerInfo.teamA && f3DFXController.playerid.helixPlayerInfo.teamA) || (playerIDComponent.helixPlayerInfo.teamB && !f3DFXController.playerid.helixPlayerInfo.teamB) || (!playerIDComponent.helixPlayerInfo.teamB && f3DFXController.playerid.helixPlayerInfo.teamB))
                        SocketNetworkManager.Instance.EmitHealth(f3DFXController.playerid.playerID, playerIDComponent.playerID, f3DFXController.DamagePower.ToString());
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

                    if ((playerIDComponent.helixPlayerInfo.teamA && !f3DFXController.playerid.helixPlayerInfo.teamA) || (!playerIDComponent.helixPlayerInfo.teamA && f3DFXController.playerid.helixPlayerInfo.teamA) || (playerIDComponent.helixPlayerInfo.teamB && !f3DFXController.playerid.helixPlayerInfo.teamB) || (!playerIDComponent.helixPlayerInfo.teamB && f3DFXController.playerid.helixPlayerInfo.teamB))
                        SocketNetworkManager.Instance.EmitHealth(f3DFXController.playerid.playerID, playerIDComponent.playerID, f3DFXController.DamagePower.ToString());
                    else
                    {
                        //Debug.Log(  playerIDComponent.helixPlayerInfo.userName +":::Get damage:::::>>>" + playerIDComponent.helixPlayerInfo.teamA + "::::::" + teamA + "BBBBB" + playerIDComponent.helixPlayerInfo.teamB + "::::::" + teamB);
                    }
                }
            }

            if (other.gameObject.CompareTag("OilBarrel"))
            {
                if (f3DFXController && f3DFXController.playerid)
                    other.gameObject.GetComponent<OilBarrel>().Blast(f3DFXController.playerid.playerID);
            }

        }
    }
}