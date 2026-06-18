using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Helix_Your_Team : MonoBehaviour
{
    public static Helix_Your_Team Instance { get; private set; }
    public Button startButton, ReadyButton, npcButton;
    public Sprite npcOn, npcOff;
    public Image npcButtonImg;
    bool npc = false;
    public Transform teamA, teamB;
    public TeamPlayerDetaile teamAPlayer;
    public TeamPlayerDetaile teamBPlayer;

    [Header("Team List")]
    public List<TeamPlayerDetaile> team_AList = new List<TeamPlayerDetaile>();
    public List<TeamPlayerDetaile> team_BList = new List<TeamPlayerDetaile>();
    public List<TeamPlayerDetaile> teamsPlayer = new List<TeamPlayerDetaile>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }
    public void OnStartBtnClick()
    {
#if !UNITY_EDITOR
        PlayerPrefs.SetInt("trialRound", 1);
#endif
        SocketNetworkManager.Instance.StartGame(SocketUIManager.Instance.helixUIManager.mapSelection.mapName);
    }
    public void OnReadyBtnClick()
    {
        try
        {
            //Debug.Log("Is Ready Player");
            SocketNetworkManager.Instance.ReadyPlayer(true);
            SocketUIManager.Instance.SetScreen(SocketScreens.Helix_PreGame);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketLobbyUIManager ---> OnReadyBtnClick ---> {e.ToString()}");
        }

    }

    public void On_Helix_Your_Team_PopUp_Btn_Click()
    {
        try
        {
            SocketNetworkManager.Instance.Team_Select(SocketNetworkManager.Instance.playerID, string.Empty, false, false);
            SocketUIManager.Instance.SetScreen(SocketScreens.teampanel);
        }
        catch (System.Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnRoomList ---> {e}");
        }

    }

    public void OnNpcSpawnActive()
    {
        try
        {
            // Assuming npc is a boolean variable
            if (!npc)
            {
                // If npc is currently false, set it to true
                npc = true;
                npcButtonImg.sprite = npcOn;
                //HelixNpcCarManager.Instance.NpcStart();
                Debug.Log("RoomID: " + SocketPlayerManager.Instance.helixPlayerInfo.roomID);
                Debug.Log("RoomName: " + SocketPlayerManager.Instance.helixPlayerInfo.roomName);
                /*for (int i = 0; i < 9; i++)
                {
                    HelixNpcCarManager.Instance.NpcStart();
                }*/

                //HelixNpcCarManager.Instance.JoinRoom(SocketPlayerManager.Instance.helixPlayerInfo.roomID, SocketPlayerManager.Instance.helixPlayerInfo.roomName);
            }
            else
            {
                // If npc is currently true, set it to false
                npc = false;

                npcButtonImg.sprite = npcOff;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Helix_Your_Team ---> OnNpcSpawnActive ---> {e.ToString()}");
        }



    }


}
