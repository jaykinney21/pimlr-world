using UnityEngine;
using System.Collections;
using System;
using CarControllerwithShooting;

namespace FORGE3D
{
    // Weapon types
    public enum F3DFXType
    {
        Vulcan,
        SoloGun,
        Sniper,
        ShotGun,
        Seeker,
        RailGun,
        PlasmaGun,
        PlasmaBeam,
        PlasmaBeamHeavy,
        LightningGun,
        FlameRed,
        LaserImpulse
    }

    public class F3DFXController : MonoBehaviour
    {
        // Singleton instance
        //public static F3DFXController instance;


        // Current firing socket
        private int curSocket = 0;

        // Timer reference                
        private int timerID = -1;

        [Header("Turret setup")] public Transform[] TurretSocket; // Sockets reference
        public ParticleSystem[] ShellParticles; // Bullet shells particle system

        public F3DFXType DefaultFXType; // Default starting weapon type

        [Header("Vulcan")] public Transform vulcanProjectile; // Projectile prefab
        public Transform vulcanMuzzle; // Muzzle flash prefab  
        public Transform vulcanImpact; // Impact prefab
        public float vulcanOffset;

        public float VulcanFireRate = 0.07f;

        [Header("Solo gun")] public Transform soloGunProjectile;
        public Transform soloGunMuzzle;
        public Transform soloGunImpact;
        public float soloGunOffset;

        [Header("Sniper")] public Transform sniperBeam;
        public Transform sniperMuzzle;
        public Transform sniperImpact;
        public float sniperOffset;

        [Header("Shotgun")] public Transform shotGunProjectile;
        public Transform shotGunMuzzle;
        public Transform shotGunImpact;
        public float shotGunOffset;

        [Header("Seeker")] public Transform seekerProjectile;
        public Transform seekerMuzzle;
        public Transform seekerImpact;
        public float seekerOffset;

        [Header("Rail gun")] public Transform railgunBeam;
        public Transform railgunMuzzle;
        public Transform railgunImpact;
        public float railgunOffset;

        [Header("Plasma gun")] public Transform plasmagunProjectile;
        public Transform plasmagunMuzzle;
        public Transform plasmagunImpact;
        public float plasmaGunOffset;

        [Header("Plasma beam")] public Transform plasmaBeam;
        public float plasmaOffset;

        [Header("Plasma beam heavy")] public Transform plasmaBeamHeavy;
        public float plasmaBeamHeavyOffset;

        [Header("Lightning gun")] public Transform lightningGunBeam;
        public float lightingGunBeamOffset;

        [Header("Flame")] public Transform flameRed;
        public float flameOffset;

        [Header("Laser impulse")] public Transform laserImpulseProjectile;
        public Transform laserImpulseMuzzle;
        public Transform laserImpulseImpact;
        public float laserImpulseOffset;


       [SerializeField] internal PlayerID playerid;

        public float DamagePower;


        public F3DPlayerTurretController f3DPlayerTurretController;
        private void Awake()
        {
            // Initialize singleton  
            //instance = this;

            //  FindObjectOfType<F3DTurretUI>()._fxControllers.Add(this);
            // Initialize bullet shells particles
            for (int i = 0; i < ShellParticles.Length; i++)
            {
                var em = ShellParticles[i].emission;
                em.enabled = false;
                ShellParticles[i].Stop();
                ShellParticles[i].gameObject.SetActive(true);
            }
        }


        // Switch to next weapon type
        public void NextWeapon()
        {
            if ((int)DefaultFXType < Enum.GetNames(typeof(F3DFXType)).Length - 1)
            {
                //  DefaultFXType++;
            }
        }

        // Switch to previous weapon type
        public void PrevWeapon()
        {
            if (DefaultFXType > 0)
            {
                //  DefaultFXType--;
            }
        }

        // Advance to next turret socket
        private void AdvanceSocket()
        {
            curSocket++;
            if (curSocket >= TurretSocket.Length)
                curSocket = 0;
        }

        // Fire turret weapon

