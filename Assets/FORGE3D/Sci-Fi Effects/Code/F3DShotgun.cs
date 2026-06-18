using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FORGE3D
{
    public class F3DShotgun : MonoBehaviour
    {
        private readonly List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();
        private ParticleSystem _ps;

        internal F3DFXController f3DFXController;

        private void Start()
        {
            _ps = GetComponent<ParticleSystem>();
        }

        // On particle collision
        private void OnParticleCollision(GameObject other)
        {
            var numCollisionEvents = _ps.GetCollisionEvents(other, _collisionEvents);

            // Play collision sound and apply force to the rigidbody was hit
            for (var j = 0; j < numCollisionEvents; j++)
            {
                F3DAudioController.instance.ShotGunHit(_collisionEvents[j].intersection);

                var rb = other.GetComponent<Rigidbody>();
                if (!rb) continue;
                var pos = _collisionEvents[j].intersection;
                var force = _collisionEvents[j].velocity.normalized * 50f;

                rb.AddForceAtPosition(force, pos);
            }

            ColliderWithObject(other);


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

            if (other.gameObject.CompareTag("OilBarrel")/*&&f3DFXController && f3DFXController.playerid*/)
            {
                if (f3DFXController && f3DFXController.playerid)
                {
                    Debug.Log("Other Gameobject Name::" + other.gameObject.name);
                    other.gameObject.GetComponent<OilBarrel>().Blast(f3DFXController.playerid.playerID);
                }
                else
                {
                    Debug.Log("f3DFXController::>>" + (f3DFXController == null) + ":::::::" + (f3DFXController.playerid == null));
                }

            }
        }
    }
}