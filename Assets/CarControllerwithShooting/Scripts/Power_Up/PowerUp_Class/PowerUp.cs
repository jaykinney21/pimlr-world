using CarControllerwithShooting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public string powerUpID;


    public bool isSecondaryPowerUp;

    internal MissilePowerUp missilePowerUp;

    [Header("Assing Use Time/Total Ammo")]
    [SerializeField] internal float activateTime = 10;

    public Sprite powerUpImage;



    public bool isUsed = false;

    private void OnEnable()
    {
        missilePowerUp = null;
    }
    private void Start()
    {
        GameplayManager.Instance.allPowerUps.Add(powerUpID, this);
    }

    public virtual IEnumerator ActivatepowerUp()
    {
        yield return new WaitForSeconds(3);
        if (this?.gameObject)
            this?.gameObject?.SetActive(true);
    }

    private void OnDestroy()
    {
        //  StopCoroutine(ActivatepowerUp());
    }

    public virtual void DeActivatePowerup()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void OnCollectBox(PlayerID currentPlayer, bool sendToServer = false)
    {
        if (sendToServer)
        {
            SocketNetworkManager.Instance.EmitCollectPowerUp(powerUpID, currentPlayer.playerID);
        }

        if(currentPlayer && currentPlayer.isLocalPlayer)
        {

            if (isSecondaryPowerUp)
            {
                if (activateTime < 10)
                    GameCanvas.Instance.secondaryPowerUpValue.text = ("0" + (int)activateTime).ToString();
                else
                    GameCanvas.Instance.secondaryPowerUpValue.text = ((int)activateTime).ToString();
            }
            else
            {
                GameCanvas.Instance.crossHair.SetActive(true);
            }
            //else
            //    GameCanvas.Instance.powerUpValue.text = activateTime.ToString();
        }


       
        if (currentPlayer)
        {
            if (!isSecondaryPowerUp)
            {
                currentPlayer.controller.currentPowerUp = this;

                currentPlayer.gunController.Weapon_MachineGun.gameObject.SetActive(false);
                currentPlayer.gunController.fire = false;
            }
            else
                currentPlayer.controller.currentSecondaryPowerUp = this;
        }
        DeActivatePowerup();

    }

    public virtual void OnStartUsePowerUp(string playerID, bool sendToServer)
    {
        //Debug.Log("=====================>aaaaaaaaaaaa::::::Activate Shild");
        if (isUsed == false)
        {
            if (sendToServer)
            {
                SocketNetworkManager.Instance.EmitUsePrimaryPowerUp(playerID, powerUpID, true, false);
            }
            isUsed = true;
        }
    }

    CarController currentPlayer;
    public virtual void OnCompletedUsePowerUp(string playerID, bool sendToServer)
    {

        isUsed = false;
        if (SocketPlayerManager.Instance.allPlayers.ContainsKey(playerID))
        {
            currentPlayer = SocketPlayerManager.Instance.allPlayers[playerID].controller;
            if (currentPlayer.currentPowerUp && currentPlayer.currentPowerUp.powerUpID == powerUpID)
            {
                currentPlayer.currentPowerUp = null;
                currentPlayer.playerid.gunController.Weapon_MachineGun.gameObject.SetActive(true);
            }
            if (currentPlayer.currentSecondaryPowerUp && currentPlayer.currentSecondaryPowerUp.powerUpID == powerUpID)
            {
                currentPlayer.currentSecondaryPowerUp = null;
            }
            if (currentPlayer.playerid.isLocalPlayer)
            {
                if (isSecondaryPowerUp)
                {
                    GameCanvas.Instance.secondaryPowerUpValue.text = "00";
                    GameCanvas.Instance.secondarypowerUpImage.enabled = false;

                }
                else
                {
                    GameCanvas.Instance.powerUpImage.enabled = false;
                    GameCanvas.Instance.powerUpValue.text = "00";
                    //GameCanvas.Instance.crossHair.SetActive(false);
                }
            }
        }
        if (sendToServer)
        {
            SocketNetworkManager.Instance.EmitUsePrimaryPowerUp(playerID, powerUpID, false, true);
        }

        if (GameCanvas.Instance)
            GameCanvas.Instance.StartCoroutine(ActivatepowerUp());
    }

}
