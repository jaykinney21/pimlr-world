using System;
using UnityEngine;
using System.Collections;

namespace FORGE3D
{
    public class F3DPlayerTurretController : MonoBehaviour
    {
        RaycastHit hitInfo; // Raycast structure
        public F3DTurret turret;
       internal bool isFiring; // Is turret currently in firing state
        public F3DFXController fxController;

        void Update()
        {
            if (this.gameObject.activeSelf)
                CheckForFire();
        }

        private void LateUpdate()
        {
            CheckForTurn();
        }

        void CheckForFire()
        {
            // Fire turret
            if (!isFiring && Input.GetKey(KeyCode.Mouse0) && fxController.playerid && fxController.playerid.isLocalPlayer && fxController.playerid.gunController.playerid.controller.playeInputsActivate)
            {
                isFiring = true;

                fxController.Fire(true);
            }

            // Stop firing
            if (isFiring && Input.GetKeyUp(KeyCode.Mouse0) && fxController.playerid && fxController.playerid.isLocalPlayer)
            {
                SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, -1, fxController.playerid.helixPlayerInfo.userID, "");
                isFiring = false;
                fxController.Stop();
            }
        }


        public void StopAIFiring()
        {
            if (isFiring  && fxController.playerid && fxController.playerid.isAIPlayer && SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)
            {
                SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, -1, fxController.playerid.helixPlayerInfo.userID, "");
                isFiring = false;
                fxController.Stop();
            }
        }
        void CheckForTurn()
        {
            // Construct a ray pointing from screen mouse position into world space
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast
            if (Physics.Raycast(cameraRay, out hitInfo, 500f) && fxController.playerid && fxController.playerid.isLocalPlayer)
            {
                turret.SetNewTarget(hitInfo.point);
            }
        }
    }
}