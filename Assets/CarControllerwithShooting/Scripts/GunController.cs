using FORGE3D;
using System.Collections;
using UnityEngine;


namespace CarControllerwithShooting
{
    public class GunController : MonoBehaviour
    {


        [Header("PlayerInput")]
        public PlayerInput playerInput;

        [Header("Machine Gun Properties")]

        public Transform FiringPoint_Machinegun;
        //private float LastTime_Machinegun_Fire = 0;
        public float Machinegun_Firing_Interval;
        public AudioClip AudioClip_Machinegun_Fire;
        public Animation Anim_MachineGun;

        [Header("Missile Properties")]
        public Transform[] FiringPoints_Missiles;
        public float Missile_Firing_Interval;
        private int Missile_Firing_Point_Index = 0;
        public AudioClip AudioClip_Missile_Fire;
        public ParticleSystem[] Particle_Missile_Firing_Explosion;

        [Header("Ammo Capacity")]
        public int Ammo_Machinegun = 10000;
        public int Ammo_Missile = 10000;

        public AudioSource AudioSource_Gun;



        public F3DFXController Weapon_MachineGun;
        public GameObject Weapon_MissileLeft;
        public GameObject Weapon_MissileRight;
        public PlayerID playerid;
        public PlayerID currentEnemyDetected;
        public float Range = 80;



        private void OnEnable()
        {
            GameplayManager.OnMissileDestroyed += ResetCameraToCar;

            Weapon_MachineGun.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            currentEnemyDetected = null;
            ResetCameraToCar();
            GameplayManager.OnMissileDestroyed -= ResetCameraToCar;
        }

        public void DeactivateWeapons()
        {
            Weapon_MachineGun.gameObject.SetActive(false);
            Weapon_MissileLeft.SetActive(false);
            Weapon_MissileRight.SetActive(false);
            this.enabled = false;
        }

        [System.Obsolete]
        IEnumerator FireMachingun()
        {
            while (fire && Weapon_MachineGun.gameObject.activeSelf)
            {
                yield return new WaitForSeconds(.2f);
                SocketNetworkManager.Instance.EmitFireUpdate("NormalBullet", FiringPoint_Machinegun, 15, playerid.helixPlayerInfo.userID, "");
                GenBullate(FiringPoint_Machinegun.position, FiringPoint_Machinegun.rotation.eulerAngles);

            }
            fire = false;
        }

        public GunController setplayerID(PlayerID playerid)
        {
            this.playerid = playerid;
            return this;
        }

        internal bool fire = false;

