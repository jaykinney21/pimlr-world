
using FORGE3D;
using System.Collections;
using UnityEngine;

public class VulcanMachineGunPowerUp : PowerUp
{
   

     [SerializeField]F3DFXType currentBulleteType=F3DFXType.Vulcan;
    [SerializeField] float DamagePower = 5;

    public override void OnCollectBox(PlayerID playerid, bool sendToServer = false)
    {
        base.OnCollectBox(playerid, sendToServer);

        if (playerid != null)
            SocketPlayerManager.Instance.StartCoroutine(playerid.SetGun(true, currentBulleteType, DamagePower, activateTime));
    }

 

    public override void OnCompletedUsePowerUp(string playerID, bool sendToServer)
    {
        base.OnCompletedUsePowerUp(playerID, sendToServer);

        if (SocketPlayerManager.Instance.allPlayers.ContainsKey(playerID))
            SocketPlayerManager.Instance.StartCoroutine(SocketPlayerManager.Instance.allPlayers[playerID].SetGun(false,F3DFXType.Vulcan, DamagePower, activateTime));
    }

  


}
