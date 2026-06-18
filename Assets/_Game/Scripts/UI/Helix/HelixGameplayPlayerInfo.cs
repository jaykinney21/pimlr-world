using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixGameplayPlayerInfo : MonoBehaviour
{
    public TMPro.TextMeshProUGUI username;
    public TMPro.TextMeshProUGUI userId;
    public TMPro.TextMeshProUGUI UserKillPoint;
    public TMPro.TextMeshProUGUI teamKillPoint;

    private void Start()
    {
        this.InvokeRepeating(nameof(HelixUserInfoSet), 0, 0.5f);
    }
    public void HelixUserInfoSet()
    {
        HelixPlayerInfo _player = SocketNetworkManager.GetPlayerData(SocketNetworkManager.Instance.playerID);

        username.text = $"User Name: {_player.userName}";
        userId.text = $"User ID: {_player.userID}";
        UserKillPoint.text = $"User Kill Point: {_player.userKillPoint}";
        if (SocketPlayerManager.Instance.helixPlayerInfo.teamA == true)
        {
            teamKillPoint.text = $"Team Kill Point: {_player.team_A_KillPoint}";
        }
        if (SocketPlayerManager.Instance.helixPlayerInfo.teamB == true)
        {
            teamKillPoint.text = $"Team Kill Point: {_player.team_B_KillPoint}";
        }

    }
}