        [System.Obsolete]
        private void Update()
        {

            //if (playerid == null)
            //{
            //    //Debug.Log(":::" + this.gameObject.name);
            //}
            if (playerid.isLocalPlayer)
            {
                //if (Weapon_MachineGun.activeSelf)
                //    FollowProcess();

                if (CarSystemManager.Instance.controllerType == ControllerType.KeyboardMouse)
                {
                    if (fire == false && Input.GetMouseButton(0) && playerid.controller.playeInputsActivate && Weapon_MachineGun.gameObject.activeSelf)
                    {
                        fire = true;
                        StartCoroutine(FireMachingun());
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        fire = false;
                        StopCoroutine(FireMachingun());
                    }

                    if (Input.GetMouseButtonDown(1) && playerid.controller.playeInputsActivate && playerid.controller.currentSecondaryPowerUp != null && playerid.controller.currentSecondaryPowerUp.isUsed == false && playerid.controller.currentSecondaryPowerUp.missilePowerUp)
                    {
                        //Debug.Log("<color=Red>(01) ||||||||||||||||||  </color>");
                        playerid.controller.currentSecondaryPowerUp.isUsed = true;
                        playerid.controller.playeInputsActivate = false;
                        Missile_Firing_Point_Index++;
                        var t = FiringPoints_Missiles[Missile_Firing_Point_Index % 2];
                        string randomString = SocketNetworkManager.RandomString(8);
                        SocketNetworkManager.Instance.EmitFireUpdate("missile", t, Missile_Firing_Point_Index, playerid.helixPlayerInfo.userID, randomString);
                        MissailBullate(t.position, t.rotation.eulerAngles, Missile_Firing_Point_Index, randomString);
                    }
                    else if (Input.GetMouseButtonDown(1) && playerid.controller.playeInputsActivate && playerid.controller.currentSecondaryPowerUp != null && playerid.controller.currentSecondaryPowerUp.isUsed == false && !Weapon_MissileLeft.gameObject.activeSelf)
                    {
                        playerid.controller.currentSecondaryPowerUp.OnStartUsePowerUp(playerid.playerID, true);
                    }

                    if (Input.GetMouseButtonDown(1) && playerid.controller.currentSecondaryPowerUp == null)
                    {
                        Missile_Firing_Point_Index++;
                        var t = FiringPoints_Missiles[Missile_Firing_Point_Index % 2];
                        string randomString = SocketNetworkManager.RandomString(8);
                        SocketNetworkManager.Instance.EmitFireUpdate("NormalMissile", t, Missile_Firing_Point_Index % 2, playerid.helixPlayerInfo.userID, randomString);
                        playerid.f3DMissileLauncher.FireMissile(Missile_Firing_Point_Index % 2);
                        //MissailBullate(t.position, t.rotation.eulerAngles, Missile_Firing_Point_Index, randomString,true);
                    }
                }
                if (currentEnemyDetected != null)
                {
                    if (Vector3.Distance(currentEnemyDetected.transform.position, transform.position) < Range == false || currentEnemyDetected.gameObject.activeSelf == false)
                    {
                        Debug.Log("***************************************************************************************************::::OUT OF RANGE$$$$$$$$$$$$$$$$$$$$$$$$");
                        currentEnemyDetected = null;
                        //playerid.currentGunController.f3DPlayerTurretController.turret.DebugTarget = null;

                        if (playerid.currentGunController)
                        {
                            playerid.currentGunController.f3DPlayerTurretController.turret.headTransform.localRotation = Quaternion.identity;
                            SocketNetworkManager.Instance.EmitMachineGunRot(playerid.playerID, Vector3.zero, playerid.currentGunController.f3DPlayerTurretController.turret.headTransform.transform.localEulerAngles);
                        }
                    }
                }


            }
        }

        [System.Obsolete]
        private void FollowProcess()
        {
            if (Weapon_MachineGun.gameObject.activeSelf)
            {
                Vector3 relative = Vector3.zero;
                if (currentEnemyDetected != null)
                {

                    relative = transform.InverseTransformPoint(currentEnemyDetected.transform.position);
                }
                else
                {

                    relative = transform.InverseTransformPoint(GameplayManager.Instance.gunTarget.transform.position);
                }
                float angle = Mathf.Atan2(relative.z, relative.x) * Mathf.Rad2Deg;

                if (angle < 0)
                    angle += 360;

                Weapon_MachineGun.transform.localEulerAngles = new Vector3(0, 0, -angle + 90);
                SocketNetworkManager.Instance.EmitMachineGunRot(playerid.playerID, Vector3.zero, new Vector3(0, 0, -angle + 90));
            }
        }


        public void UpdateRotation(Vector3 newRot)
        {
            Weapon_MachineGun.transform.localEulerAngles = newRot;
        }