        float valueOfValue = 0;
        public float currentRound
        {
            get { return valueOfValue; }
            set
            {
                valueOfValue = value;
                if (playerid && playerid.isLocalPlayer)
                {
                    if (valueOfValue > 0 && valueOfValue < 10)
                    {
                        GameCanvas.Instance.powerUpValue.text = "0" + ((int)valueOfValue).ToString();
                    }
                    else
                    {
                        GameCanvas.Instance.powerUpValue.text = ((int)valueOfValue).ToString();

                    }
                }
            }
        }
        //public float currentUsedTime = 0;


        bool isUsed = false;




        private void OnEnable()
        {
            if (playerid && playerid.isLocalPlayer && playerid.gunController != null /*&& playerid.gunController.currentEnemyDetected != null*/)
            {
                //Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$&&&&&&&&&&&&&&&&&&&&&&&&&&");
                //  f3DPlayerTurretController.turret.DebugTarget = GameplayManager.Instance.gunTarget ;                     ///Assgin gun target
            }
        }

        private void OnDisable()
        {
            //playerid = null;
        }
        private void Update()
        {
            if (currentRound >= 0 && isUsed)
            {
                currentRound -= Time.deltaTime;
                if (currentRound <= 0 && isUsed && playerid.controller.currentPowerUp)
                {
                    isUsed = false;
                    playerid.controller.currentPowerUp.OnCompletedUsePowerUp(playerid.playerID, playerid.isLocalPlayer);
                    Stop();
                }
            }
        }
        public void Fire(bool sendToServer)
        {
            //Debug.Log("<color=Green>" + (playerid.controller.currentPowerUp == null) + "::::::::::::</color>" + currentRound + "::::" + playerid.name);

            if (playerid && playerid.controller.currentPowerUp && currentRound > 0 )
            {



                playerid.controller.currentPowerUp.OnStartUsePowerUp(playerid.playerID, false);
                switch (DefaultFXType)
                {

                    case F3DFXType.Vulcan:
                        // Fire vulcan at specified rate until canceled
                        if (sendToServer)
                            timerID = F3DTime.time.AddTimer(VulcanFireRate, () => { Vulcan(sendToServer); });
                        // Invoke manually before the timer ticked to avoid initial delay
                        Vulcan(sendToServer);


                        break;

                    case F3DFXType.SoloGun:
                        if (sendToServer)
                            timerID = F3DTime.time.AddTimer(0.2f, () => { SoloGun(sendToServer); });
                        SoloGun(sendToServer);
                        break;

                    case F3DFXType.Sniper:
                        if (sendToServer)
                            timerID = F3DTime.time.AddTimer(0.3f, () => { Sniper(sendToServer); });
                        Sniper(sendToServer);
                        break;

                    case F3DFXType.ShotGun:
                        if (sendToServer)
                            timerID = F3DTime.time.AddTimer(0.3f, () => { ShotGun(sendToServer); });
                        ShotGun(sendToServer);
                        break;

                    case F3DFXType.Seeker:
                        if (sendToServer)
                            timerID = F3DTime.time.AddTimer(0.2f, () => { Seeker(sendToServer); });
                        Seeker(sendToServer);
                        break;

                    case F3DFXType.RailGun:
                        if (sendToServer)
                            timerID = F3DTime.time.AddTimer(0.2f, () => { RailGun(sendToServer); });
                        RailGun(sendToServer);
                        break;

                    case F3DFXType.PlasmaGun:
                        if (sendToServer)
                            timerID = F3DTime.time.AddTimer(0.2f, () =>
                            {
                                PlasmaGun(sendToServer);
                            });
                        PlasmaGun(sendToServer);
                        break;

                    case F3DFXType.PlasmaBeam:
                        // Beams has no timer requirement
                        isUsed = true;
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                        PlasmaBeam();
                        break;

                    case F3DFXType.PlasmaBeamHeavy:
                        // Beams has no timer requirement
                        isUsed = true;
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                        PlasmaBeamHeavy();
                        break;

                    case F3DFXType.LightningGun:
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                        // Beams has no timer requirement
                        isUsed = true;
                        LightningGun();
                        break;

                    case F3DFXType.FlameRed:
                        // Flames has no timer requirement
                        isUsed = true;
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                        FlameRed();
                        break;

                    case F3DFXType.LaserImpulse:
                        if (sendToServer)
                            timerID = F3DTime.time.AddTimer(0.15f, () => { LaserImpulse(sendToServer); });
                        LaserImpulse(sendToServer);
                        break;
                }
            }
        }

