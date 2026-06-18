using UnityEngine;
using UnityEngine.UI;

public class RoomNamePrefabScript : MonoBehaviour
{

    public string RoomId;
    public TMPro.TMP_Text RoomIndex;
    public TMPro.TMP_Text RoomName;
    public TMPro.TMP_Text Available_Player;
    public UnityEngine.UI.Image RoomImage;
    public Button RoomPrefabButton;
    public string playerId;

    public void SetData(string roomName, string RoomID, int roomPlayerCount,int roomcount)
    {
        RoomIndex.text = $"{roomcount}.";
        RoomName.text = roomName;
        RoomId = RoomID;
        Available_Player.text = $"Available : <color=red>{roomPlayerCount}</color> / <color=green>10</color>";
    }
    private void OnEnable()
    {
        RoomPrefabButton.interactable = true;
    }
    public void onClick()
    {
        //Debug.Log("RoomName = " + RoomName.text);
        SocketNetworkManager.Instance.JoinRoom(RoomName.text);
        SocketUIManager.Instance.SetScreen(SocketScreens.teampanel);
        RoomPrefabButton.interactable = false;
    }

}
