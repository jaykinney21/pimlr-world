using AshVP;
using CarControllerwithShooting;
using System.Collections;
using UnityEngine;

public class ShieldPowerUp : PowerUp
{
    public float shiledTime = 10f;

    public override void OnStartUsePowerUp(string playerID, bool sendToServer)
    {

        base.OnStartUsePowerUp(playerID, sendToServer);

        if (SocketPlayerManager.Instance.allPlayers.ContainsKey(playerID))
            SocketPlayerManager.Instance.allPlayers[playerID].helixPlayerHealth.OnActivateShield();

        if (sendToServer)
        {
            SocketPlayerManager.Instance.StartCoroutine(CompletedShieldTimer(playerID));
        }
    }

    public override void OnCompletedUsePowerUp(string playerID, bool sendToServer)
    {
        base.OnCompletedUsePowerUp(playerID, sendToServer);

        if (SocketPlayerManager.Instance.allPlayers.ContainsKey(playerID))
            SocketPlayerManager.Instance.allPlayers[playerID].helixPlayerHealth.OnDeActivateShield();
    }



    float currentTimer;
    IEnumerator CompletedShieldTimer(string playerID)
    {


        currentTimer = shiledTime;
        if (currentTimer < 10)
            GameCanvas.Instance.secondaryPowerUpValue.text = ("0"+ (int)currentTimer).ToString();
        else
            GameCanvas.Instance.secondaryPowerUpValue.text = ((int)currentTimer).ToString();


        while (currentTimer > 0)
        {
            yield return new WaitForSeconds(1);
            currentTimer--;

            if (SocketPlayerManager.Instance.player_Id == playerID)
            {
                if (currentTimer < 10)
                    GameCanvas.Instance.secondaryPowerUpValue.text = ("0" + (int)currentTimer).ToString();
                else
                    GameCanvas.Instance.secondaryPowerUpValue.text = ((int)currentTimer).ToString();
            }

        }


        
        OnCompletedUsePowerUp(playerID, true);
    }

  

}
