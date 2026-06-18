//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using BestHTTP.SocketIO3;
//using System.Collections;

//public class HelixSocketManager : MonoBehaviour
//{
//    /// <summary>
//    /// BestHTTP.SocketIO3
//    /// </summary>
//    public SocketManager manager;
//    public Socket socket;

//    [Header("Set the interval in seconds")]
//    public float pingInterval = 5f;

//    public static HelixSocketManager Instance { get; private set; }
//    [Header("This URL By Node Backed Server")]
//    [SerializeField]
//    private string Url = "https://effa-2405-201-200b-c05b-497e-34ed-9c44-fcc3.ngrok-free.app";

//    //My Player Info
//    public HelixPlayer myPlayer;


//    internal Dictionary<string, HelixPlayer> networkPlayers = new Dictionary<string, HelixPlayer>(); //put player script in place of int 

//    public Dictionary<string, HelixPlayer> allPlayers = new Dictionary<string, HelixPlayer>(); //put player script in place of int 

//    #region Awake
//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            //Don'tDestroyOnLoad(this.gameObject);
//        }
//        else Destroy(this.gameObject);
//    }
//    #endregion

//    #region Start
//    private void Start()
//    {

//        Url = Url + "/socket.io/";
//        Debug.Log("Start : " + Url);

//        try
//        {
//            if (Url.Equals(""))
//            {
//                Debug.LogError(" NO URL : Please Enter URL");
//                return;
//            }
//            BestHTTP.HTTPManager.MaxConnectionIdleTime = TimeSpan.FromSeconds(120);
//            BestHTTP.HTTPManager.UseAlternateSSLDefaultValue = false;
//            var options = new BestHTTP.SocketIO3.SocketOptions
//            {
//                //options.AutoConnect = false;
//                QueryParamsOnlyForHandshake = false,
//                ConnectWith = BestHTTP.SocketIO3.Transports.TransportTypes.WebSocket
//            };
//            Url += "/socket.io/";
//            Debug.Log("Url = " + Url);
//            manager = new BestHTTP.SocketIO3.SocketManager(new Uri(Url), options);
//            //manager.Parser = new MsgPackParser();
//            //Debug.Log("URl " + manager.Uri);
//            socket = manager.Socket;
//            socket.On<BestHTTP.SocketIO3.Events.ConnectResponse>(SocketIOEventTypes.Connect, OnConnected);
//            socket.On(BestHTTP.SocketIO3.SocketIOEventTypes.Disconnect, OnDisconnected);
//            socket.On<ErrorData>(BestHTTP.SocketIO3.SocketIOEventTypes.Error, OnError);
//            socket.On<GetData>(SocketStaticData.ON_PONG, OnPong);
//            socket.On<GetData>(SocketStaticData.ON_START_GAME, OnStartGame);
//            //socket.On<GetAllPlayerData>(SocketStaticData.ON_GLOBAL_LOBBY_JOIN, OnGlobalLobbyJoin);
//            //socket.On<GetData>(SocketStaticData.ON_CREATE_ROOM_SUCCESS, OnCreateRoomSuccess);
//            //socket.On<GetData>(SocketStaticData.ON_JOIN_ROOM_SUCCESS, OnJoinRoomSuccess);





//            Debug.Log($"<b><color=cyan>Connecting...</color></b>");
//            manager.Open();
//        }
//        catch (Exception e)
//        {
//            Debug.LogError(string.Format("Connection Error: {0}", e));
//            throw;
//        }
//    }
//    #endregion

//    #region Connected || Disconnected 
//    private void OnConnected(BestHTTP.SocketIO3.Events.ConnectResponse response)
//    {
//        Debug.Log($"<b><color=green>Connected: {response.sid} </color></b>");
//        //Invoke(nameof(Ping), .5f);
//        StartCoroutine(SendPingRoutine());
//    }
//    private void OnDisconnected()
//    {
//        //PlayerManager.Instance.playerList.Remove(myPlayer);
//        Debug.Log($"<b><color=red>Disconnected from the server.</color></b>");
//    }
//    #endregion

//    #region Disconnect from the server when the script is destroyed or the game exits
//    // Disconnect from the server when the script is destroyed or the game exits
//    private void OnDestroy()
//    {
//        manager.Close();
//    }
//    #endregion

//    #region Socket Errors
//    void OnError(ErrorData args)
//    {
//        if (args != null)
//        {
//            Debug.LogError(string.Format("Socket.IO Error: {0}", args.content));
//        }
//    }
//    #endregion

//    #region PingPong
//    //SendPingRoutine
//    IEnumerator SendPingRoutine()
//    {
//        while (socket.IsOpen)
//        {
//            Ping();
//            yield return new WaitForSeconds(pingInterval);
//        }
//    }
//    //SendPing
//    public void Ping()
//    {
//        socket.Emit(SocketStaticData.PING, string.Empty);
//    }
//    //Received pong
//    private void OnPong(GetData data)
//    {
//        Debug.Log("<color=yellow>Ping:Pong</color>");
//    }
//    #endregion

//    #region Game Start To Create [uuId]
//    public void StartGame(string playerName)
//    {
//        SimpleJSON.JSONNode playerInfoJson = new SimpleJSON.JSONObject
//        {
//            ["playerName"] = playerName,
//            ["region"] = "India",
//        };
//        Debug.Log("Player Data:" + playerInfoJson.ToString());
//        socket.Emit(SocketStaticData.START_GAME, playerInfoJson.ToString());
//    }