        public void GenBullate(Vector3 pos, Vector3 rot)
        {

            //Debug.Log("PPPPPPPPPPPPPPPPPPPPPPPPPPPP:::");
            Anim_MachineGun.Play();
            if (this.gameObject.activeSelf)
                AudioSource_Gun.PlayOneShot(AudioClip_Machinegun_Fire);

            if (playerid.isLocalPlayer)
                GameCanvas.Instance.FireEffect();

            Vector3 force = FiringPoint_Machinegun.transform.forward * 30;
            BulletScript newBullet = Instantiate(GameplayManager.Instance.Bullet_Machinegun_Player, pos, Quaternion.Euler(rot));
            newBullet.shooterID = playerid.playerID;
            newBullet.teamA = playerid.helixPlayerInfo.teamA;
            newBullet.teamB = playerid.helixPlayerInfo.teamB;
            Physics.IgnoreCollision(newBullet.GetComponent<BoxCollider>(), this.gameObject.transform.GetComponent<BoxCollider>());

            //newBullet.rb.AddForce(force, ForceMode.Impulse);
        }
        public void MissailBullate(Vector3 pos, Vector3 rot, float i, string missileId, bool isAI = false)
        {
            AudioSource_Gun.PlayOneShot(AudioClip_Missile_Fire);

            MissileScript newMissile = null;
            if (playerid.playerID == SocketNetworkManager.Instance.playerID)
            {
                newMissile = Instantiate(GameplayManager.Instance.Bullet_Missile_Player, pos, Quaternion.Euler(rot));
            }
            else
            {
                newMissile = Instantiate(GameplayManager.Instance.Bullet_Missile_Client, pos, Quaternion.Euler(rot));
            }

            newMissile.missileControl.isAI = isAI;
            Physics.IgnoreCollision(newMissile.GetComponent<BoxCollider>(), this.gameObject.transform.GetComponent<BoxCollider>());

            for (int j = 0; j < playerid.controller.wheelColliders.Length; j++)
            {
                //Debug.Log(":::::::::::::::+++++++++++++++++++");
                Physics.IgnoreCollision(newMissile.boxCollider, playerid.controller.wheelColliders[j]);
            }
            newMissile.missileId = missileId;
            newMissile.shooterId = playerid.playerID;
            newMissile.teamA = playerid.helixPlayerInfo.teamA;
            newMissile.teamB = playerid.helixPlayerInfo.teamB;

            if (GameplayManager.Instance.allmissiles.ContainsKey(missileId))
                Destroy(newMissile.gameObject);
            else
            {
                GameplayManager.Instance.allmissiles.Add(missileId, newMissile);
                if (playerid.playerID == SocketNetworkManager.Instance.playerID && isAI == false)
                {
                    if (SocketPlayerManager.Instance.MyPlayer != null)
                    {
                        SocketPlayerManager.Instance.MyPlayer.controller.currentMissile = newMissile;
                    }
                }
                //   else
                // newMissile.rb.mass = 0;

                //newMissile.rb.AddForce(force, ForceMode.Impulse);

                newMissile.missileControl.SetAsCurrentControlledID(playerid.playerID); // Set this missile as the currently controlled one




                // Notify when a missile is fired (for camera to follow)
                if (playerid.playerID == SocketNetworkManager.Instance.playerID && !isAI)
                    GameplayManager.Instance.OnRaiseMissileFiredHandler(newMissile.transform, playerid.playerID);
            }
        }

        //public void Fire_Missile()
        //{

        //    //Debug.Log(this.gameObject.name + "**********************");
        //    //playerID.controller.playeInputsActivate = false;

        //    if (this.enabled)
        //    {
        //        Missile_Firing_Point_Index++;
        //        var t = FiringPoints_Missiles[Missile_Firing_Point_Index % 2];
        //        string randomString = SocketNetworkManager.RandomString(8);
        //        if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)
        //            SocketNetworkManager.Instance.EmitFireUpdate("missile", t, Missile_Firing_Point_Index,playerid.helixPlayerInfo.userID, randomString, false);
        //        MissailBullate(t.position, t.localRotation.eulerAngles, Missile_Firing_Point_Index, randomString,true);
        //    }
        //}
        public void Add_Ammo_MachineGun(int Amount, AudioClip clip)
        {
            Ammo_Machinegun += Amount;
            GameCanvas.Instance.Text_Ammo_Machinegun.text = Ammo_Machinegun.ToString();
            AudioSource_Gun.PlayOneShot(clip);
            GameCanvas.Instance.Text_Ammo_Machinegun.color = Color.green;
        }
        public void Add_Ammo_Missile(int Amount, AudioClip clip)
        {
            Ammo_Missile += Amount;
            GameCanvas.Instance.Text_Ammo_Missile.text = Ammo_Missile.ToString();
            AudioSource_Gun.PlayOneShot(clip);
            GameCanvas.Instance.Text_Ammo_Missile.color = Color.green;
        }
        void ResetCamera()
        {
            //ResetCameraToCar();
        }
        private void ResetCameraToCar()
        {
            // Re-enable car controls
            if (playerInput != null)
            {
                playerInput.enabled = true;
            }

            // Notify to switch the camera back to the car
            GameplayManager.Instance.OnRaiseMissileFiredHandler(null, playerid.helixPlayerInfo.userID);
        }



