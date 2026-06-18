using FORGE3D;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace CarControllerwithShooting
{
    public class GunControllerInAi : MonoBehaviour
    {
        [Header("Machine Gun Properties")]
        public BulletScript Bullet_Machinegun;
        //private float LastTime_Machinegun_Fire = 0;
        public float Machinegun_Firing_Interval;
        public AudioClip AudioClip_Machinegun_Fire;
        public Animation Anim_MachineGun;

        [Header("Missile Properties")]
        public MissileScript Bullet_Missile;
        public Transform[] FiringPoints_Missiles;
        public float Missile_Firing_Interval;
        private int Missile_Firing_Point_Index = 0;
        public AudioClip AudioClip_Missile_Fire;
        public ParticleSystem[] Particle_Missile_Firing_Explosion;

        [Header("Ammo Capacity")]
        //public int Ammo_Machinegun = 240;
        //public int Ammo_Missile = 20;

        public AudioSource AudioSource_Gun;
        //public  GunControllerInAi Instance;

        public F3DFXController Weapon_MachineGun;
        public GameObject Weapon_MissileLeft;
        public GameObject Weapon_MissileRight;
        bool fire = false;

        [SerializeField] PlayerID playerID;


        [SerializeField] internal PlayerID currentEnemy;

        [System.Obsolete]
        private void Awake()
        {
            fire = true;
            _ = StartCoroutine(AIFireMachingun());
        }
        [System.Obsolete]
        private void OnEnable()
        {
            //StopCoroutine(_);


            Weapon_MachineGun.gameObject.SetActive(true);
            _ = StartCoroutine(AIFireMachingun());
        }
        [System.Obsolete]
        private void OnDisable()
        {
            currentEnemy = null;
            Weapon_MachineGun.f3DPlayerTurretController.turret.DebugTarget = null;
            playerID.helixCarFollower.nearestObject = null;
            StopCoroutine(AIFireMachingun());
        }

        public float Range = 80;
        [System.Obsolete]
        private void Update()
        {
            //if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)
            //    FollowProcess();

            if (currentEnemy != null && SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)
            {
                if (Vector3.Distance(currentEnemy.transform.position, transform.position) < Range)
                {
                    FollowProcess();

                }
                else
                {
                    if (currentEnemy != null && playerID.currentGunController)
                    {
                        currentEnemy = null;
                        Weapon_MachineGun.f3DPlayerTurretController.turret.DebugTarget = null;
                        playerID.helixCarFollower.nearestObject = null;
                        playerID.currentGunController.f3DPlayerTurretController.turret.DebugTarget = null;
                        playerID.currentGunController.f3DPlayerTurretController.turret.headTransform.localRotation = Quaternion.identity;
                        SocketNetworkManager.Instance.EmitMachineGunRot(playerID.playerID, Vector3.zero, playerID.currentGunController.f3DPlayerTurretController.turret.headTransform.transform.localEulerAngles);
                    }
                    //Weapon_MachineGun.transform.localRotation = Quaternion.identity;
                    //if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)
                    //    SocketNetworkManager.Instance.EmitMachineGunRot(playerID.playerID, Vector3.zero, Weapon_MachineGun.transform.localRotation.eulerAngles);
                    //if (currentEnemy != null)
                    //{
                    //    //Debug
                    //    playerID.currentGunController.f3DPlayerTurretController.turret.DebugTarget = currentEnemy.transform;
                    //    SocketNetworkManager.Instance.EmitMachineGunRot(playerID.playerID, Vector3.zero, playerID.currentGunController.f3DPlayerTurretController.turret.barrelTransform.transform.localEulerAngles);
                    //}
                    // currentEnemyDetected = null;
                }

                if (currentEnemy != null)
                {
                    if (Vector3.Distance(currentEnemy.transform.position, transform.position) < Range == false || currentEnemy.gameObject.activeSelf == false)
                    {
                        //Debug.Log("***************************************************************************************************::::OUT OF RANGE$$$$$$$$$$$$$$$$$$$$$$$$");
                        currentEnemy = null;
                        //playerid.currentGunController.f3DPlayerTurretController.turret.DebugTarget = null;

                        if (playerID.currentGunController)
                        {
                            playerID.currentGunController.f3DPlayerTurretController.turret.headTransform.localRotation = Quaternion.identity;
                            SocketNetworkManager.Instance.EmitMachineGunRot(playerID.playerID, Vector3.zero, playerID.currentGunController.f3DPlayerTurretController.turret.headTransform.transform.localEulerAngles);
                        }
                    }
                }
            }
        }


        int randomnumber = 0;
        [System.Obsolete]
        IEnumerator AIFireMachingun()
        {
            while (fire)
            {
                yield return new WaitForSeconds(.5f);


                if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser && currentEnemy)
                {
                    //Debug.Log("playerID.helixPlayerInfo.userID::::" + playerID.helixPlayerInfo.userID);
                    if (playerID.currentGunController)
                    {
                        if (playerID.currentGunController.f3DPlayerTurretController.turret.DebugTarget == null)
                            playerID.currentGunController.f3DPlayerTurretController.turret.DebugTarget = currentEnemy.shootingTransform;

                        if (playerID.currentGunController.f3DPlayerTurretController.turret.DebugTarget != null)
                        {
                            SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerID.helixPlayerInfo.userID, "");
                            randomnumber = Random.Range(0, 50);
                            if (randomnumber == 15 || randomnumber == 0)
                            {
                                Fire_Missile();
                            }
                        }
                    }
                    else
                    {

                        FireBullate();
                    }






                }
                else if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser && currentEnemy == null && playerID.currentGunController && playerID.currentGunController.f3DPlayerTurretController.isFiring)
                {
                    playerID.currentGunController.f3DPlayerTurretController.StopAIFiring();
                }
            }
        }


        int Gun_Firing_Point_Index = 0;
        public void FireBullate()
        {
            if (this.enabled && currentEnemy && currentEnemy.gameObject.activeSelf && SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser && Weapon_MachineGun.gameObject.activeSelf)
            {
                if (Weapon_MachineGun.f3DPlayerTurretController.turret.DebugTarget == null)
                {
                    Weapon_MachineGun.f3DPlayerTurretController.turret.DebugTarget = currentEnemy.shootingTransform.transform;
                }

                if (!(Weapon_MachineGun.TurretSocket.Length > Gun_Firing_Point_Index))
                {
                    Gun_Firing_Point_Index = 0;
                }

                var t = Weapon_MachineGun.TurretSocket[Gun_Firing_Point_Index];
                string randomString = SocketNetworkManager.RandomString(8);

                SocketNetworkManager.Instance.EmitFireUpdate("NormalBullet", t, Gun_Firing_Point_Index, playerID.helixPlayerInfo.userID, randomString, false);
                GenBullate(t.position, t.rotation.eulerAngles);
                Gun_Firing_Point_Index++;
            }
        }



        //[System.Obsolete]
        public void GenBullate(Vector3 pos, Vector3 rot)
        {
            Anim_MachineGun.Play();

            if (gameObject.activeSelf && AudioSource_Gun.enabled)
                AudioSource_Gun.PlayOneShot(AudioClip_Machinegun_Fire);


            BulletScript newBullet = Instantiate(Bullet_Machinegun, pos, Quaternion.Euler(rot));
            if (playerID == null)
            {
                playerID = this.transform.GetComponent<PlayerID>();
            }
            newBullet.shooterID = playerID.helixPlayerInfo.userID;
            newBullet.teamA = playerID.helixPlayerInfo.teamA;
            newBullet.teamB = playerID.helixPlayerInfo.teamB;

            Physics.IgnoreCollision(newBullet.GetComponent<BoxCollider>(), this.gameObject.transform.GetComponent<BoxCollider>());
            //newBullet.rb.AddForce(force, ForceMode.Impulse);
        }

        #region non usable code...

        public void DeactivateWeapons()
        {
            //GameCanvas.Instance.button_Machinegun.gameObject.SetActive(false);
            // GameCanvas.Instance.button_Missile.gameObject.SetActive(false);
            Weapon_MachineGun.gameObject.SetActive(false);
            Weapon_MissileLeft.SetActive(false);
            Weapon_MissileRight.SetActive(false);
            this.enabled = false;
        }



        //[ContextMenu("::::::::::::")]
        public void Fire_Missile(bool isPowerUp = false)
        {

            Debug.Log(this.gameObject.name + "**********************");
            //playerID.controller.playeInputsActivate = false;

            if (this.enabled)
            {
                Missile_Firing_Point_Index++;
                var t = FiringPoints_Missiles[Missile_Firing_Point_Index % 2];
                string randomString = SocketNetworkManager.RandomString(8);

                if (isPowerUp)
                {
                    if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)
                        SocketNetworkManager.Instance.EmitFireUpdate("missile", t, Missile_Firing_Point_Index, playerID.helixPlayerInfo.userID, randomString, false);
                    MissailBullate(t.position, t.localRotation.eulerAngles, Missile_Firing_Point_Index, randomString);
                }
                else
                {
                    if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)
                        SocketNetworkManager.Instance.EmitFireUpdate("NormalMissile", t, Missile_Firing_Point_Index, playerID.helixPlayerInfo.userID, randomString, false);
                    playerID.f3DMissileLauncher.FireMissile(Missile_Firing_Point_Index % 2);
                }
            }
        }



        public void MissailBullate(Vector3 pos, Vector3 rot, float i, string missileId)
        {

            AudioSource_Gun.PlayOneShot(AudioClip_Missile_Fire);
            Vector3 force = FiringPoints_Missiles[(int)i % 2].transform.forward * 300;
            MissileScript newMissile = Instantiate(GameplayManager.Instance.Bullet_Missile_Player, pos, Quaternion.identity);
            //newMissile.transform.position = pos;
            newMissile.missileControl.isAI = true;
            //newMissile.transform.localEulerAngles = rot;
            Physics.IgnoreCollision(newMissile.GetComponent<BoxCollider>(), this.gameObject.transform.GetComponent<BoxCollider>());

            for (int j = 0; j < playerID.controller.wheelColliders.Length; j++)
            {
                Physics.IgnoreCollision(newMissile.boxCollider, playerID.controller.wheelColliders[j]);
            }
            Debug.Log(rot + "$$$$$" + missileId);
            newMissile.name = missileId;
            newMissile.missileId = missileId;
            newMissile.shooterId = playerID.playerID;
            newMissile.teamA = playerID.helixPlayerInfo.teamA;
            newMissile.teamB = playerID.helixPlayerInfo.teamB;

            if (GameplayManager.Instance.allmissiles.ContainsKey(missileId))
                Destroy(newMissile.gameObject);
            else
            {
                GameplayManager.Instance.allmissiles.Add(missileId, newMissile);
                if (playerID.playerID == SocketNetworkManager.Instance.playerID)
                {
                    if (SocketPlayerManager.Instance.MyPlayer != null)
                    {
                        SocketPlayerManager.Instance.MyPlayer.controller.currentMissile = newMissile;
                    }
                }


                newMissile.missileControl.SetAsCurrentControlledID(playerID.playerID); // Set this missile as the currently controlled one

            }
        }

        [System.Obsolete]
        private void FollowProcess()
        {
            if (currentEnemy != null)
            {
                if (playerID.currentGunController == null)
                {
                    //Vector3 relative = transform.InverseTransformPoint(currentEnemy.transform.position);
                    //float angle = Mathf.Atan2(relative.z, relative.x) * Mathf.Rad2Deg;

                    //if (angle < 0)
                    //    angle += 360;

                    //Debug.Log("Follow Roatation");
                    //Weapon_MachineGun.transform.localEulerAngles = new Vector3(0, 0, -angle + 90);
                    //if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)
                    //    SocketNetworkManager.Instance.EmitMachineGunRot(playerID.playerID, Vector3.zero, new Vector3(0, 0, -angle + 90));
                }
                else if (playerID.currentGunController != null && playerID.currentGunController.f3DPlayerTurretController.turret.DebugTarget == null)
                    playerID.currentGunController.f3DPlayerTurretController.turret.DebugTarget = currentEnemy.shootingTransform;
            }
        }
        public void UpdateRotation(Vector3 newRot)
        {
            Weapon_MachineGun.transform.localEulerAngles = newRot;
        }


        PlayerID currentFound;
        private void OnTriggerEnter(Collider other)
        {

            if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser && this.enabled)
            {

                if (currentEnemy == null && other.CompareTag("Player") || other.CompareTag("Enemy"))
                {

                    currentFound = other.transform.GetComponent<PlayerID>();
                    if ((currentFound.helixPlayerInfo.teamA && !playerID.helixPlayerInfo.teamA) || (!currentFound.helixPlayerInfo.teamA && playerID.helixPlayerInfo.teamA) || (currentFound.helixPlayerInfo.teamB && !playerID.helixPlayerInfo.teamB) || (!currentFound.helixPlayerInfo.teamB && playerID.helixPlayerInfo.teamB))
                    {
                        currentEnemy = currentFound;
                        playerID.helixCarFollower.nearestObject = currentFound.gameObject;
                        Weapon_MachineGun.f3DPlayerTurretController.turret.DebugTarget = currentFound.shootingTransform.transform;
                        currentFound = null;
                    }
                    else
                        currentFound = null;

                    //Debug.Log(":::::::::FOUND ENEMY"+other.gameObject.name);

                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (currentEnemy == null && (other.CompareTag("Player") || other.CompareTag("Enemy")) && this.enabled)
            {

                currentFound = other.transform.GetComponent<PlayerID>();
                if ((currentFound.helixPlayerInfo.teamA && !playerID.helixPlayerInfo.teamA) || (!currentFound.helixPlayerInfo.teamA && playerID.helixPlayerInfo.teamA) || (currentFound.helixPlayerInfo.teamB && !playerID.helixPlayerInfo.teamB) || (!currentFound.helixPlayerInfo.teamB && playerID.helixPlayerInfo.teamB))
                {
                    currentEnemy = currentFound;
                    Weapon_MachineGun.f3DPlayerTurretController.turret.DebugTarget = currentFound.shootingTransform.transform;
                    playerID.helixCarFollower.nearestObject = currentFound.gameObject;
                    currentFound = null;
                }
                else
                {

                    currentFound = null;
                }

                //Debug.Log(":::::::::FOUND:::::ENEMY"+other.gameObject.name);

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (currentEnemy && other.gameObject == currentEnemy.gameObject && this.enabled)
            {
                if (currentEnemy != null && playerID.currentGunController)
                {
                    Weapon_MachineGun.f3DPlayerTurretController.turret.DebugTarget = null;
                    currentEnemy = null;
                    playerID.helixCarFollower.nearestObject = null;
                    playerID.currentGunController.f3DPlayerTurretController.turret.DebugTarget = null;
                    playerID.currentGunController.f3DPlayerTurretController.turret.headTransform.localRotation = Quaternion.identity;
                    SocketNetworkManager.Instance.EmitMachineGunRot(playerID.playerID, Vector3.zero, playerID.currentGunController.f3DPlayerTurretController.turret.headTransform.transform.localEulerAngles);
                }
            }
        }




        public void Add_Ammo_MachineGun(int Amount, AudioClip clip)
        {
            //Ammo_Machinegun += Amount;
            //GameCanvas.Instance.Text_Ammo_Machinegun.text = Ammo_Machinegun.ToString();
            AudioSource_Gun.PlayOneShot(clip);
            GameCanvas.Instance.Text_Ammo_Machinegun.color = Color.green;
        }
        public void Add_Ammo_Missile(int Amount, AudioClip clip)
        {
            //Ammo_Missile += Amount;
            //GameCanvas.Instance.Text_Ammo_Missile.text = Ammo_Missile.ToString();
            AudioSource_Gun.PlayOneShot(clip);
            GameCanvas.Instance.Text_Ammo_Missile.color = Color.green;
        }




        #endregion
    }
}
