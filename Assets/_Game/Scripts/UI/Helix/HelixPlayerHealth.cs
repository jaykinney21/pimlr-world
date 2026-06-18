using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HelixPlayerHealth : MonoBehaviour
{

    [Header("Settings")]
    public float Health = 100;
    public float MaxHealth = 100;

    [Header("Effects")]
    public bool BloodScreenEffect = false;
    public GameObject BloodHitParticle;
    public GameObject ExplosionParticle;

    [Header("On Death Event")]
    public UnityEvent OnDeath;

    [Header("Stats")]
    public bool IsDead;

    [Header("PlayerID")]
    [SerializeField] PlayerID player;
    string shooterPlayerId;

    public bool activateShield = false;
    [SerializeField] ParticleSystem shieldParticle;
    void Start()
    {
        //shieldTime = 10f;
        if (player == null)
            player = gameObject.GetComponentInParent<PlayerID>();
        IsDead = false;
        LimitHealth();
        this.InvokeRepeating(nameof(CheckHealthState), 0, 0.5f);
    }

    private void OnEnable()
    {
        if (shieldParticle)
        {
            shieldParticle?.Stop();

        }
        activateShield = false;
    }

    //[ContextMenu("ACTIVE SHIELD")]
    public void OnActivateShield()
    {

        //Debug.Log("<color=Green>aaaaaaaaaaaa::::::</color>Activate Shild");
        shieldParticle.Play();
        activateShield = true;
        //StartCoroutine(OnDeActivateShield());
    }

    public void OnDeActivateShield()
    {
        //yield return new WaitForSeconds(shieldTime);
        shieldParticle.Stop();
        activateShield = false;
    }


    private void LimitHealth()
    {
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        //Debug.Log($"<color=green>Health: {Health}</color>");
    }
    //public static void DoDamage(HelixPlayerHealth health, float damage, Vector3 hitPosition = default(Vector3))
    //{
    //    health.DoDamage(damage, hitPosition, "");
    //}

    public void DoDamage(float damage, Vector3 hitPosition = default(Vector3), string ShooterPlayerId = "")
    {

        if (!activateShield)
        {
            shooterPlayerId = ShooterPlayerId;
            Health -= damage;
            LimitHealth();
            //Debug.Log($"<color=Red>------ Health ------</color>" + Health + "Damage" + damage);
        }
        if (player.isAIPlayer && player.controller.currentSecondaryPowerUp != null && !player.controller.currentSecondaryPowerUp.isUsed && SocketPlayerManager.Instance.MyPlayer.helixPlayerInfo.isRoomHostUser)
        {
            player.controller.currentSecondaryPowerUp.OnStartUsePowerUp(player.playerID, true);
        }
        Invoke(nameof(CheckHealthState), 0.016f);
    }

    public void CheckHealthState()
    {
        LimitHealth();

        if (Health <= 0 && !IsDead)
        {
            Health = 0;
            IsDead = true;
            OnDeath.Invoke();
            //Debug.Log($"<color=Red>------ IsDead ------</color>" + shooterPlayerId);
            /*if (player.isLocalPlayer)
            {
                SocketNetworkManager.Instance.EmitExlodePlayer(player.playerID, shooterPlayerId);
                //TeamScore(player.playerID);
            }*/
            if (player.isLocalPlayer || (player.isAIPlayer && SocketPlayerManager.Instance.MyPlayer.helixPlayerInfo.isRoomHostUser))
            {
                //Debug.Log("aaaa");
                SocketNetworkManager.Instance.EmitExlodePlayer(player.playerID, shooterPlayerId);
                //TeamScore(player.playerID);
            }
        }
        else if (Health > 0)
        {
            IsDead = false;


        }
    }

    void HelixKillData(Data killdata)
    {
        try
        {

            var deadPlayerId = killdata.player_id;
            var killerPlayerID = killdata.opponentPlayerID;

            //Debug.Log($"<color=Red>-- Dead User ID: {deadPlayerId} --</color> || <color=Green>-- Killer User ID: {killerPlayerID} --</color>");

            if (HelixKillInfoObjectPool.Instance != null)
            {

                HelixPlayerInfo _deadPlayerId = null;
                HelixPlayerInfo _killerPlayerID = null;

                _deadPlayerId = SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId];


                _killerPlayerID = SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID];

                var killInfo = HelixKillInfoObjectPool.Instance.GetObjectFromPool(Vector3.zero, Quaternion.identity, killdata);
                killInfo.transform.SetParent(HelixKillInfoObjectPool.Instance.killParent);
                killInfo.transform.localScale = Vector3.one;
                killInfo.deadPlayerUserName.text = _deadPlayerId.userName;
                killInfo.killerPlayerUserName.text = _killerPlayerID.userName;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"HelixPlayerHealth ---> HelixKillData ---> {e.ToString()}");
        }
    }

    public void ExplodeCar(bool responce, Data data)
    {
        if (responce)
        {
            if (player != null)
            {
                Instantiate(ExplosionParticle, transform.position, Quaternion.identity);
                HelixKillData(data);
                GameplayManager.Instance.StartCoroutine(GameplayManager.Instance.RespawnCountDown(player.playerID));
                if (!player.isAIPlayer)
                {
                    GameplayManager.Instance.ReGanretedPlayer(player.playerID);
                    GameplayManager.Instance.playerList.Remove(player);
                    SocketPlayerManager.Instance.allPlayers.Remove(player.playerID);
                    Destroy(gameObject);
                }
                else
                {
                    player.helixPlayerHealth.IsDead = false;
                    player.helixPlayerHealth.Health = 100;
                    player.gameObject.SetActive(false);
                    player.transform.position = GameplayManager.Instance.spawnPoints[UnityEngine.Random.Range(0, GameplayManager.Instance.spawnPoints.Length)].position;
                    player.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.Log($"HelixPlayerHealth ---> ExplodeCar ---> Player is null......");
            }

        }
    }


    public void TeamScore(string player)
    {
        //HelixPlayerInfo _player = SocketNetworkManager.GetPlayerData(player);
        HelixPlayerInfo _player = null /*= SocketNetworkManager.GetPlayerData(player)*/;

        //for (int i = 0; i < SocketPlayerManager.Instance.helixPlayerInfoList.Count; i++)
        //{
        //if(SocketPlayerManager.Instance.helixPlayerInfoList[i].userID==player)
        //{
        _player = SocketPlayerManager.Instance.helixPlayerInfoList[player];
        //}
        //}

        //Debug.Log("::::Increase score::::::" + _player.userName + "::::" + _player.teamA + ":::::::>>>" + _player.teamB);
        if (_player.teamB)
        {
            //Debug.l
            GameplayManager.Instance.IncrementTeamBScore(1);
        }
        else if (_player.teamA)
        {
            GameplayManager.Instance.IncrementTeamAScore(1);
        }
        //else
        //{
        //    Debug.Log("Both False");
        //}
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("OilBarrel"))
        {
            OilBarrel currentBarrel;
            other.TryGetComponent<OilBarrel>(out currentBarrel);
            if (currentBarrel == null)
            {
                other.transform.parent.parent.TryGetComponent<OilBarrel>(out currentBarrel);
            }
            //Debug.Log("::::::>>>");
            if (currentBarrel != null)
                DoDamage(100, Vector3.zero, currentBarrel.shotterId);
            //else
            //{
            //    Debug.Log("current barrel:::" + (currentBarrel==null)+"Name::::>>"+other.name);            
            //}
        }
    }
}
