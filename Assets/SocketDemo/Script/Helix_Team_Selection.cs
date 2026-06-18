using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Helix_Team_Selection : MonoBehaviour
{
    //public SocketUIManager socketUIManager;
    public static Helix_Team_Selection Instance { get; private set; }
    public TeamPlayerListPrefab roomNamePrefab;
    public Transform RoomnameListParant;
    public List<TeamPlayerListPrefab> PlayerList = new List<TeamPlayerListPrefab>();
    // public Button teamA, teamB;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);

    }
    public void OnRemoveRoomFromList(string roomId, string roomName, bool playerLeaving)
    {
        // Call this when a player leaves a room
        //SocketDemoManager.Instance?.UIManager?.LobbyPanal?.OnRemoveRoomFromList(roomlist[i].RoomId, roomlist[i].RoomName.ToString(), true);
    }
    public void On_Helix_Team_Selection_Leave_Btn_Click()
    {
        SocketNetworkManager.Instance.LeaveRoom();
        SocketUIManager.Instance.SetScreen(SocketScreens.Lobby);
        //SocketNetworkManager.Instance.LeaveRoom();
    }
}
