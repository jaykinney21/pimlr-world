using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixPlayer : MonoBehaviour
{
    public bool isLocalUser;
    public string userID;
    public string userName;
    public string userRegion;
    public string roomID;
    public string roomName;
    public string roomPassword;
    public bool isRoomHostUser;
    public string roomType;
    public int roomMaxPlayer;
    public int roomTotalPlayer;
    public bool teamA, teamB;
    public bool userReadyRoom, userStartRoom;
    public float maxHeath;
    public float currentHeath;
}
