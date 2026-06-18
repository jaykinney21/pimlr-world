using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class LeaderboardBattleRoyale : MonoBehaviour
{
    public List<PlayerResultDetail> totalTeamPlayer = new List<PlayerResultDetail>();
    public List<PlayerResultDetail> winingTeam = new List<PlayerResultDetail>();
    public List<PlayerResultDetail> loserTeam = new List<PlayerResultDetail>();
    public TextMeshProUGUI winText;
    public TextMeshProUGUI win;
    public TextMeshProUGUI lose;

    public List<HelixPlayerInfo> teamA;
    public List<HelixPlayerInfo> teamB;


    public List<HelixPlayerInfo> getResultList;

    private void OnEnable()
    {
        Debug.Log("Unload");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void TeamAWin(List<HelixPlayerInfo> playerInfoList)
    {
        /*var*/
        teamA = new List<HelixPlayerInfo>();
        /*var*/
        teamB = new List<HelixPlayerInfo>();

        getResultList = playerInfoList;

        for (int i = 0; i < playerInfoList.Count; i++)
        {
            if (playerInfoList[i].teamA && !playerInfoList[i].teamB)
            {
                teamA.Add(playerInfoList[i]);
            }
            else if (!playerInfoList[i].teamA && playerInfoList[i].teamB)
            {
                teamB.Add(playerInfoList[i]);
            }
            else
            {
                if (SocketPlayerManager.Instance.allPlayers.ContainsKey(playerInfoList[i].userID))
                {
                    Debug.Log("<color=Yellow>|||Contain ::" + playerInfoList[i].userName);
                    if (SocketPlayerManager.Instance.allPlayers[playerInfoList[i].userID].helixPlayerInfo.teamA && !SocketPlayerManager.Instance.allPlayers[playerInfoList[i].userID].helixPlayerInfo.teamB)
                    {
                        teamA.Add(playerInfoList[i]);
                    }
                    else if (!SocketPlayerManager.Instance.allPlayers[playerInfoList[i].userID].helixPlayerInfo.teamA && SocketPlayerManager.Instance.allPlayers[playerInfoList[i].userID].helixPlayerInfo.teamB)
                    {
                        teamB.Add(playerInfoList[i]);
                    }
                    else
                    {
                        Debug.Log("<color=Grren>|||========================>NULL::");
                    }
                }
                else
                {
                    Debug.Log("=======>Not contains");
                }
            }
        }

        for (int i = 0; i < teamA.Count; i++)
        {
            var player = teamA[i];
            if (SocketNetworkManager.Instance.playerID == player.userID)
            {
                winingTeam[i].playerRootImg.color = Color.yellow;
            }
            winingTeam[i].playerID = teamA[i].userID;
            winingTeam[i].playerNameInfo.text = $"{teamA[i].userName}<br><size=20>[India] Cyber AI</size>";

            if (SocketPlayerManager.Instance.allPlayers.ContainsKey(winingTeam[i].playerID) && SocketPlayerManager.Instance.allPlayers[winingTeam[i].playerID].isAIPlayer)
            {
                winingTeam[i].playerPoint.text = $"{SocketPlayerManager.Instance.allPlayers[winingTeam[i].playerID].helixPlayerInfo.userKillPoint}<size=35>pts</size>";

            }
            else
                winingTeam[i].playerPoint.text = $"{teamA[i].userKillPoint}<size=35>pts</size>";
        }
        for (int i = 0; i < teamB.Count; i++)
        {

            //Debug.Log("::::" +teamB.Count);
            var player = teamB[i];
            if (SocketNetworkManager.Instance.playerID == player.userID)
            {
                loserTeam[i].playerRootImg.color = Color.yellow;
            }
            loserTeam[i].playerID = teamB[i].userID;
            loserTeam[i].playerNameInfo.text = $"{player.userName}<br><size=20>[India] Cyber AI</size>";
            if (SocketPlayerManager.Instance.allPlayers.ContainsKey(loserTeam[i].playerID) && SocketPlayerManager.Instance.allPlayers[loserTeam[i].playerID].isAIPlayer)
            {
                loserTeam[i].playerPoint.text = $"{SocketPlayerManager.Instance.allPlayers[loserTeam[i].playerID].helixPlayerInfo.userKillPoint}<size=35>pts</size>";

            }
            else
                loserTeam[i].playerPoint.text = $"{player.userKillPoint}<size=35>pts</size>";

        }
    }
    public void TeamBWin(List<HelixPlayerInfo> playerInfoList)
    {
        /* var*/
        teamA = new List<HelixPlayerInfo>();
        /*  var*/
        teamB = new List<HelixPlayerInfo>();

        getResultList = playerInfoList;

        for (int i = 0; i < playerInfoList.Count; i++)
        {
            if (playerInfoList[i].teamA && !playerInfoList[i].teamB)
            {
                teamA.Add(playerInfoList[i]);
            }
            else if (!playerInfoList[i].teamA && playerInfoList[i].teamB)
            {
                teamB.Add(playerInfoList[i]);
            }
            else
            {
                if (SocketPlayerManager.Instance.allPlayers.ContainsKey(playerInfoList[i].userID))
                {
                    Debug.Log("<color=Yellow>|||Contain ::" + playerInfoList[i].userName);
                    if (SocketPlayerManager.Instance.allPlayers[playerInfoList[i].userID].helixPlayerInfo.teamA && !SocketPlayerManager.Instance.allPlayers[playerInfoList[i].userID].helixPlayerInfo.teamB)
                    {
                        teamA.Add(playerInfoList[i]);
                    }
                    else if (!SocketPlayerManager.Instance.allPlayers[playerInfoList[i].userID].helixPlayerInfo.teamA && SocketPlayerManager.Instance.allPlayers[playerInfoList[i].userID].helixPlayerInfo.teamB)
                    {
                        teamB.Add(playerInfoList[i]);
                    }
                    else
                    {
                        Debug.Log("<color=Grren>|||========================>NULL::");
                    }
                }
                else
                {
                    Debug.Log("=======>Not contains");
                }

            }
        }

        for (int i = 0; i < teamB.Count; i++)
        {
            var player = teamB[i];
            if (SocketNetworkManager.Instance.playerID == player.userID)
            {
                winingTeam[i].playerRootImg.color = Color.yellow;
            }
            winingTeam[i].playerID = player.userID;
            winingTeam[i].playerNameInfo.text = $"{player.userName}<br><size=20>[India] Cyber AI</size>";


            if (SocketPlayerManager.Instance.allPlayers.ContainsKey(winingTeam[i].playerID) && SocketPlayerManager.Instance.allPlayers[winingTeam[i].playerID].isAIPlayer)
            {
                winingTeam[i].playerPoint.text = $"{SocketPlayerManager.Instance.allPlayers[winingTeam[i].playerID].helixPlayerInfo.userKillPoint}<size=35>pts</size>";
            }
            else
                winingTeam[i].playerPoint.text = $"{player.userKillPoint}<size=35>pts</size>";
        }
        for (int i = 0; i < teamA.Count; i++)
        {
            var player = teamA[i];
            if (SocketNetworkManager.Instance.playerID == player.userID)
            {
                loserTeam[i].playerRootImg.color = Color.yellow;
            }
            loserTeam[i].playerID = player.userID;
            loserTeam[i].playerNameInfo.text = $"{player.userName}<br><size=20>[India] Cyber AI</size>";


            if(SocketPlayerManager.Instance.allPlayers.ContainsKey(loserTeam[i].playerID) && SocketPlayerManager.Instance.allPlayers[loserTeam[i].playerID].isAIPlayer)
                loserTeam[i].playerPoint.text = $"{SocketPlayerManager.Instance.allPlayers[loserTeam[i].playerID].helixPlayerInfo.userKillPoint}<size=35>pts</size>";
            else
                loserTeam[i].playerPoint.text = $"{player.userKillPoint}<size=35>pts</size>";



        }
    }
}
