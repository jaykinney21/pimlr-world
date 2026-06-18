using UnityEngine;
using System.Collections;

namespace FORGE3D
{
    public class F3DFlameThrower : MonoBehaviour
    {
        public Light pLight; // Attached point light
        public ParticleSystem heat; // Heat particles

        int lightState; // Point light state flag (fading in or out)
        bool despawn; // Despawn state flag

        ParticleSystem ps;

        internal F3DFXController f3DFXController;
        void Start()
        {
            ps = GetComponent<ParticleSystem>();
        }

        // OnSpawned called by pool manager 
        void OnSpawned()
        {
            despawn = false;

            lightState = 1;
            pLight.intensity = 0f;
        }

        // OnDespawned called by pool manager 
        void OnDespawned()
        {
        }

        // Despawn game object
        void OnDespawn()
        {
            F3DPoolManager.Pools["GeneratedPool"].Despawn(transform);
        }

        private void OnDisable()
        {
            if (!despawn)
            {
                // Set despawn flag and add despawn timer allowing particles fading
                despawn = true;
                // F3DTime.time.AddTimer(1f, 1, OnDespawn);
                OnDespawn();
                // Stop the particle systems
                ps?.Stop();
                if (heat)
                    heat.Stop();

                // Toggle light state
                pLight.intensity = 0.6f;
                lightState = -1;
            }
        }

        public void StopFire()
        {
            if (!despawn)
            {
                // Set despawn flag and add despawn timer allowing particles fading
                despawn = true;
                F3DTime.time.AddTimer(1f, 1, OnDespawn);

                // Stop the particle systems
                ps.Stop();
                if (heat)
                    heat.Stop();

                // Toggle light state
                pLight.intensity = 0.6f;
                lightState = -1;
            }
        }

        void Update()
        {
            // Despawn on mouse
            if (Input.GetMouseButtonUp(0) && f3DFXController &&  f3DFXController.playerid.isLocalPlayer)
            {
                if (!despawn)
                {
                    // Set despawn flag and add despawn timer allowing particles fading
                    despawn = true;
                    F3DTime.time.AddTimer(1f, 1, OnDespawn);

                    // Stop the particle systems
                    ps.Stop();
                    if (heat)
                        heat.Stop();

                    // Toggle light state
                    pLight.intensity = 0.6f;
                    lightState = -1;
                }
            }

            // Fade in point light
            if (lightState == 1)
            {
                pLight.intensity = Mathf.Lerp(pLight.intensity, 0.7f, Time.deltaTime * 10f);

                if (pLight.intensity >= 0.5f)
                    lightState = 0;
            }
            // Fade out point light
            else if (lightState == -1)
            {
                pLight.intensity = Mathf.Lerp(pLight.intensity, -0.1f, Time.deltaTime * 10f);

                if (pLight.intensity <= 0f)
                    lightState = 0;
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            //Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$" + other.gameObject.tag);
            if (other.CompareTag("Enemy"))
            {
                PlayerID playerIDComponent = other.GetComponent<PlayerID>();

                //   Debug.Log(" ENEMY::  Player ID: " + playerIDComponent.helixPlayerInfo.userName + "::::" + SocketPlayerManager.Instance.allPlayers[shooterID].helixPlayerInfo.userName);
                if (playerIDComponent != null && playerIDComponent.playerID != SocketNetworkManager.Instance.playerID)
                {

                    if (f3DFXController &&(playerIDComponent.helixPlayerInfo.teamA &&  !f3DFXController.playerid.helixPlayerInfo.teamA) || (!playerIDComponent.helixPlayerInfo.teamA && f3DFXController.playerid.helixPlayerInfo.teamA) || (playerIDComponent.helixPlayerInfo.teamB && !f3DFXController.playerid.helixPlayerInfo.teamB) || (!playerIDComponent.helixPlayerInfo.teamB && f3DFXController.playerid.helixPlayerInfo.teamB))
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

                    if (f3DFXController &&(playerIDComponent.helixPlayerInfo.teamA && !f3DFXController.playerid.helixPlayerInfo.teamA) || (!playerIDComponent.helixPlayerInfo.teamA && f3DFXController.playerid.helixPlayerInfo.teamA) || (playerIDComponent.helixPlayerInfo.teamB && !f3DFXController.playerid.helixPlayerInfo.teamB) || (!playerIDComponent.helixPlayerInfo.teamB && f3DFXController.playerid.helixPlayerInfo.teamB))
                        SocketNetworkManager.Instance.EmitHealth(f3DFXController.playerid.playerID, playerIDComponent.playerID, f3DFXController.DamagePower.ToString());
                    else
                    {
                        //Debug.Log(  playerIDComponent.helixPlayerInfo.userName +":::Get damage:::::>>>" + playerIDComponent.helixPlayerInfo.teamA + "::::::" + teamA + "BBBBB" + playerIDComponent.helixPlayerInfo.teamB + "::::::" + teamB);
                    }
                }
            }
        }



    }
}