//    private void OnStartGame(GetData data)
//    {
//        if (!myPlayer)
//        {
//           /* PlayerManager playerManager = PlayerManager.Instance;
//            myPlayer = Instantiate(playerManager.playerPrefab, playerManager.playerParent);
//            myPlayer.isLocalPlayer = true;
//            myPlayer.gameObject.name = string.Format("[{0}]", data.data.playerName);
//            myPlayer.userId = data.data.playerId;
//            myPlayer.userName = data.data.playerName;
//            myPlayer.region = data.data.region;
//            networkPlayers.Add(myPlayer.userId, myPlayer);
//            allPlayers.Add(myPlayer.userId, myPlayer);
//            playerManager.playerList.Add(myPlayer);
//            UIManager.Instance.OnPlayerInfoAdd(myPlayer.userId, myPlayer.userName);*/
//            //GlobalLobbyJoin(myPlayer.userId, myPlayer.userName);
//        }

//    }
//    #endregion

//    #region Globally Join All Player
//    private void GlobalLobbyJoin(string playerId, string playerName)
//    {
//        SimpleJSON.JSONNode GlobalLobbyJoin = new SimpleJSON.JSONObject
//        {
//            ["playerId"] = playerId,
//            ["playerName"] = playerName,
//        };
//        Debug.Log("Global Lobby Join Data:" + GlobalLobbyJoin.ToString());
//        //socket.Emit(SocketStaticData.GLOBAL_LOBBY_JOIN, GlobalLobbyJoin.ToString());
//    }
//    private void OnGlobalLobbyJoin(GetAllPlayerData data)
//    {
//        for (int i = 0; i < data.players.Count; i++)
//        {
//            string formattedString = string.Format("1. Name: {0} 2. UserId: {1}", data.players[0].playerName, data.players[0].playerId);
//            Debug.Log(formattedString);
//        }

//    }
//    #endregion

//    #region Room Update || Create || Join || Leave

//    #region UpdaterRooms Data
//    void OnUpdateRoomsSuccess(GetData data) { }
//    void On_UpdateRooms(GetData data)
//    {
//        // Handle the updated room list received from the server
//        // e.data contains the updated room list
//    }
//    public void UpdateRooms()
//    {
//        // Handle the updated room list received from the server
//        // e.data contains the updated room list

//    }
//    private void OnUpdateRoomsFailed(GetData data) { }
//    #endregion

//    #region CreateRoom 
//    private void OnCreateRoomSuccess(GetData data)
//    {
//        Debug.Log("Create Room Success...");
//        //Debug.LogError(string.Format("Room: {0}", data.));
//    }
//    public void CreateRoom(/*string hostPlayerName,*/string roomName/*, bool isPrivate, bool isVisible*/, int maxPlayers/*, int totalPlayers*/, string password)
//    {
//        SimpleJSON.JSONNode roomInfoJson = new SimpleJSON.JSONObject
//        {
//            /* ["hostPlayerName"] = hostPlayerName,
//             ["room_name"]       = roomName,
//             ["isPrivate"]      = isPrivate,
//             ["isVisible"]      = isVisible,
//             ["max_players"]     = maxPlayers,
//             ["password"]       = password,*/
//        };

//        //this code is  JSONObject 
//        //JSONObject roomInfoJson = new JSONObject();
//        //roomInfoJson.AddField("hostPlayerName", hostPlayerName);
//        //roomInfoJson.AddField("roomId", roomId);
//        //roomInfoJson.AddField("roomName", roomName);
//        //roomInfoJson.AddField("isPrivate", isPrivate);
//        //roomInfoJson.AddField("isVisible", isVisible);
//        //roomInfoJson.AddField("maxPlayers", maxPlayers);
//        //roomInfoJson.AddField("totalPlayers", totalPlayers);
//        //roomInfoJson.AddField("password", password);

//        Debug.Log(roomInfoJson);
//        // Emit the 'createRoom' event with room information
//        socket.Emit(SocketStaticData.CREATE_ROOM, roomInfoJson.ToString());
//    }
//    private void OnCreateRoomFailed(GetFailedData data)
//    {
//        SocketDemoManager.Instance.UIManager.ShowMassage(data.message);
//    }
//    #endregion

//    #region JoinRoom
//    private void OnJoinRoomSuccess(GetData data)
//    {
//        SocketDemoManager.Instance.UIManager.LobbyPanal.ShowStartBtn();
//    }
//    public void JoinRoom(string roomId, string password)
//    {
//        //JSONObject roomInfoJson = new JSONObject();
//        //roomInfoJson.AddField("roomId", roomId);
//        //roomInfoJson.AddField("password", password);

//        // Emit the 'joinRoom' event with room information
//        //socket.Emit("joinRoom", roomInfoJson);
//    }
//    private void OnJoinRoomFailed(GetData data)
//    {
//        SocketDemoManager.Instance.UIManager.LobbyPanal.ShowStartBtn();
//    }
//    #endregion

//    #region LeaveRoom
//    private void OnLeaveRoomSuccess(GetData data)
//    {
//        SocketDemoManager.Instance.UIManager.LobbyPanal.ShowStartBtn();
//    }
//    void LeaveRoom(string roomId)
//    {
//        // Emit the 'leaveRoom' event with room ID
//        socket.Emit("leaveRoom", roomId);
//    }
//    private void OnLeaveRoomFailed(GetData data)
//    {
//        SocketDemoManager.Instance.UIManager.LobbyPanal.ShowStartBtn();
//    }
//    #endregion

//    #endregion
//}