        PlayerID currentFound;
        private void OnTriggerEnter(Collider other)
        {

            if (playerid.isLocalPlayer && currentEnemyDetected == null && (other.CompareTag("Player") || other.CompareTag("Enemy")))
            {
                currentFound = other.transform.GetComponent<PlayerID>();
                if (!((currentFound.helixPlayerInfo.teamA && !playerid.helixPlayerInfo.teamB) || (!currentFound.helixPlayerInfo.teamA && playerid.helixPlayerInfo.teamB)))
                {

                    if (!((currentFound.helixPlayerInfo.teamA && !playerid.helixPlayerInfo.teamB) || (!currentFound.helixPlayerInfo.teamA && playerid.helixPlayerInfo.teamB)))
                    {
                        currentEnemyDetected = currentFound;
                        currentFound = null;

                        if (playerid.currentGunController)
                        {
                            //playerid.currentGunController.f3DPlayerTurretController.turret.DebugTarget = currentEnemyDetected.shootingTransform.transform;
                        }
                    }
                    else
                        currentEnemyDetected = null;
                }
                else
                {
                    currentFound = null;
                }

            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (playerid.isLocalPlayer && currentEnemyDetected == null && (other.CompareTag("Player") || other.CompareTag("Enemy")))
            {
                currentFound = other.transform.GetComponent<PlayerID>();
                if (!((currentFound.helixPlayerInfo.teamA && !playerid.helixPlayerInfo.teamB) || (!currentFound.helixPlayerInfo.teamA && playerid.helixPlayerInfo.teamB)))
                {

                    if (!((currentFound.helixPlayerInfo.teamA && !playerid.helixPlayerInfo.teamB) || (!currentFound.helixPlayerInfo.teamA && playerid.helixPlayerInfo.teamB)))
                    {
                        currentEnemyDetected = currentFound;
                        currentFound = null;
                    }
                    else
                        currentFound = null;

                    if (playerid.currentGunController)
                    {
                        //playerid.currentGunController.f3DPlayerTurretController.turret.DebugTarget = currentEnemyDetected.shootingTransform.transform;
                    }
                }
                else
                {
                    currentFound = null;
                }
            }

            if (playerid.isLocalPlayer && currentEnemyDetected && playerid.currentGunController && playerid.currentGunController.f3DPlayerTurretController.turret.DebugTarget == null)
            {
                //playerid.currentGunController.f3DPlayerTurretController.turret.DebugTarget = currentEnemyDetected.shootingTransform.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (currentEnemyDetected && other.gameObject == currentEnemyDetected.gameObject)
            {
                if (currentEnemyDetected != null && playerid.currentGunController && playerid)
                {
                    currentEnemyDetected = null;



                    //playerid.currentGunController.f3DPlayerTurretController.turret.DebugTarget = null;
                    //playerid.currentGunController.f3DPlayerTurretController.turret.headTransform.localRotation = Quaternion.identity;
                    //SocketNetworkManager.Instance.EmitMachineGunRot(playerid.playerID, Vector3.zero, playerid.currentGunController.f3DPlayerTurretController.turret.headTransform.transform.localEulerAngles);

                }
            }

        }

    }
}