        // Stop firing 
        public void Stop()
        {
            isUsed = false;
            // Remove firing timer
            if (timerID != -1)
            {
                F3DTime.time.RemoveTimer(timerID);
                timerID = -1;
            }
            if (currentRound <= 0 && isUsed)
            {
                isUsed = false;
                playerid.controller.currentPowerUp.OnCompletedUsePowerUp(playerid.playerID, playerid.isLocalPlayer);

            }

            if (playerid && playerid.controller.currentPowerUp)
                playerid.controller.currentPowerUp.isUsed = false;
            switch (DefaultFXType)
            {
                case F3DFXType.PlasmaBeam:
                    F3DAudioController.instance.PlasmaBeamClose(transform.position);
                    break;

                case F3DFXType.PlasmaBeamHeavy:
                    F3DAudioController.instance.PlasmaBeamHeavyClose(transform.position);
                    break;

                case F3DFXType.LightningGun:
                    F3DAudioController.instance.LightningGunClose(transform.position);
                    break;

                case F3DFXType.FlameRed:
                    F3DAudioController.instance.FlameGunClose(transform.position);
                    break;
            }
        }

        // Fire vulcan weapon
        private void Vulcan(bool sendToServer)
        {

            try
            {

                if (currentRound > 0)
                {
                    currentRound--;
                    if (currentRound <= 0)
                    {
                        Stop();
                        playerid.controller.currentPowerUp.OnCompletedUsePowerUp(playerid.playerID, playerid.isLocalPlayer);
                    }

                    if (playerid.isLocalPlayer)
                        GameCanvas.Instance.FireEffect();

                    if (sendToServer && playerid && this.gameObject)
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                    // Get random rotation that offset spawned projectile
                    var offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
                    // Spawn muzzle flash and projectile with the rotation offset at current socket position
                    F3DPoolManager.Pools["GeneratedPool"]?.Spawn(vulcanMuzzle, TurretSocket[curSocket].position,
                        TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
                    var newGO =
                        F3DPoolManager.Pools["GeneratedPool"]?.Spawn(vulcanProjectile,
                            TurretSocket[curSocket].position + TurretSocket[curSocket].forward,
                            offset * TurretSocket[curSocket].rotation, null).gameObject;

                    var proj = newGO.gameObject.GetComponent<F3DProjectile>();
                    proj.f3DFXController = this;
                    if (proj)
                    {
                        proj.SetOffset(vulcanOffset);
                    }

                    // Emit one bullet shell
                    if (ShellParticles.Length > 0)
                        ShellParticles[curSocket].Emit(1);

                    // Play shot sound effect
                    F3DAudioController.instance.VulcanShot(TurretSocket[curSocket].position);

                    // Advance to next turret socket
                    AdvanceSocket();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }

        }

        // Spawn vulcan weapon impact
        public void VulcanImpact(Vector3 pos)
        {

            try
            {
                // Spawn impact prefab at specified position
                F3DPoolManager.Pools["GeneratedPool"]?.Spawn(vulcanImpact, pos, Quaternion.identity, null);
                // Play impact sound effect
                F3DAudioController.instance.VulcanHit(pos);
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Fire sologun weapon
        private void SoloGun(bool sendToServer)
        {
            try
            {
                if (currentRound > 0)
                {
                    currentRound--;
                    if (currentRound <= 0)
                    {
                        Stop();
                        playerid.controller.currentPowerUp.OnCompletedUsePowerUp(playerid.playerID, playerid.isLocalPlayer);
                    }

                    if (playerid.isLocalPlayer)
                        GameCanvas.Instance.FireEffect();
                    if (sendToServer && playerid && this.gameObject)
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                    var offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
                    F3DPoolManager.Pools["GeneratedPool"]?.Spawn(soloGunMuzzle, TurretSocket[curSocket].position,
                        TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
                    var newGO =
                        F3DPoolManager.Pools["GeneratedPool"]?.Spawn(soloGunProjectile,
                            TurretSocket[curSocket].position + TurretSocket[curSocket].forward,
                            offset * TurretSocket[curSocket].rotation, null).gameObject;
                    var proj = newGO.GetComponent<F3DProjectile>();
                    proj.f3DFXController = this;
                    if (proj)
                    {
                        proj.SetOffset(soloGunOffset);
                    }

                    F3DAudioController.instance.SoloGunShot(TurretSocket[curSocket].position);
                    AdvanceSocket();


                }
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Spawn sologun weapon impact
        public void SoloGunImpact(Vector3 pos)
        {
            try
            {
                F3DPoolManager.Pools["GeneratedPool"]?.Spawn(soloGunImpact, pos, Quaternion.identity, null);
                F3DAudioController.instance.SoloGunHit(pos);
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Fire sniper weapon
        private void Sniper(bool sendToServer)
        {

            try
            {
                if (currentRound > 0)
                {
                    currentRound--;
                    if (currentRound <= 0)
                    {
                        Stop();
                        playerid.controller.currentPowerUp.OnCompletedUsePowerUp(playerid.playerID, playerid.isLocalPlayer);
                    }


                    if (playerid.isLocalPlayer)
                        GameCanvas.Instance.FireEffect();

                    if (sendToServer && playerid && this.gameObject)
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                    var offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);

                    F3DPoolManager.Pools["GeneratedPool"]?.Spawn(sniperMuzzle, TurretSocket[curSocket].position,
                        TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
                    var newGO = F3DPoolManager.Pools["GeneratedPool"]?.Spawn(sniperBeam, TurretSocket[curSocket].position,
                            offset * TurretSocket[curSocket].rotation, null).gameObject;
                    var beam = newGO.GetComponent<F3DBeam>();
                    beam.f3DFXController = this;
                    //Debug.Log("aAAAAAAAAAAAAAAAA");


                    if (beam)
                    {
                        beam.SetOffset(sniperOffset);
                    }

                    F3DAudioController.instance.SniperShot(TurretSocket[curSocket].position);
                    if (ShellParticles.Length > 0)
                        ShellParticles[curSocket].Emit(1);
                    AdvanceSocket();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Spawn sniper weapon impact
        public void SniperImpact(Vector3 pos)
        {
            try
            {

                F3DPoolManager.Pools["GeneratedPool"]?.Spawn(sniperImpact, pos, Quaternion.identity, null);
                F3DAudioController.instance.SniperHit(pos);
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Fire shotgun weapon
        private void ShotGun(bool sendToServer)
        {

            try
            {
                if (currentRound > 0)
                {
                    currentRound--;
                    if (currentRound <= 0)
                    {
                        Stop();
                        playerid.controller.currentPowerUp.OnCompletedUsePowerUp(playerid.playerID, playerid.isLocalPlayer);
                    }

                    if (sendToServer && playerid && this.gameObject)
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                    //                playerid.controller.currentPowerUp.OnCompletedUsePowerUp(playerid.playerID, playerid.isLocalPlayer);

                    if (playerid.isLocalPlayer)
                        GameCanvas.Instance.FireEffect();

                    var offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
                    F3DPoolManager.Pools["GeneratedPool"]?.Spawn(shotGunMuzzle, TurretSocket[curSocket].position,
                        TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
                    var newGO = F3DPoolManager.Pools["GeneratedPool"]?.Spawn(shotGunProjectile, TurretSocket[curSocket].position,
                        offset * TurretSocket[curSocket].rotation, null);

                    newGO.GetComponent<F3DShotgun>().f3DFXController = this;

                    F3DAudioController.instance.ShotGunShot(TurretSocket[curSocket].position);
                    if (ShellParticles.Length > 0)
                        ShellParticles[curSocket].Emit(1);
                    AdvanceSocket();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Fire seeker weapon
        private void Seeker(bool sendToServer)
        {

            try
            {
                if (currentRound > 0)
                {
                    currentRound--;
                    if (currentRound <= 0)
                    {
                        Stop();
                        playerid.controller.currentPowerUp.OnCompletedUsePowerUp(playerid.playerID, playerid.isLocalPlayer);
                    }

                    if (playerid.isLocalPlayer)
                        GameCanvas.Instance.FireEffect();

                    if (sendToServer && playerid && this.gameObject)
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                    var offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
                    F3DPoolManager.Pools["GeneratedPool"]?.Spawn(seekerMuzzle, TurretSocket[curSocket].position,
                        TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
                    var newGO =
                        F3DPoolManager.Pools["GeneratedPool"]?.Spawn(seekerProjectile, TurretSocket[curSocket].position,
                            offset * TurretSocket[curSocket].rotation, null).gameObject;
                    var proj = newGO.GetComponent<F3DProjectile>();
                    proj.f3DFXController = this;
                    if (proj)
                    {
                        proj.SetOffset(seekerOffset);
                    }

                    F3DAudioController.instance.SeekerShot(TurretSocket[curSocket].position);
                    AdvanceSocket();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Spawn seeker weapon impact
        public void SeekerImpact(Vector3 pos)
        {
            try
            {
                F3DPoolManager.Pools["GeneratedPool"]?.Spawn(seekerImpact, pos, Quaternion.identity, null);
                F3DAudioController.instance.SeekerHit(pos);
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Fire rail gun weapon
        private void RailGun(bool sendToServer)
        {
            try
            {
                if (currentRound > 0)
                {
                    currentRound--;
                    if (currentRound <= 0)
                    {
                        Stop();
                        playerid.controller.currentPowerUp.OnCompletedUsePowerUp(playerid.playerID, playerid.isLocalPlayer);
                    }
                    if (playerid.isLocalPlayer)
                        GameCanvas.Instance.FireEffect();

                    if (sendToServer && playerid && this.gameObject)
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                    var offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
                    if (railgunMuzzle)
                        F3DPoolManager.Pools["GeneratedPool"]?.Spawn(railgunMuzzle, TurretSocket[curSocket].position,
                            TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
                    var newGO =
                        F3DPoolManager.Pools["GeneratedPool"]?.Spawn(railgunBeam, TurretSocket[curSocket].position,
                            offset * TurretSocket[curSocket].rotation, null).gameObject;
                    var beam = newGO.GetComponent<F3DBeam>();
                    beam.f3DFXController = this;
                    if (beam)
                    {
                        beam.SetOffset(railgunOffset);
                    }

                    F3DAudioController.instance.RailGunShot(TurretSocket[curSocket].position);
                    if (ShellParticles.Length > 0)
                        ShellParticles[curSocket].Emit(1);
                    AdvanceSocket();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Spawn rail gun weapon impact
        public void RailgunImpact(Vector3 pos)
        {
            try
            {
                F3DPoolManager.Pools["GeneratedPool"]?.Spawn(railgunImpact, pos, Quaternion.identity, null);
                F3DAudioController.instance.RailGunHit(pos);
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Fire plasma gun weapon
        private void PlasmaGun(bool sendToServer)
        {

            try
            {
                if (currentRound > 0)
                {
                    currentRound--;
                    if (currentRound <= 0)
                    {
                        Stop();
                        playerid.controller.currentPowerUp.OnCompletedUsePowerUp(playerid.playerID, playerid.isLocalPlayer);
                    }
                    if (playerid.isLocalPlayer)
                        GameCanvas.Instance.FireEffect();
                    if (sendToServer && playerid && this.gameObject)
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                    var offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
                    F3DPoolManager.Pools["GeneratedPool"]?.Spawn(plasmagunMuzzle, TurretSocket[curSocket].position,
                        TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
                    var newGo =
                        F3DPoolManager.Pools["GeneratedPool"]?.Spawn(plasmagunProjectile, TurretSocket[curSocket].position,
                            offset * TurretSocket[curSocket].rotation, null).gameObject;
                    var proj = newGo.GetComponent<F3DProjectile>();
                    proj.f3DFXController = this;
                    if (proj)
                    {
                        proj.SetOffset(plasmaOffset);
                    }

                    F3DAudioController.instance.PlasmaGunShot(TurretSocket[curSocket].position);
                    AdvanceSocket();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Spawn plasma gun weapon impact
        public void PlasmaGunImpact(Vector3 pos)
        {
            try
            {
                F3DPoolManager.Pools["GeneratedPool"]?.Spawn(plasmagunImpact, pos, Quaternion.identity, null);
                F3DAudioController.instance.PlasmaGunHit(pos);
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Fire plasma beam weapon
        private void PlasmaBeam()
        {

            try
            {
                for (var i = 0; i < TurretSocket.Length; i++)
                {
                    var newGo = F3DPoolManager.Pools["GeneratedPool"]?.Spawn(plasmaBeam, TurretSocket[i].position,
                        TurretSocket[i].rotation,
                        TurretSocket[i]);

                    newGo.GetComponent<F3DBeam>().f3DFXController = this;
                }

                F3DAudioController.instance.PlasmaBeamLoop(transform.position, transform.parent);
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Fire heavy beam weapon
        private void PlasmaBeamHeavy()
        {
            try
            {
                for (var i = 0; i < TurretSocket.Length; i++)
                {
                    var newGo = F3DPoolManager.Pools["GeneratedPool"]?.Spawn(plasmaBeamHeavy, TurretSocket[i].position,
                          TurretSocket[i].rotation,
                          TurretSocket[i]);

                    newGo.GetComponent<F3DBeam>().f3DFXController = this;
                }

                F3DAudioController.instance.PlasmaBeamHeavyLoop(transform.position, transform.parent);
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Fire lightning gun weapon
        private void LightningGun()
        {

            try
            {
                for (var i = 0; i < TurretSocket.Length; i++)
                {
                    var newGo = F3DPoolManager.Pools["GeneratedPool"]?.Spawn(lightningGunBeam, TurretSocket[i].position,
                         TurretSocket[i].rotation,
                         TurretSocket[i]);

                    newGo.GetComponent<F3DLightning>().f3DFXController = this;
                }

                F3DAudioController.instance.LightningGunLoop(transform.position, transform);
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Fire flames weapon
        private void FlameRed()
        {
            try
            {
                for (var i = 0; i < TurretSocket.Length; i++)
                {
                    var newGo = F3DPoolManager.Pools["GeneratedPool"]?.Spawn(flameRed, TurretSocket[i].position,
                           TurretSocket[i].rotation,
                           TurretSocket[i]);

                    if (newGo)
                        newGo.GetComponent<F3DFlameThrower>().f3DFXController = this;
                }

                F3DAudioController.instance.FlameGunLoop(transform.position, transform);
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Fire laser pulse weapon
        private void LaserImpulse(bool sendToServer)
        {

            try
            {
                if (currentRound > 0)
                {
                    currentRound--;
                    if (currentRound <= 0)
                    {
                        Stop();
                        playerid.controller.currentPowerUp.OnCompletedUsePowerUp(playerid.playerID, playerid.isLocalPlayer);
                    }
                    if (playerid.isLocalPlayer)
                        GameCanvas.Instance.FireEffect();

                    if (sendToServer && playerid && this.gameObject)
                        SocketNetworkManager.Instance.EmitFireUpdate("bullet", this.transform, 15, playerid.helixPlayerInfo.userID, "");
                    var offset = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
                    F3DPoolManager.Pools["GeneratedPool"]?.Spawn(laserImpulseMuzzle, TurretSocket[curSocket].position,
                        TurretSocket[curSocket].rotation, TurretSocket[curSocket]);
                    var newGO =
                        F3DPoolManager.Pools["GeneratedPool"]?.Spawn(laserImpulseProjectile, TurretSocket[curSocket].position,
                            offset * TurretSocket[curSocket].rotation, null).gameObject;
                    var proj = newGO.GetComponent<F3DProjectile>();
                    proj.f3DFXController = this;
                    if (proj)
                    {
                        proj.SetOffset(laserImpulseOffset);
                    }

                    F3DAudioController.instance.LaserImpulseShot(TurretSocket[curSocket].position);

                    AdvanceSocket();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }

        // Spawn laser pulse weapon impact
        public void LaserImpulseImpact(Vector3 pos)
        {
            try
            {
                F3DPoolManager.Pools["GeneratedPool"]?.Spawn(laserImpulseImpact, pos, Quaternion.identity, null);
                F3DAudioController.instance.LaserImpulseHit(pos);
            }
            catch (Exception e)
            {
                Debug.Log("Error = " + e.ToString());
            }
        }
    }
}