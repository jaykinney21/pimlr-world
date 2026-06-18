using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocketLobbyUiManager : MonoBehaviour
{
    public TMPro.TMP_InputField RoomName;
    public Button CreateRoomBtn, joinButton;
    public Button Readybtn, StartBtn;
    public Transform RoomnameListParant;
    public Transform PlayerListParant;
    private bool isready = false;

    [Header("List Prefab")]
    public RoomNamePrefabScript roomNamePrefab;
    public PlayerListPlayerPrefab PLayerListPrefab;
    public TeamPlayerListPrefab teamPlayerListPrefab;
    [Header("List")]
    public List<RoomNamePrefabScript> roomNameList = new List<RoomNamePrefabScript>();
    public List<PlayerListPlayerPrefab> PlayerList = new List<PlayerListPlayerPrefab>();

    //changes by abhay 
    public Transform teamPlayerListPrefabTransform;
    public Helix_Team_Selection helix_Team_Selection;
    public Helix_Your_Team helix_Your_Team;

    private void Start()
    {
        joinButton.interactable = true;
        CreateRoomBtn.interactable = true;
        Readybtn?.gameObject.SetActive(false);
        StartBtn?.gameObject.SetActive(false);
        if (IsInCreateRoomScreen())
        {

            CreateRoomBtn.interactable = false;
        }
        if (IsInJoinRoomScreen())
        {

            joinButton.interactable = false;
        }
    }

    private void OnEnable()
    {
        if (IsInCreateRoomScreen() )
        {

            CreateRoomBtn.interactable = !string.IsNullOrWhiteSpace(RoomName.text); ;
        }
        if (IsInJoinRoomScreen())
        {
            joinButton.interactable = !string.IsNullOrWhiteSpace(RoomName.text); 
        }

    }
    #region CreatRoom-InputFiled-Button
    public void UpdateCreateRoomButtonInteractivity()
    {
        if (IsInCreateRoomScreen())
        {
            CreateRoomBtn.interactable = !string.IsNullOrWhiteSpace(RoomName.text);
        }
        else
        {
            CreateRoomBtn.interactable = false;
        }
    }
    private bool IsInCreateRoomScreen()
    {
        var currentScreen = this.gameObject;
        return currentScreen != null && currentScreen.name.Equals("Helix_Create_Room") && currentScreen.activeSelf;
    }
    #endregion

    #region JionRoom-InputFiled-Button
    public void UpdateJoinRoomButtonInteractivity(string roomName)
    {
        if (IsInJoinRoomScreen())
        {
            joinButton.interactable = !string.IsNullOrWhiteSpace(roomName);
        }
        else
        {
            joinButton.interactable = false;
        }
    }
    private bool IsInJoinRoomScreen()
    {
        var currentScreen =this.gameObject;
        
        return currentScreen != null && currentScreen.name.Equals("Helix_Join_Room") && currentScreen.activeSelf;
    }
    #endregion

    public void GenerateRoomList(ROOM_LIST_UPDATE data)
    {

        for (int i = 0; i < roomNameList.Count; i++)
        {
            if (roomNameList[i] != null)
                DestroyImmediate(roomNameList[i].gameObject);
        }
        roomNameList.Clear();

        for (int i = 0; i < data.data.Count; i++)
        {
            RoomNamePrefabScript room = Instantiate(roomNamePrefab, RoomnameListParant);
            //Debug.Log(data.data[i].count);
            room.SetData(data.data[i].room_name, data.data[i].room_id, data.data[i].current_players, i + 1);
            roomNameList.Add(room);
        }

    }
    public void GeneratePlayerList(PLAYER_LIST_UPDATE data)
    {
        for (int i = 0; i < helix_Team_Selection.PlayerList.Count; i++)
        {
            if (helix_Team_Selection.PlayerList[i] != null)
            {
                Destroy(helix_Team_Selection.PlayerList[i].gameObject);
            }

        }
        helix_Team_Selection.PlayerList.Clear();
        for (int i = 0; i < data.data.Count; i++)
        {
            TeamPlayerListPrefab player = Instantiate(teamPlayerListPrefab, teamPlayerListPrefabTransform);
            player.SetDetail(data.data[i].player_name, data.data[i].player_id, i + 1);
            helix_Team_Selection.PlayerList.Add(player);
        }

    }

    public void GenerateTeamAPlayerList(PLAYER_LIST_UPDATE data)
    {
        for (int i = 0; i < helix_Your_Team.team_AList.Count; i++)
        {
            if (helix_Your_Team.team_AList[i] != null)
            {
                Destroy(helix_Your_Team.team_AList[i].gameObject);
            }

        }
        helix_Your_Team.team_AList.Clear();

        for (int i = 0; i < data.data.Count; i++)
        {
            if (data.data[i].team_A)
            {
                TeamPlayerDetaile player = Instantiate(helix_Your_Team.teamAPlayer, helix_Your_Team.teamA);
                player.SetPlayerDetail(data.data[i].player_name, data.data[i].player_id, i + 1, 0);
                helix_Your_Team.team_AList.Add(player);
            }

        }
    }
    public void GenerateTeamBPlayerList(PLAYER_LIST_UPDATE data)
    {

        for (int i = 0; i < helix_Your_Team.team_BList.Count; i++)
        {
            if (helix_Your_Team.team_BList[i] != null)
            {
                Destroy(helix_Your_Team.team_BList[i].gameObject);
            }
        }
        helix_Your_Team.team_BList.Clear();

        for (int i = 0; i < data.data.Count; i++)
        {
            if (data.data[i].team_B)
            {
                TeamPlayerDetaile player = Instantiate(helix_Your_Team.teamBPlayer, helix_Your_Team.teamB);
                player.SetPlayerDetail(data.data[i].player_name, data.data[i].player_id, i + 1, 0);
                helix_Your_Team.team_BList.Add(player);
            }
        }
    }

    public void CheckIsPlayerReady(PLAYER_LIST_UPDATE data)
    {
        for (int i = 0; i < data.data.Count; i++)
        {
            if (data.data[i].team_A)
            {
                for (int j = 0; j < helix_Your_Team.team_AList.Count; j++)
                {
                    if (data.data[i].player_id == helix_Your_Team.team_AList[j].playerId)
                    {
                        if (data.data[i].isReady)
                        {
                            //Debug.Log("A :"+data.data[i].isReady);
                            helix_Your_Team.team_AList[j].PlayerReady_Icon.color = Color.green;
                        }
                        else
                        {
                            helix_Your_Team.team_AList[j].PlayerReady_Icon.color = Color.white;
                        }
                    }
                }
            }
            else if (data.data[i].team_B)
            {
                for (int j = 0; j < helix_Your_Team.team_BList.Count; j++)
                {
                    if (data.data[i].player_id == helix_Your_Team.team_BList[j].playerId)
                    {
                        if (data.data[i].isReady)
                        {
                            //Debug.Log("B :"+data.data[i].isReady);
                            helix_Your_Team.team_BList[j].PlayerReady_Icon.color = Color.green;
                        }
                        else
                        {
                            helix_Your_Team.team_BList[j].PlayerReady_Icon.color = Color.white;
                        }
                    }

                }
            }

        }

    }

    public void OnPlayerReady(string playerID, bool isReady)
    {
        try
        {
            var playerList = Helix_Team_Selection.Instance.PlayerList;

            var teamsPlayer = Helix_Your_Team.Instance?.teamsPlayer;

            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].playerId.Equals(playerID))
                {

                    if (teamsPlayer != null && i < teamsPlayer.Count && teamsPlayer[i])
                    {
                        if (isReady)
                            teamsPlayer[i].PlayerReady_Icon.color = Color.green;
                        else
                            teamsPlayer[i].PlayerReady_Icon.color = Color.white;
                    }
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log("SocketLobbyUiManager:::OnPlayerReady:::" + e.ToString());
        }
    }
    public void SelfPlayerLeaveRoom()
    {
        try
        {
            for (int i = 0; i < PlayerList.Count; i++)
            {
                Destroy(PlayerList[i].gameObject);
            }
            PlayerList.Clear();
            SocketPlayerManager.Instance.helixPlayerInfoList.Clear();// clear the local database

            Start();
            SocketPlayerManager.Instance.LeaveRoom();
        }
        catch (Exception e)
        {
            Debug.Log("SocketLobbyUiManager:::SelfPlayerLeaveRoom:::" + e.ToString());
        }

    }

    public void OnCreateRoomClick()
    {
        if (RoomName.text == "")
        {
            return;
        }
        else
        {
            SocketNetworkManager.Instance.CreateRoom(RoomName.text, SocketPlayerManager.Instance.MaxPlayer);
            SocketUIManager.Instance.SetScreen(SocketScreens.teampanel);
        }
    }

    public void OnJoinRoomClick()
    {
        if (RoomName.text == "" && RoomName.text == string.Empty)
        {
            return;
        }
        SocketNetworkManager.Instance.JoinRoom(RoomName.text);
        SocketUIManager.Instance.SetScreen(SocketScreens.teampanel);
    }

    public void ShowReadyBtn()
    {

        Debug.Log("Show Ready Btn");
        Readybtn?.gameObject.SetActive(true);
    }

    public void ShowStartBtn()
    {
        Debug.Log("Show Start Btn");
        helix_Your_Team.ReadyButton.gameObject.SetActive(false);
        StartBtn?.gameObject.SetActive(true);
        helix_Your_Team.startButton.gameObject.SetActive(true);
        SocketUIManager.Instance.SetScreen(SocketScreens.MapSelectionPanel);
    }

    public void OnReadyBtnClick()
    {
        try
        {
            SocketNetworkManager.Instance.ReadyPlayer(isready);
            SocketUIManager.Instance.SetScreen(SocketScreens.Helix_PreGame);
            Debug.Log("Is Ready Player");
        }
        catch (Exception e)
        {
            Debug.Log($"SocketLobbyUIManager ---> OnReadyBtnClick ---> {e.ToString()}");
        }

    }

    public void StartBtnClick()
    {
        SocketNetworkManager.Instance.StartGame(SocketUIManager.Instance.helixUIManager.mapSelection.mapName);
    }

    #region HELIX-ROOMS-PANELS
    public void OnHelixRoomLeaveBtnClick()
    {
        SocketNetworkManager.Instance.LeaveRoom();
        SocketUIManager.Instance.SetScreen(SocketScreens.Helix_mainMenu);
    }
    #endregion
    #region HELIX-CREATE-PANELS
    public void On_Helix_Creat_And_Join_Room_PopUp_Btn_Click()
    {
        SocketNetworkManager.Instance.LeaveRoom();
        SocketUIManager.Instance.SetScreen(SocketScreens.Lobby);
    }
    #endregion

    public void OnClickRoomCreatePopUpOn()
    {
        SocketUIManager.Instance.SetScreen(SocketScreens.Helix_Create_Room);
    }
}

