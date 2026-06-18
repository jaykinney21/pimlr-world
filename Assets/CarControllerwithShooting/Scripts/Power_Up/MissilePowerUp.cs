using System.Collections;
using UnityEngine;

public class MissilePowerUp : PowerUp
{

    public override void OnCollectBox(PlayerID playerid, bool sendToServer = false)
    {
        base.OnCollectBox(playerid, sendToServer);


        missilePowerUp = this;

        playerid.gunController.Weapon_MissileLeft.SetActive(true);
        playerid.gunController.Weapon_MissileRight.SetActive(true);

        //if (playerid != null)
        //    SocketPlayerManager.Instance.StartCoroutine(playerid.SetGun(true, currentBulleteType, DamagePower, activateTime));
    }


    public override void OnStartUsePowerUp(string playerID, bool sendToServer)
    {
        base.OnStartUsePowerUp(playerID, false);
        //Debug.Log("=====================>aaaaaaaaaaaa::::::Activate Shild");
        if (SocketPlayerManager.Instance.allPlayers.ContainsKey(playerID))
        {
            if (SocketPlayerManager.Instance.allPlayers[playerID].isAIPlayer)
                SocketPlayerManager.Instance.allPlayers[playerID].gunControllerinAi.Fire_Missile(true);
        }
    }


    public override void OnCompletedUsePowerUp(string playerID, bool sendToServer)
    {
        if (playerID == SocketPlayerManager.Instance.player_Id)
        {
            missilePowerUp = null;
        }
        base.OnCompletedUsePowerUp(playerID, sendToServer);
        //if (SocketPlayerManager.Instance.allPlayers.ContainsKey(playerID))
        //{
        //    SocketPlayerManager.Instance.allPlayers[playerID].gunController.Weapon_MissileLeft.SetActive(false);
        //    SocketPlayerManager.Instance.allPlayers[playerID].gunController.Weapon_MissileRight.SetActive(false);
        //}
    }


}
