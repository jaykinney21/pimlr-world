using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelixSocketStaticData 
{
    #region ON EVENTS
    public static readonly string ON_PONG = "PONG";//Receive Ping Pong
    public static readonly string ON_USER_LOGIN = "ON_USER_LOGIN";//Game Start to Receive Data
    public static readonly string ON_GLOBAL_LOBBY_JOIN = "ON_GLOBAL_LOBBY_JOIN";
    public static readonly string ON_CREATE_ROOM_SUCCESS = "ON_CREATE_ROOM_SUCCESS";
    public static readonly string ON_JOIN_ROOM_SUCCESS = "ON_JOIN_ROOM_SUCCESS";
    public static readonly string ON_UPDATEROOMS = "ON_UPDATEROOMS";
    #endregion

    #region Emit EVENTS
    public static readonly string PING = "PING";//Send Ping Pong
    public static readonly string USER_LOGIN = "USER_LOGIN";//Send Game Start
    public static readonly string GLOBAL_LOBBY_JOIN = "GLOBAL_LOBBY_JOIN";
    public static readonly string CREATE_ROOM = "CREATE_ROOM";
    public static readonly string JOIN_ROOM = "JOIN_ROOM";
    public static readonly string UPDATEROOMS = "UPDATEROOMS";
    #endregion

    #region User Data
    public class HelixData
    {
        public bool isLocalUser { get; set; }
        public string userID { get; set; }
        public string userName { get; set; }
        public string userRegion { get; set; }
        public string roomID { get; set; }
        public string roomName { get; set; }
        public string roomPassword { get; set; }
        public bool isRoomHostUser { get; set; }
        public string roomType { get; set; }
        public int roomMaxPlayer { get; set; }
        public int roomTotalPlayer { get; set; }
        public bool teamA { get; set; }
        public bool teamB { get; set; }
        public bool userReadyRoom { get; set; }
        public bool userStartRoom { get; set; }

        //public string playerName;
        //public string playerId;
        //public string region;
        //public string message { get; set; }
        //public string roomId { get; set; }
        //public string roomName { get; set; }
        //public bool isPrivate { get; set; }
        //public bool isVisible { get; set; }
        //public int maxPlayers { get; set; }
        //public int totalPlayers { get; set; }
        //public string hostPlayerName { get; set; }
        //public string password { get; set; }
        //public int totalRoom;
    }
    #endregion

    public class GetFailedData
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }
    public class GetDataList
    {
        public bool success { get; set; }
        public List<Data> data { get; set; }
    }
    public class GetData
    {
        public bool success { get; set; }
        public Data data { get; set; }
    }
    public class GetAllPlayerData
    {
        public bool success { get; set; }
        public List<players> players { get; set; }

    }
    public class ErrorData
    {
        public int code;
        public string content;
    }
    public class players
    {
        public string playerName { get; set; }
        public string playerId { get; set; }

    }
}
