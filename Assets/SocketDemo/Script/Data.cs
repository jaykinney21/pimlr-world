using BestHTTP.SocketIO3;
using System;
using System.Collections.Generic;

[Serializable]
public class Data
{


    //User Login data
    //public bool isLocalUser;
    //public string userID;
    //public string userName;
    public string userRegion;
    //public string roomID;
    //public string roomName;
    //public string roomPassword;
    //public bool isRoomHostUser;
    //public string roomType;
    //public int roomMaxPlayer;
    //public int roomCurrentPlayer;
    //public bool teamA, teamB;
    //public bool userReadyRoom, userStartRoom;

    public string region { get ; set; }
    public string player_name { get; set; }
    public string id { get; set; }
    public string room_name { get; set; }
    public string room_id { get; set; }
    public string roomID { get; set ; }
    public int max_players { get; set; }
    public int current_players { get; set; }
    public string player_id { get; set; }
    public bool isMaster { get; set; }
    public string left_player_id { get; set; }
    public string newOwnerId { get; set; }
    public bool isReady { get; set; }
    public bool started { get; set; }
    public string teamname { get; set; }
    public bool team_A { get; set; }
    public bool team_B { get; set; }

    public float dy { get; set; }
    public float dx { get; set; }
    public float dz { get; set; }
    public float rx { get; set; }
    public float ry { get; set; }
    public float rz { get; set; }
    public float force { get; set; }
    public string fire { get; set; }
    public string opponent_player_id { get; set; }
    public string health { get; set; }
    public int duration { get; set; }
    public int count { get; set; }
    public int point { get; set; }
    public int team_A_Points { get; set; }
    public int team_B_Points { get; set; }

    public string mapname { get; set; }
    public string message { get; set; }

    public int leaderBordScore { get; set; }
    public int playerScore { get; set; }
    public string localPlayerID { get; set; }
    public string opponentPlayerID { get; set; }

    public string ShooterPlayerId { get; set; }
    public string deadPlayerId { get; set; }
    public string killerPlayerID { get; set; }


}


class ErrorData
{
    public int code;
    public string content;
}

// Error already defines the message property
class CustomError : Error
{
    public ErrorData data;

    public override string ToString()
    {
        return $"[CustomError {message}, {data?.code}, {data?.content}]";
    }
}




public class JOINED_LOBBY_SUCCESS
{

    public bool success { get; set; }
    public Data data { get; set; }

}

public class ROOM_LIST_UPDATE
{
    public bool success { get; set; }
    public List<Data> data { get; set; }
   

}

public class CREATE_ROOM_SUCCESS
{
    public bool success { get; set; }
    public Data data { get; set; }
}
public class CREATE_ROOM_FAILED
{
    public bool success { get; set; }
    public string message { get; set; }
    public string code { get; set; }
}
public class PLAYER_JOINED
{
    public bool success { get; set; }
    public Data data { get; set; }
}
public class JOIN_ROOM_FAILED
{
    public bool success { get; set; }
    public string message { get; set; }
    public string code { get; set; }
}

[Serializable]
public class PLAYER_LIST_UPDATE
{
    public bool success { get; set ; }
    public List<Data> data;//{get; set; }
     //public List<HelixPlayerInfo> helixPlayerInfoList = new List<HelixPlayerInfo>();
    //public List<HelixPlayerInfo> helixPlayerData;
}

public class POWERUP_DATA
{
    //public bool success { get; set; }

    public string powerUpID { get; set; }

    public string player_ID { get; set;}

    public bool isStartUse { get; set; }

    public bool isCompleteUse { get; set;}
}

public class PLAYER_LEFT
{
    public bool success { get; set; }
    public Data data { get; set; }
}
public class PLAYER_READY
{
    public bool success { get; set; }
    public Data data { get; set; }
}
public class PLAYER_READY_FAILED
{
    public bool success { get; set; }
    public string message { get; set; }
    public string code { get; set; }
}
public class START_GAME_NOW
{
    public bool success { get; set; }
    public Data data { get; set; }
}
public class START_GAME_FAILED
{
    public bool success { get; set; }
    public string message { get; set; }
    public string code { get; set; }
}

public class ON_TEAM_SELECTION
{
    public bool success { get; set; }
    public Data data { get; set; }
}

public class ON_POS_AND_ROT
{
    public bool success { get; set; }
    public Data data { get; set; }
}

public class ON_PLAYER_FIRE
{
    public bool success { get; set; }
    public Data data { get; set; }
}


public class ON_PLAYER_EXPLODE
{
    public bool success { get; set; }
    public Data data { get; set; }
}

public class ON_PLAYER_HEALTH
{
    public  bool success { get;set; }
    public Data data { get; set; }
}

public class ON_PLAYER_LEADERBORD_INFO
{
    public bool success { get; set; }
    public Data data { get; set; }
}

public class ON_TIMER
{
    public bool success { get; set;}

    public Data data { get; set; }
}

public class ON_PLAYER_DESTROYED
{
    public bool success { get; set; }

    public Data data { get; set; }
}

public class ON_PONG
{
    public bool success { get; set; }

    public Data data { get; set; }
}

public class ON_USER_LOGIN
{
    public bool success { get; set; }

    public Data data { get; set; }
}
