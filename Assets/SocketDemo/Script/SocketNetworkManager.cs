using UnityEngine;
using System;
using BestHTTP.SocketIO3;
using BestHTTP;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SocketNetworkManager : MonoBehaviour
{
    public static SocketNetworkManager Instance { get; private set; }

    public SocketManager manager;
    public Socket socket;
    public string playerID = string.Empty;
    public string Url = "https://fdb4-2405-201-200b-c05b-14fd-7a27-2ef5-da7e.ngrok.io";


    #region AWAKE
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);

        }
        else Destroy(this.gameObject);




    }
    #endregion

    #region START 
    private void Start()
    {

        if (TS.Generics.AllScenes.instance != null)
        {
            Destroy(TS.Generics.AllScenes.instance.gameObject);
        }
        HTTPManager.MaxConnectionIdleTime = TimeSpan.FromSeconds(180);

        Url = Url + "/socket.io/";
        //Debug.Log("Start : " + Url);
        try
        {
            manager = new SocketManager(new Uri(Url));
            //Debug.Log("URl " + manager.Uri);

            socket = manager.Socket;

            socket.On(SocketIOEventTypes.Connect, () =>
            {
                Debug.Log("Connected");
                InvokeRepeating("Ping", 0.5f, 3f);
            });

            socket.On(SocketIOEventTypes.Disconnect, () => { Debug.Log("Disconnected"); });
            socket.On<CustomError>(SocketIOEventTypes.Error, OnError);
            socket.On<ON_PONG>(SocketStaticData.ON_PONG, OnPong);
            socket.On<JOINED_LOBBY_SUCCESS>(SocketStaticData.ON_JOINED_LOBBY_SUCCESS, OnJoinLobbySuccess);
            socket.On<ROOM_LIST_UPDATE>(SocketStaticData.ON_ROOM_LIST_UPDATE, OnRoomList);
            socket.On<CREATE_ROOM_SUCCESS>(SocketStaticData.ON_CREATE_ROOM_SUCCESS, OnCreateRoomSuccess);
            socket.On<CREATE_ROOM_FAILED>(SocketStaticData.ON_CREATE_ROOM_FAILED, OnCreateRoomFailed);
            socket.On<PLAYER_JOINED>(SocketStaticData.ON_PLAYER_JOINED, OnPlayerJoinedRoom);
            socket.On<JOIN_ROOM_FAILED>(SocketStaticData.ON_JOIN_ROOM_FAILED, OnPlayerJoinedRoomFail);
            socket.On<PLAYER_LIST_UPDATE>(SocketStaticData.ON_PLAYER_LIST_UPDATE, OnPlayerListUpdate);
            socket.On<PLAYER_LIST_UPDATE>(SocketStaticData.ON_NPC_DATA, OnNPCData);
            socket.On<PLAYER_LEFT>(SocketStaticData.ON_PLAYER_LEFT, OnPlayerLeft);
            socket.On<PLAYER_READY>(SocketStaticData.ON_PLAYER_READY, OnPlayerReady);
            socket.On<PLAYER_READY_FAILED>(SocketStaticData.ON_PLAYER_READY_FAILED, OnPlayerReadyFailed);
            socket.On<START_GAME_NOW>(SocketStaticData.ON_START_GAME, OnStartGame);
            socket.On<START_GAME_FAILED>(SocketStaticData.ON_START_GAME_FAILED, OnStartGameFailed);
            socket.On<ON_TEAM_SELECTION>(SocketStaticData.ON_TEAM_SELECT, OnTeamSelected);
            socket.On<ON_POS_AND_ROT>(SocketStaticData.ON_POS_AND_ROT, OnPosRotRecieved);
            socket.On<ON_POS_AND_ROT>(SocketStaticData.ON_NPC_POS_AND_ROT, OnNPCPosRotRecieved);
            socket.On<ON_PLAYER_HEALTH>(SocketStaticData.ON_PLAYER_HEALTH, OnHealthUpdate);
            socket.On<ON_PLAYER_EXPLODE>(SocketStaticData.ON_PLAYER_EXPLODE, OnExlodePlayer);
            socket.On<ON_PLAYER_LEADERBORD_INFO>(SocketStaticData.ON_PLAYER_LEADERBORD_INFO, OnLeaderBordUpdate);
            socket.On<ON_TIMER>(SocketStaticData.ON_TIMER, OnTimerReceived);
            socket.On<ON_PLAYER_DESTROYED>(SocketStaticData.ON_PLAYER_DESTROYED, ON_PLAYER_DESTROYED);
            socket.On<ON_PLAYER_FIRE>(SocketStaticData.ON_PLAYER_FIRE, FireUpdate);
            socket.On<POWERUP_DATA>(SocketStaticData.ON_POWERUP_COLLECTED, OnCollectPowerUp);
            socket.On<POWERUP_DATA>(SocketStaticData.ON_POWERUP_USE, OnUsePrimaryPowerUp);
            socket.On<ON_POS_AND_ROT>(SocketStaticData.ON_MACHINE_GUN_ROT, OnMachineGunRotReceived);

            Debug.Log("Connecting...");

            manager.Open();
            //UILodingScreen loadingScreen = SocketUIManager.Instance.helixUIManager.UIScreens[2].gameObject.GetComponent<UILodingScreen>();

        }
        catch (Exception e)
        {
            Debug.Log("Error = " + e.ToString());
            throw;
        }
    }
    #endregion

    #region ERROR EVENT
    void OnError(CustomError args)
    {
        Debug.LogError(string.Format("Error: {0}", args.ToString()));
    }
    #endregion

    #region PING/PONG
    private void OnPong(ON_PONG pong)
    {
        //Debug.Log("ON PONG");
    }
    public void Ping()
    {
        socket.Emit(SocketStaticData.PING, string.Empty);
    }
    #endregion

    #region PLAYER_DESTROYED
    private void ON_PLAYER_DESTROYED(ON_PLAYER_DESTROYED data)
    {
        try
        {
            Debug.Log("   " + data.data.player_id);
            SocketPlayerManager.Instance.helixPlayerInfoList.Clear();
            //GameCanvas.Instance.Update_PointsTable(data.data.player_id);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> ON_PLAYER_DESTROYED ---> {e.ToString()}");
            throw;
        }
    }
    public void EmitDestroyedPlayerID(string playerId)
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["player_id"] = playerId,
            };
            Debug.Log("Destroyed playerId" + data.ToString());
            socket.Emit(SocketStaticData.PLAYER_DESTROYED, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> EmitDestroyedPlayerID ---> {e.ToString()}");
        }

    }
    #endregion

    #region TIMER EVENT
    private void OnTimerReceived(ON_TIMER count)
    {
        try
        {
            /*if (GameCanvas.Instance)
                GameCanvas.Instance.Update_Text_Timer(count.data.duration);*/


            //NEW Code ...........
            int receiveTime = count.data.duration;
            //Debug.Log("receiveTime: " + receiveTime);
            GameplayManager.Instance?.ReceiveTimerData(receiveTime);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnTimerReceived ---> {e.ToString()}");
        }

    }
    public void emitSetTimer(int duration)
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["duration"] = duration,
            };
            //Debug.Log("duration: " + duration);
            socket.Emit(SocketStaticData.TIMER, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> emitSetTimer ---> {e.ToString()}");
        }

    }
    #endregion

    #region ON_EXPLODE_PLAYER

    public void EmitExlodePlayer(string localPlayer, string opponentPlayerID)
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["player_id"] = localPlayer,
                ["opponentPlayerID"] = opponentPlayerID,

            };
            //Debug.Log("Emit Exlode Player: " + data.ToString());
            socket.Emit(SocketStaticData.PLAYER_EXPLODE, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> EmitExlodePlayer ---> {e.ToString()}");
        }
    }

    public void OnExlodePlayer(ON_PLAYER_EXPLODE data)
    {
        try
        {
            if (!data.success)
            {
                return;
            }

            var deadPlayerId = data.data.player_id;
            var killerPlayerID = data.data.opponentPlayerID;

            //Debug.Log("deadPlayerId: " + deadPlayerId + "killerPlayerID: " + killerPlayerID);
            if (SocketPlayerManager.Instance.helixPlayerInfo.userID == deadPlayerId)
            {
                //Debug.Log("------dead_current Player:::  DeadPlayerID:::"+SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].userName + "killerPlayerID: " + SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].userName);

                PlayerID player = SocketPlayerManager.Instance.allPlayers[deadPlayerId];
                player.helixPlayerHealth.IsDead = true;
                player.helixPlayerHealth.Health = 0;
                //GameplayManager.instance.IncrementPlayerScore(100, killerPlayerID);
                if (SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamA != SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamA || !SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamA == SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamA ||
                    SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamB != SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamB || !SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamB == SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamB)
                    player.helixPlayerHealth.TeamScore(killerPlayerID);
                player.helixPlayerHealth.ExplodeCar(true, data.data);
                player.helixPlayerHealth.OnDeath.Invoke();


                //player = SocketPlayerManager.Instance.allPlayers[killerPlayerID];
                if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser && SocketPlayerManager.Instance.allPlayers.ContainsKey(killerPlayerID) && SocketPlayerManager.Instance.allPlayers[killerPlayerID].isAIPlayer)
                {
                    EmitLeaderBord(killerPlayerID, GameplayManager.Instance.teamAScore, GameplayManager.Instance.teamBScore, SocketPlayerManager.Instance.allPlayers[killerPlayerID].helixPlayerInfo.userKillPoint);
                }

            }
            else if (SocketPlayerManager.Instance.helixPlayerInfo.userID == killerPlayerID)
            {
                //Debug.Log("---------killerPlayerID------------deadPlayerId: " + SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].userName + "killerPlayerID: " + SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].userName);

                //Debug.Log("------kill by current Player");

                //Debug.Log("2");
                //PlayerID player = SocketPlayerManager.Instance.allPlayers[killerPlayerID];
                PlayerID player = SocketPlayerManager.Instance.allPlayers[deadPlayerId];
                player.helixPlayerHealth.IsDead = true;
                player.helixPlayerHealth.Health = 0;
                if (SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamA != SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamA || !SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamA == SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamA ||
                   SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamB != SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamB || !SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamB == SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamB)
                    player.helixPlayerHealth.TeamScore(killerPlayerID);


                EmitLeaderBord(killerPlayerID, GameplayManager.Instance.teamAScore, GameplayManager.Instance.teamBScore, SocketPlayerManager.Instance.helixPlayerInfo.userKillPoint);
                player.helixPlayerHealth.ExplodeCar(true, data.data);
                player.helixPlayerHealth.OnDeath.Invoke();

            }
            else /*if(SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)*/
            {
                // Debug.Log("------other Players");
                //Debug.Log("|||||||||---------OtherPLayer------------deadPlayerId: " +SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].userName + "killerPlayerID: " + SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].userName);
                //Debug.Log("3");
                PlayerID player = SocketPlayerManager.Instance.allPlayers[deadPlayerId];
                player.helixPlayerHealth.IsDead = true;
                player.helixPlayerHealth.Health = 0;
                if (SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamA != SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamA || !SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamA == SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamA ||
                   SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamB != SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamB || !SocketPlayerManager.Instance.helixPlayerInfoList[deadPlayerId].teamB == SocketPlayerManager.Instance.helixPlayerInfoList[killerPlayerID].teamB)
                    player.helixPlayerHealth.TeamScore(killerPlayerID);
                player.helixPlayerHealth.ExplodeCar(true, data.data);
                player.helixPlayerHealth.OnDeath.Invoke();


                if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser && SocketPlayerManager.Instance.allPlayers.ContainsKey(killerPlayerID) && SocketPlayerManager.Instance.allPlayers[killerPlayerID].isAIPlayer)
                {
                    //Debug.Log("Increase score");
                    EmitLeaderBord(killerPlayerID, GameplayManager.Instance.teamAScore, GameplayManager.Instance.teamBScore, SocketPlayerManager.Instance.allPlayers[killerPlayerID].helixPlayerInfo.userKillPoint);
                    //player.helixPlayerHealth.TeamScore(deadPlayerId);
                }
            }

            //EditorApplication.isPaused = true;
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnExlodePlayer ---> {e.ToString()}");
        }

    }

    #endregion

    #region  HEALTH EVENT

    public void EmitHealth(string shooterID, string playerid, string health)
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["ShooterPlayerId"] = shooterID,
                ["opponent_player_id"] = playerid,
                ["health"] = health,

            };

            //Debug.Log("EMIT HEATH DATA::::::::" + data.ToString());
            socket.Emit(SocketStaticData.PLAYER_HEALTH, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> EmitHealth ---> {e.ToString()}");
        }
    }
    private void OnHealthUpdate(ON_PLAYER_HEALTH data)
    {
        try
        {
            int _health = int.Parse(data.data.health);
            string opponentPlayerID = data.data.opponent_player_id;
            string ShooterPlayerId = data.data.ShooterPlayerId;

            //Debug.Log("ShooterPlayerId:::;;::: " + ShooterPlayerId + "||oppPlayerId: " + opponentPlayerID + "|| Health: " + _health);
            if (opponentPlayerID != null)
            {
                if (SocketPlayerManager.Instance.allPlayers.ContainsKey(opponentPlayerID))
                {
                    SocketPlayerManager.Instance.allPlayers[opponentPlayerID].helixPlayerHealth.DoDamage(_health, default(Vector3), ShooterPlayerId);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnHealthUpdate ---> {e.ToString()}");
        }
    }
    #endregion

    #region LEADERBORD EVENT
    private void OnLeaderBordUpdate(ON_PLAYER_LEADERBORD_INFO data)
    {


        try
        {
            if (SocketPlayerManager.Instance.allPlayers.ContainsKey(data.data.player_id))
            {
                SocketPlayerManager.Instance.allPlayers[data.data.player_id].helixPlayerInfo.userKillPoint = data.data.playerScore;


                SocketPlayerManager.Instance.helixPlayerInfoList[data.data.player_id].userKillPoint = data.data.playerScore;
                //Debug.Log("::::::>>>>Get Player ID:::::::Increase Score::::" + SocketPlayerManager.Instance.allPlayers[data.data.player_id].helixPlayerInfo.userName + "::::" + SocketPlayerManager.Instance.allPlayers[data.data.player_id].helixPlayerInfo.teamA.ToString() + ":::" + SocketPlayerManager.Instance.allPlayers[data.data.player_id].helixPlayerInfo.teamB.ToString() + "::::" + data.data.playerScore.ToString());

            }
            else
            {
                if (SocketPlayerManager.Instance.helixPlayerInfoList.ContainsKey(data.data.player_id))
                {
                    SocketPlayerManager.Instance.helixPlayerInfoList[data.data.player_id].userKillPoint = data.data.playerScore;
                }
            }

            //else
            //{
            //    Debug.Log("data.data.player_id:::::");
            //}


        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnLeaderBordUpdate ---> {e.ToString()}");
        }

    }
    public void EmitLeaderBord(string currentPlayerID, int team_A_Point, int team_B_Point, int points)
    {
        try
        {


            if (currentPlayerID == playerID)
            {
                Debug.Log("::::>>>>POINTS:::" + points);
            }

            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["player_id"] = currentPlayerID,
                ["team_A_Points"] = team_A_Point,
                ["team_B_Points"] = team_B_Point,

                ["playerScore"] = points + 100,

            };

            //Debug.Log("::::::>>>>Emit Player ID::" + currentPlayerID);
            socket.Emit(SocketStaticData.PLAYER_LEADERBORD_INFO, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> EmitLeaderBord ---> {e.ToString()}");
        }

    }
    #endregion

    #region FiRE EVENT


    SimpleJSON.JSONNode emitFireUpdateData;
    public void EmitFireUpdate(string method, Transform gen, int force, string firePlayerID, string missileID, bool isBlast = false)
    {
        try
        {
            if (firePlayerID == playerID || (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser && SocketPlayerManager.Instance.allPlayers.ContainsKey(firePlayerID) && SocketPlayerManager.Instance.allPlayers[firePlayerID].isAIPlayer))
            {
                if (!string.IsNullOrEmpty(firePlayerID) && !string.IsNullOrEmpty(missileID) || method == "bullet" || method == "NormalBullet" || method == "NormalMissile")
                {

                    emitFireUpdateData = new SimpleJSON.JSONObject
                    {
                        ["player_id"] = firePlayerID,
                        ["id"] = missileID,
                        ["fire"] = method,
                        ["dx"] = (float)gen.position.x,
                        ["dy"] = (float)gen.position.y,
                        ["dz"] = (float)gen.position.z,
                        ["rx"] = (float)gen.eulerAngles.x,
                        ["ry"] = (float)gen.eulerAngles.y,
                        ["rz"] = (float)gen.eulerAngles.z,
                        ["deadPlayerId"] = isBlast.ToString(),
                        ["force"] = force,
                    };
                    //Debug.Log(">>>>>>>>>>>>EmitFire Update " + firePlayerID + "::::::"+ gen.eulerAngles);
                    socket.Emit(SocketStaticData.PLAYER_FIRE, emitFireUpdateData.ToString());
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> EmitFireUpdate ---> {e.ToString()}");
        }
    }

    private void FireUpdate(ON_PLAYER_FIRE method)
    {
        try
        {
            //Debug.Log(">>>>>>>>>>>>Get Fire Update " + method.data.player_id.ToString() + "::::::" + method.data.fire);
            ////de
            if (!method.data.player_id.Equals(playerID) && GameplayManager.Instance)
                GameplayManager.Instance.UpdateBulletHit(method.data);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> FireUpdate ---> {e.ToString()}");

        }
    }
    #endregion


    #region MACHINE GUN ROTATION

    public void EmitMachineGunRot(string firePlayerID, Vector3 pos, Vector3 gen)
    {
        try
        {
            if (!string.IsNullOrEmpty(firePlayerID))
            {

                emitFireUpdateData = new SimpleJSON.JSONObject
                {
                    ["player_id"] = firePlayerID,
                    ["rx"] = (float)gen.x,
                    ["ry"] = (float)gen.y,
                    ["rz"] = (float)gen.z,
                    ["dx"] = (float)pos.x,
                    ["dy"] = (float)pos.y,
                    ["dz"] = (float)pos.z
                };

                //Debug.Log(">>>>>>>>>>>>EmitFire Update " + firePlayerID + "::::::"+ gen.eulerAngles);
                socket.Emit(SocketStaticData.MACHINE_GUN_ROT, emitFireUpdateData.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> EmitMachineGunRot ---> {e.ToString()}");
        }
    }

    private void OnMachineGunRotReceived(ON_POS_AND_ROT rOT)
    {
        if (SocketPlayerManager.Instance.allPlayers.ContainsKey(rOT.data.player_id) && playerID != rOT.data.player_id)
        {


            #region Old Gun
            //if (SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].isAIPlayer && !SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)
            //{
            //    SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].gunControllerinAi.UpdateRotation(new Vector3(rOT.data.rx, rOT.data.ry, rOT.data.rz));
            //}
            //else
            //    SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].gunController.UpdateRotation(new Vector3(rOT.data.rx, rOT.data.ry, rOT.data.rz));
            #endregion

            #region New Gun
            if (SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].isAIPlayer && !SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)
            {
                // SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].gunControllerinAi.UpdateRotation(new Vector3(rOT.data.rx, rOT.data.ry, rOT.data.rz));
                if (SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].currentGunController != null)
                {
                    Debug.Log("GetRotation:::::>>>>" + new Vector3(rOT.data.rx, rOT.data.ry, rOT.data.rz));
                    SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].currentGunController.f3DPlayerTurretController.turret.UpdateRotation(new Vector3(rOT.data.dx, rOT.data.dy, rOT.data.dz), new Vector3(rOT.data.rx, rOT.data.ry, rOT.data.rz));
                }
                else
                    SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].gunControllerinAi.Weapon_MachineGun.f3DPlayerTurretController.turret.UpdateRotation(new Vector3(rOT.data.dx, rOT.data.dy, rOT.data.dz), new Vector3(rOT.data.rx, rOT.data.ry, rOT.data.rz));
            }
            else
            {
                if (SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].currentGunController != null)
                    SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].currentGunController.f3DPlayerTurretController.turret.UpdateRotation(new Vector3(rOT.data.dx, rOT.data.dy, rOT.data.dz), new Vector3(rOT.data.rx, rOT.data.ry, rOT.data.rz));
                else
                    SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].gunController.Weapon_MachineGun.f3DPlayerTurretController.turret.UpdateRotation(new Vector3(rOT.data.dx, rOT.data.dy, rOT.data.dz), new Vector3(rOT.data.rx, rOT.data.ry, rOT.data.rz));
            }
            #endregion
        }
    }
    #endregion

    #region POSITION & ROTATION EVENT

    private void OnPosRotRecieved(ON_POS_AND_ROT rOT)
    {
        try
        {

            if (SocketPlayerManager.Instance.allPlayers.ContainsKey(rOT.data.player_id) && playerID != rOT.data.player_id)
            {
                SocketPlayerManager.Instance.allPlayers[rOT.data.player_id].UpdatePosRot(rOT.data);
            }

        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnPosRotRecieved ---> {e.ToString()}");
        }
    }
    public void EmitPosAndRot(Transform t)
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {

                ["dx"] = (float)t.position.x,
                ["dy"] = (float)t.position.y,
                ["dz"] = (float)t.position.z,
                ["rx"] = (float)t.rotation.eulerAngles.x,
                ["ry"] = (float)t.rotation.eulerAngles.y,
                ["rz"] = (float)t.rotation.eulerAngles.z,

            };
            //Debug.Log("365 emit pos nad rot" + data.ToString());
            socket.Emit(SocketStaticData.POS_UPDATE, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> UpdatePos ---> {e.ToString()}");
        }
    }





    public void EmitNPCPosAndRot(string npcID, string npcName, Transform t)
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["player_id"] = npcID,
                ["player_name"] = npcName,
                ["dx"] = (float)t.position.x,
                ["dy"] = (float)t.position.y,
                ["dz"] = (float)t.position.z,
                ["rx"] = (float)t.rotation.eulerAngles.x,
                ["ry"] = (float)t.rotation.eulerAngles.y,
                ["rz"] = (float)t.rotation.eulerAngles.z,

            };
            //Debug.Log("||" + data.ToString());
            socket.Emit(SocketStaticData.NPC_POS_UPDATE, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> EmitNPCPosAndRot ---> {e.ToString()}");
        }
    }


    private void OnNPCPosRotRecieved(ON_POS_AND_ROT data)
    {
        try
        {

            if (SocketPlayerManager.Instance.allPlayers.ContainsKey(data.data.player_id) && playerID != data.data.player_id && !SocketPlayerManager.Instance.MyPlayer.helixPlayerInfo.isRoomHostUser)
            {
                Debug.Log("::::::>>>>>>>");
                SocketPlayerManager.Instance.allPlayers[data.data.player_id].UpdatePosRot(data.data);
            }

        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnNPCPosRotRecieved ---> {e.ToString()}");
        }
    }
    #endregion

    #region TEAM SELECTION EVENT
    public void Team_Select(string playerid, string Teamname, bool team_A, bool team_B)
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["player_id"] = playerid,
                ["teamname"] = Teamname,
                ["team_A"] = team_A,
                ["team_B"] = team_B,
            };
            //Debug.Log("teamname emit" + data.ToString());
            socket.Emit(SocketStaticData.TEAM_SELECTION, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> Team_Select ---> {e.ToString()}");

        }
    }
    private void OnTeamSelected(ON_TEAM_SELECTION TeamData)
    {
        try
        {
            //Debug.Log("||||" + TeamData.data.player_id + ":::::::::" + TeamData.data.team_A + ":::::::::" + TeamData.data.team_B);
            if (SocketPlayerManager.Instance.allPlayers.ContainsKey(TeamData.data.player_id))
            {

                //Debug.Log(TeamData.data.player_id+":::::::::" + TeamData.data.team_A+":::::::::" + TeamData.data.team_B);
                SocketPlayerManager.Instance.allPlayers[TeamData.data.player_id].helixPlayerInfo.teamA = TeamData.data.team_A;
                SocketPlayerManager.Instance.allPlayers[TeamData.data.player_id].helixPlayerInfo.teamB = TeamData.data.team_B;
                SocketPlayerManager.Instance.allPlayers[TeamData.data.player_id].SetTeamColor();



            }
            //else
            //{
            //Debug.Log("||||" + TeamData.data.player_id + ":::::::::" + TeamData.data.team_A + ":::::::::" + TeamData.data.team_B);
            //}
            //for (int i = 0; i < SocketPlayerManager.Instance.helixPlayerInfoList.Count; i++)
            //{
            if (SocketPlayerManager.Instance.helixPlayerInfoList.ContainsKey(TeamData.data.player_id))
            {
                SocketPlayerManager.Instance.helixPlayerInfoList[TeamData.data.player_id].teamA = TeamData.data.team_A;
                SocketPlayerManager.Instance.helixPlayerInfoList[TeamData.data.player_id].teamB = TeamData.data.team_B;
            }
            //}
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnTeamSelected ---> {e.ToString()}");
        }
    }
    #endregion

    #region ROOM GAME START
    private void OnStartGameFailed(START_GAME_FAILED fAILED)
    {
        try
        {
            SocketUIManager.Instance.ShowMassage(fAILED.message);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnStartGameFailed ---> {e.ToString()}");
        }
    }
    private void OnStartGame(START_GAME_NOW game)
    {
        try
        {
            if (!game.success)
            {
                return;
            }

            if (game.data.started)
            {
                //if (SocketDemoManager.Instance.PlayerManager.IsMaster)
                //{
                //    SocketDemoManager.Instance.UIManager.SetScreen(SocketScreens.SelectionPanel);
                //}
                //else
                //{
                //    SocketDemoManager.Instance.UIManager.SetScreen(SocketScreens.GameStarted);
                //}

                SocketUIManager.Instance.SetScreen(SocketScreens.GameStarted);
                string loadmap = game.data.mapname;

                StartCoroutine(LoadSceneCoroutine(loadmap));
            }
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnStartGame ---> {e.ToString()}");
        }
    }
    #endregion

    #region SCENELOAD
    public void OnSceneLoad(string sceneName)
    {
        try
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnSceneLoad ---> {e.ToString()}");
        }
    }
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        UILodingScreen loadingScreen = SocketUIManager.Instance.helixUIManager.UIScreens[2].gameObject.GetComponent<UILodingScreen>();

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Disable scene activation to allow loading to complete
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            // Update the loading slider value based on the loading progress
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9 is the max value for progress

            //Debug.Log("LoadingScreen Name: " + loadingScreen.gameObject.name);
            if (loadingScreen != null)
            {
                if (loadingScreen != null) loadingScreen.loadingSlider.value = progress;

                // Update loading text
                if (loadingScreen.loadingText != null)
                    loadingScreen.loadingText.text = "Loading: " + (progress * 100f).ToString("F0") + "%";
                //Debug.Log(loadingScreen.loadingText.text);

                // Check if the loading is almost complete (progress >= 0.9)
                if (asyncOperation.progress >= 0.9f)
                {
                    // Enable scene activation to complete the loading
                    asyncOperation.allowSceneActivation = true;
                    yield return new WaitForSeconds(1f);
                    //Debug.Log("Load scene Done");
                }
            }

            yield return null;
        }
    }
    #endregion

    #region PLAYER READY
    public void ReadyPlayer(bool isready)
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["player_id"] = this.playerID,
                ["isReady"] = isready,
            };
            socket.Emit(SocketStaticData.READY_PLAYER, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> ReadyPlayer ---> {e.ToString()}");
        }

    }
    private void OnPlayerReady(PLAYER_READY rEADY)
    {
        try
        {
            if (!rEADY.success)
            {
                return;
            }

        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnPlayerReady ---> {e.ToString()}");

        }
    }
    private void OnPlayerReadyFailed(PLAYER_READY_FAILED fAILED)
    {
        try
        {
            SocketUIManager.Instance.ShowMassage(fAILED.message);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnPlayerReadyFailed ---> {e.ToString()}");
        }
    }
    #endregion


    #region PLAYER LIST UPDATE



    public void EmitPlayerList()
    {
        try
        {

            JSONObject sendData = new JSONObject();

            JSONObject data;


            foreach (HelixPlayerInfo item in SocketPlayerManager.Instance.helixPlayerInfoList.Values)
            {
                data = new JSONObject();

                data.AddField("player_id", item.userID);
                data.AddField("player_name", item.userName);
                data.AddField("team_A", item.teamA);
                data.AddField("team_B", item.teamB);
                data.AddField("roomID", SocketPlayerManager.Instance.helixPlayerInfo.roomID);

                if (SocketPlayerManager.Instance.allPlayers.ContainsKey(item.userID) && SocketPlayerManager.Instance.allPlayers[item.userID].isAIPlayer)
                    sendData.Add(data);

                //data.AddField("isMaster", item.isRoomHostUser);
                //data.AddField("teamname",item.teamA?"Team A": "Team B");
                //data.AddField("region", item.userRegion);
                //data.AddField("room_name", item.roomName);
                //data.AddField("password", "");
                //data.AddField("max_players", item.roomMaxPlayer);
                //data.AddField("current_players", item.roomCurrentPlayer);
                //data.AddField("isReady", item.userReadyRoom);
                //data.AddField("team_A_Points", item.team_A_KillPoint);
                //data.AddField("team_B_Points", item.team_B_KillPoint);
                //data.AddField("playerScore", item.userKillPoint);
                //data.AddField("localPlayerID", "");
                //data.AddField("opponentPlayerID", "");

            }

            //Debug.Log("Emit:::::::::::::" + sendData.ToString());
            socket.Emit(SocketStaticData.ADD_NPC_DATA, sendData.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> ReadyPlayer ---> {e.ToString()}");
        }
    }

    private void OnNPCData(PLAYER_LIST_UPDATE uPDATE)
    {

        //Debug.Log("<color=Grren>::::::::NPC::::::</color>" + uPDATE.data.Count);
        if (!uPDATE.success)
        {
            return;
        }

        foreach (var updateData in uPDATE.data)
        {
            //Debug.Log($"------Up------UserName: {updateData.player_name} ||Team A: {updateData.teamname}  || Team A: {updateData.team_A} || Team B: {updateData.team_B} || ");
            //Debug.Log($"------Up------UserName: {updateData.player_name} || userID: {updateData.player_id} || userRegion: {updateData.region} || isLocalUser: {false} || roomID: {updateData.roomID} || roomName: {updateData.room_id} || isRoomHostUser: {updateData.isMaster} || UserReadyRoom: {updateData.isReady} || Team A: {updateData.teamname} || Team B: {updateData.teamname} || Team A Kill Point : {updateData.team_A_Points} || Team B Kill Point: {updateData.team_B_Points} || User Kill Point: {updateData.playerScore}");
            //Debug.LogError("updateData.playerScore::" + updateData.playerScore);

            var tempData = new HelixPlayerInfo();
            tempData.userID = updateData.player_id;
            tempData.userName = updateData.player_name;
            tempData.userRegion = updateData.region;
            tempData.roomID = updateData.roomID;
            tempData.roomName = updateData.room_name;
            tempData.isRoomHostUser = updateData.isMaster;
            tempData.teamA = updateData.team_A;
            tempData.teamB = updateData.team_B;
            tempData.userReadyRoom = updateData.isReady;
            tempData.roomMaxPlayer = updateData.max_players;
            tempData.roomCurrentPlayer = updateData.current_players;
            tempData.team_A_KillPoint = updateData.team_A_Points;
            tempData.team_B_KillPoint = updateData.team_B_Points;
            tempData.userKillPoint = updateData.playerScore;

            //Debug.Log($"------AFTER DATA------UserName: {tempData.userName} || userID: {tempData.userID} || userRegion: {tempData.userRegion} || isLocalUser: {false} || roomID: {tempData.roomID} || roomName: {tempData.roomName} || roomCurrentPlayer: {tempData.roomCurrentPlayer} || || isRoomHostUser: {tempData.isRoomHostUser} || UserReadyRoom: {tempData.userReadyRoom} || Team A: {tempData.teamA} || Team B: {tempData.teamB} || Team A Kill Point : {tempData.team_A_KillPoint} || Team B Kill Point: {tempData.team_B_KillPoint} || User Kill Point: {tempData.userKillPoint}");

            //Create a unique key for the player preferences using the player's ID
            string playerPrefsKey = $"Player_{tempData.userID}";
            PlayerPrefs.SetString(playerPrefsKey + "_userName", tempData.userName);
            PlayerPrefs.SetString(playerPrefsKey + "_userRegion", tempData.userRegion);
            PlayerPrefs.SetString(playerPrefsKey + "_roomID", tempData.roomID);
            PlayerPrefs.SetString(playerPrefsKey + "_roomName", tempData.roomName);
            PlayerPrefs.SetInt(playerPrefsKey + "_isRoomHostUser", tempData.isRoomHostUser ? 1 : 0);
            PlayerPrefs.SetInt(playerPrefsKey + "_teamA", tempData.teamA ? 1 : 0);
            PlayerPrefs.SetInt(playerPrefsKey + "_teamB", tempData.teamB ? 1 : 0);
            PlayerPrefs.SetInt(playerPrefsKey + "_userReadyRoom", tempData.userReadyRoom ? 1 : 0);
            PlayerPrefs.SetInt(playerPrefsKey + "_roomMaxPlayer", tempData.roomMaxPlayer);
            PlayerPrefs.SetInt(playerPrefsKey + "_roomCurrentPlayer", tempData.roomCurrentPlayer);
            PlayerPrefs.SetInt(playerPrefsKey + "_team_A_KillPoint", tempData.team_A_KillPoint);
            PlayerPrefs.SetInt(playerPrefsKey + "_team_B_KillPoint", tempData.team_B_KillPoint);
            PlayerPrefs.SetInt(playerPrefsKey + "_userKillPoint", tempData.userKillPoint);

            // Save player preferences immediately
            PlayerPrefs.Save();



            if (SocketPlayerManager.Instance.helixPlayerInfoList.ContainsKey(tempData.userID))
            {
                SocketPlayerManager.Instance.helixPlayerInfoList[tempData.userID] = tempData;
            }
            else
            {
                SocketPlayerManager.Instance.helixPlayerInfoList.Add(tempData.userID, tempData);
            }

            if (SocketPlayerManager.Instance.allPlayers.ContainsKey(tempData.userID))
            {
                SocketPlayerManager.Instance.allPlayers[tempData.userID].helixPlayerInfo.teamA = tempData.teamA;
                SocketPlayerManager.Instance.allPlayers[tempData.userID].helixPlayerInfo.teamB = tempData.teamB;
                SocketPlayerManager.Instance.allPlayers[tempData.userID].helixPlayerInfo.userKillPoint = tempData.userKillPoint;
                SocketPlayerManager.Instance.allPlayers[tempData.userID].helixPlayerInfo.team_A_KillPoint = tempData.team_A_KillPoint;
                SocketPlayerManager.Instance.allPlayers[tempData.userID].helixPlayerInfo.team_B_KillPoint = tempData.team_B_KillPoint;
            }


            if (playerID == tempData.userID && SocketPlayerManager.Instance.helixPlayerInfo.userID == tempData.userID)
            {
                Debug.Log(":::::??????:::::" + tempData.isRoomHostUser);
                SocketPlayerManager.Instance.helixPlayerInfo = tempData;
                Debug.Log(":::>>>");
            }


        }
    }

    private void OnPlayerListUpdate(PLAYER_LIST_UPDATE uPDATE)
    {
        try
        {
            //Debug.Log("<color=Yellow>::::::::::::::</color>" + uPDATE.data.Count);
            if (!uPDATE.success)
            {
                return;
            }

            foreach (var updateData in uPDATE.data)
            {
                //Debug.Log($"------Up------UserName: {updateData.player_name} || userID: {updateData.player_id} || userRegion: {updateData.region} || isLocalUser: {false} || roomID: {updateData.roomID} || roomName: {updateData.room_id} || isRoomHostUser: {updateData.isMaster} || UserReadyRoom: {updateData.isReady} || Team A: {updateData.teamname} || Team B: {updateData.teamname} || Team A Kill Point : {updateData.team_A_Points} || Team B Kill Point: {updateData.team_B_Points} || User Kill Point: {updateData.playerScore}");
                //Debug.LogError("updateData.playerScore::" + updateData.playerScore);

                var tempData = new HelixPlayerInfo();
                tempData.userID = updateData.player_id;
                tempData.userName = updateData.player_name;
                tempData.userRegion = updateData.region;
                tempData.roomID = updateData.roomID;
                tempData.roomName = updateData.room_name;
                tempData.isRoomHostUser = updateData.isMaster;
                tempData.teamA = updateData.team_A;
                tempData.teamB = updateData.team_B;
                tempData.userReadyRoom = updateData.isReady;
                tempData.roomMaxPlayer = updateData.max_players;
                tempData.roomCurrentPlayer = updateData.current_players;



                //Debug.Log(tempData.userName + ":::tempData.userKillPoint:::" + tempData.userKillPoint + "??????????" + updateData.playerScore);
                if (SocketPlayerManager.Instance.allPlayers.ContainsKey(updateData.player_id) && !SocketPlayerManager.Instance.allPlayers[updateData.player_id].isAIPlayer)
                {
                    tempData.team_A_KillPoint = updateData.team_A_Points;
                    tempData.team_B_KillPoint = updateData.team_B_Points;
                    tempData.userKillPoint = updateData.playerScore;
                }
                else if (SocketPlayerManager.Instance.allPlayers.ContainsKey(updateData.player_id) && !SocketPlayerManager.Instance.allPlayers[updateData.player_id].isAIPlayer)
                {

                    Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                    tempData.userKillPoint = SocketPlayerManager.Instance.allPlayers[updateData.player_id].helixPlayerInfo.userKillPoint;
                    //tempData.team_B_KillPoint = updateData.team_B_Points;
                    //tempData.userKillPoint = updateData.playerScore;
                }

                //Debug.Log($"------AFTER DATA------UserName: {tempData.userName} || userID: {tempData.userID} || userRegion: {tempData.userRegion} || isLocalUser: {false} || roomID: {tempData.roomID} || roomName: {tempData.roomName} || roomCurrentPlayer: {tempData.roomCurrentPlayer} || || isRoomHostUser: {tempData.isRoomHostUser} || UserReadyRoom: {tempData.userReadyRoom} || Team A: {tempData.teamA} || Team B: {tempData.teamB} || Team A Kill Point : {tempData.team_A_KillPoint} || Team B Kill Point: {tempData.team_B_KillPoint} || User Kill Point: {tempData.userKillPoint}");

                //Create a unique key for the player preferences using the player's ID
                string playerPrefsKey = $"Player_{tempData.userID}";
                PlayerPrefs.SetString(playerPrefsKey + "_userName", tempData.userName);
                PlayerPrefs.SetString(playerPrefsKey + "_userRegion", tempData.userRegion);
                PlayerPrefs.SetString(playerPrefsKey + "_roomID", tempData.roomID);
                PlayerPrefs.SetString(playerPrefsKey + "_roomName", tempData.roomName);
                PlayerPrefs.SetInt(playerPrefsKey + "_isRoomHostUser", tempData.isRoomHostUser ? 1 : 0);
                PlayerPrefs.SetInt(playerPrefsKey + "_teamA", tempData.teamA ? 1 : 0);
                PlayerPrefs.SetInt(playerPrefsKey + "_teamB", tempData.teamB ? 1 : 0);
                PlayerPrefs.SetInt(playerPrefsKey + "_userReadyRoom", tempData.userReadyRoom ? 1 : 0);
                PlayerPrefs.SetInt(playerPrefsKey + "_roomMaxPlayer", tempData.roomMaxPlayer);
                PlayerPrefs.SetInt(playerPrefsKey + "_roomCurrentPlayer", tempData.roomCurrentPlayer);
                if (SocketPlayerManager.Instance.allPlayers.ContainsKey(updateData.player_id) && !SocketPlayerManager.Instance.allPlayers[updateData.player_id].isAIPlayer)
                {
                    PlayerPrefs.SetInt(playerPrefsKey + "_team_A_KillPoint", tempData.team_A_KillPoint);
                    PlayerPrefs.SetInt(playerPrefsKey + "_team_B_KillPoint", tempData.team_B_KillPoint);
                    PlayerPrefs.SetInt(playerPrefsKey + "_userKillPoint", tempData.userKillPoint);
                }
                else if (SocketPlayerManager.Instance.allPlayers.ContainsKey(updateData.player_id) && !SocketPlayerManager.Instance.allPlayers[updateData.player_id].isAIPlayer)
                {
                    PlayerPrefs.SetInt(playerPrefsKey + "_userKillPoint", tempData.userKillPoint);
                }


                // Save player preferences immediately
                PlayerPrefs.Save();


                if (SocketPlayerManager.Instance.helixPlayerInfoList.ContainsKey(tempData.userID))
                {
                    SocketPlayerManager.Instance.helixPlayerInfoList[tempData.userID] = tempData;
                }
                else
                {
                    SocketPlayerManager.Instance.helixPlayerInfoList.Add(tempData.userID, tempData);
                }

                if (playerID == tempData.userID)
                {
                     tempData.userKillPoint = SocketPlayerManager.Instance.helixPlayerInfo.userKillPoint;
                     tempData.team_A_KillPoint = SocketPlayerManager.Instance.helixPlayerInfo.team_A_KillPoint;
                     tempData.team_B_KillPoint = SocketPlayerManager.Instance.helixPlayerInfo.team_B_KillPoint;
                    SocketPlayerManager.Instance.helixPlayerInfo = tempData;
                }
            }
            //Debug.Log("######################");

            //SocketPlayerManager.Instance.Checkdata();
            SocketUIManager.Instance.helixUIManager.LobbyPanal.GeneratePlayerList(uPDATE);
            SocketUIManager.Instance.helixUIManager.LobbyPanal.GenerateTeamAPlayerList(uPDATE);
            SocketUIManager.Instance.helixUIManager.LobbyPanal.GenerateTeamBPlayerList(uPDATE);
            SocketUIManager.Instance.LobbyPanal.CheckIsPlayerReady(uPDATE);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnPlayerListUpdate ---> {e.ToString()}");
        }

    }
    #endregion




    public void OnSaveNPCData(HelixPlayerInfo tempData)
    {
        string playerPrefsKey = $"Player_{tempData.userID}";
        PlayerPrefs.SetString(playerPrefsKey + "_userName", tempData.userName);
        PlayerPrefs.SetString(playerPrefsKey + "_userRegion", tempData.userRegion);
        PlayerPrefs.SetString(playerPrefsKey + "_roomID", tempData.roomID);
        PlayerPrefs.SetString(playerPrefsKey + "_roomName", tempData.roomName);
        PlayerPrefs.SetInt(playerPrefsKey + "_isRoomHostUser", tempData.isRoomHostUser ? 1 : 0);
        PlayerPrefs.SetInt(playerPrefsKey + "_teamA", tempData.teamA ? 1 : 0);
        PlayerPrefs.SetInt(playerPrefsKey + "_teamB", tempData.teamB ? 1 : 0);
        PlayerPrefs.SetInt(playerPrefsKey + "_userReadyRoom", tempData.userReadyRoom ? 1 : 0);
        PlayerPrefs.SetInt(playerPrefsKey + "_roomMaxPlayer", tempData.roomMaxPlayer);
        PlayerPrefs.SetInt(playerPrefsKey + "_roomCurrentPlayer", tempData.roomCurrentPlayer);
        PlayerPrefs.SetInt(playerPrefsKey + "_team_A_KillPoint", tempData.team_A_KillPoint);
        PlayerPrefs.SetInt(playerPrefsKey + "_team_B_KillPoint", tempData.team_B_KillPoint);
        PlayerPrefs.SetInt(playerPrefsKey + "_userKillPoint", tempData.userKillPoint);

        // Save player preferences immediately
        PlayerPrefs.Save();
    }
    private void OnPlayerJoinedRoomFail(JOIN_ROOM_FAILED fAILED)
    {
        try
        {
            SocketUIManager.Instance.ShowMassage(fAILED.message);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnPlayerJoinedRoomFail ---> {e.ToString()}");
        }
    }
    private void OnPlayerJoinedRoom(PLAYER_JOINED jOINED)
    {
        try
        {
            if (!jOINED.success)
            {
                return;
            }

            if (jOINED.data.player_id.Equals(SocketPlayerManager.Instance.player_Id))
            {
                SocketUIManager.Instance.OnJoinRoom();
            }
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnPlayerJoinedRoom ---> {e.ToString()}");
        }
    }
    private void OnCreateRoomFailed(CREATE_ROOM_FAILED fAILED)
    {
        try
        {
            SocketUIManager.Instance.ShowMassage(fAILED.message);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnCreateRoomFailed ---> {e.ToString()}");
        }
    }
    private void OnCreateRoomSuccess(CREATE_ROOM_SUCCESS sUCCESS)
    {
        try
        {
            //SocketDemoManager.Instance.UIManager.LobbyPanal.ShowStartBtn();
            SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser = sUCCESS.data.isMaster;
            SocketPlayerManager.Instance.helixPlayerInfo.roomID = sUCCESS.data.room_id;
            SocketPlayerManager.Instance.helixPlayerInfo.roomName = sUCCESS.data.room_name;

        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnCreateRoomSuccess ---> {e.ToString()}");
        }
    }

    private void OnJoinLobbySuccess(JOINED_LOBBY_SUCCESS obj)
    {
        try
        {
            playerID = obj.data.id.ToTrimString();
            SocketPlayerManager.Instance.player_Id = obj.data.id.ToTrimString();
            SocketPlayerManager.Instance.helixPlayerInfo.userID = obj.data.id.ToTrimString();
            SocketUIManager.Instance.SetScreen(SocketScreens.Lobby);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnJoinLobbySuccess ---> {e.ToString()}");
        }
    }
    public void JoinLobby(string player_name, string rigion = "")
    {

        SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
        {
            ["region"] = rigion,
            ["player_name"] = player_name,
        };
        //Debug.Log("Join Loabby Emit" + data["region"].ToString());
        //Debug.Log("Join Loabby Emit" + data["player_name"].ToString());
        socket.Emit(SocketStaticData.JOIN_LOBBY, data.ToString());

    }
    public void CreateRoom(string RoomName, int Maxplayer, string password = "", string RoomType = "", string level = "")
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["room_name"] = RoomName,
                ["max_players"] = Maxplayer,
                ["password"] = password,
                ["room_type"] = RoomType,
                ["level"] = level,
            };
            //Debug.Log("Create Room Emit " + data.ToString());
            socket.Emit(SocketStaticData.CREATE_ROOM, data.ToString());
            ReadyPlayer(true);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> CreateRoom ---> {e.ToString()}");
        }

    }
    public void JoinRoom(string RoomName, string password = "")
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["room_name"] = RoomName,
                ["password"] = password,
            };
            //Debug.Log("Join Room Emit " + data.ToString());
            socket.Emit(SocketStaticData.JOIN_ROOM, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> JoinRoom ---> {e.ToString()}");

        }

    }

    public void StartGame(string mapName)
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["mapname"] = mapName,
            };
            // Debug.Log("Rady Emit");
            //Debug.Log("Start Game Emit");
            socket.Emit(SocketStaticData.START_GAME, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> StartGame ---> {e.ToString()}");
        }

    }

    public void LeaveRoom()
    {
        try
        {
            Debug.Log("Emit LeaveRoom");
            socket.Emit(SocketStaticData.LEAVE_ROOM, string.Empty);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> LeaveRoom ---> {e.ToString()}");
        }
    }
    private void OnRoomList(ROOM_LIST_UPDATE uPDATE)
    {
        try
        {
            //Debug.Log("OnRoomList: " + uPDATE.data.Count);

            if (!uPDATE.success || uPDATE.data == null)
            {
                return;
            }
            //Debug.Log("RoomPlayerCount: " +uPDATE.data.Count);
            SocketUIManager.Instance.helixUIManager.LobbyPanal.GenerateRoomList(uPDATE);

        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnRoomList ---> {e.ToString()}");
        }
    }
    #region PLAYER LEFT
    private void OnPlayerLeft(PLAYER_LEFT lEFT)
    {
        try
        {
            //Debug.Log("Player Left ID:" + lEFT.data.left_player_id);
            Debug.Log("Player ID:" + playerID);
            if (lEFT.data.left_player_id.Equals(playerID))
            {
                GameplayManager.Instance?.MyPlayerLeft();
                SocketPlayerManager.Instance?.MyPlayerLeft();

            }
            else
            {
                GameplayManager.Instance?.OnOthorplayerLeft(lEFT.data);
                SocketPlayerManager.Instance?.OnOthorplayerLeft(lEFT.data);
            }

        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnPlayerLeft ---> {e.ToString()}");
        }

    }
    #endregion






    #region  PowerUp

    public void EmitCollectPowerUp(string powerUpID, string collectedPlayerID)
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["powerUpID"] = powerUpID,
                ["player_ID"] = collectedPlayerID
            };
            //Debug.Log("SHIELD_COLLECTED:::: " + SocketStaticData.SHIELD_COLLECTED + ":::::" + data.ToString());
            socket.Emit(SocketStaticData.POWERUP_COLLECTED, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> EmitCollectShieldPowerUp ---> {e.ToString()}");

        }
    }
    public void OnCollectPowerUp(POWERUP_DATA powerUpData)
    {

        //Debug.Log("------======>Shield Collected::" + powerUpData.powerUpID + ":::::::" + powerUpData.player_ID);

        if (GameplayManager.Instance && GameplayManager.Instance.allPowerUps.ContainsKey(powerUpData.powerUpID))
        {
            if (SocketPlayerManager.Instance.allPlayers.ContainsKey(powerUpData.player_ID))
                GameplayManager.Instance.allPowerUps[powerUpData.powerUpID].OnCollectBox(SocketPlayerManager.Instance.allPlayers[powerUpData.player_ID]);
            else
                GameplayManager.Instance.allPowerUps[powerUpData.powerUpID].OnCollectBox(null);

        }
    }

    public void EmitUsePrimaryPowerUp(string playerID, string powerUpID, bool isStartUse, bool isCompleteUse)
    {
        try
        {
            SimpleJSON.JSONNode data = new SimpleJSON.JSONObject
            {
                ["powerUpID"] = powerUpID,
                ["player_ID"] = playerID,
                ["isStartUse"] = isStartUse,
                ["isCompleteUse"] = isCompleteUse,

                //["password"] = password,
            };

            socket.Emit(SocketStaticData.POWERUP_USE, data.ToString());
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> EmitUseShieldPowerUp ---> {e.ToString()}");

        }
    }

    public void OnUsePrimaryPowerUp(POWERUP_DATA powerupData)
    {


        //Debug.Log("SHIELD_USE:::: " + powerupData.player_ID + ":::::" + powerupData.isStartUse.ToString());
        if (powerupData.isStartUse)
        {

            if (GameplayManager.Instance && GameplayManager.Instance.allPowerUps.ContainsKey(powerupData.powerUpID))
            {
                GameplayManager.Instance.allPowerUps[powerupData.powerUpID].OnStartUsePowerUp(powerupData.player_ID, false);
            }
        }
        if (powerupData.isCompleteUse)
        {

            if (GameplayManager.Instance && GameplayManager.Instance.allPowerUps.ContainsKey(powerupData.powerUpID))
            {
                GameplayManager.Instance.allPowerUps[powerupData.powerUpID].OnCompletedUsePowerUp(powerupData.player_ID, false);
            }
        }

    }

    #endregion

    private void OnDestroy()
    {
        try
        {
            //socket.Disconnect();
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnDestroy ---> {e.ToString()}");
        }
    }


    public static HelixPlayerInfo GetPlayerData(string userID)
    {
        string playerPrefsKey = $"Player_{userID}";

        HelixPlayerInfo playerData = new HelixPlayerInfo();
        playerData.userID = userID;
        playerData.userName = PlayerPrefs.GetString(playerPrefsKey + "_userName");
        playerData.userRegion = PlayerPrefs.GetString(playerPrefsKey + "_userRegion");
        playerData.roomID = PlayerPrefs.GetString(playerPrefsKey + "_roomID");
        playerData.roomName = PlayerPrefs.GetString(playerPrefsKey + "_roomName");
        playerData.isRoomHostUser = PlayerPrefs.GetInt(playerPrefsKey + "_isRoomHostUser", 0) == 1;
        playerData.teamA = PlayerPrefs.GetInt(playerPrefsKey + "_teamA", 0) == 1;
        playerData.teamB = PlayerPrefs.GetInt(playerPrefsKey + "_teamB", 0) == 1;
        playerData.userReadyRoom = PlayerPrefs.GetInt(playerPrefsKey + "_userReadyRoom", 0) == 1;
        playerData.roomMaxPlayer = PlayerPrefs.GetInt(playerPrefsKey + "_roomMaxPlayer");
        playerData.roomCurrentPlayer = PlayerPrefs.GetInt(playerPrefsKey + "_roomCurrentPlayer");
        playerData.team_A_KillPoint = PlayerPrefs.GetInt(playerPrefsKey + "_team_A_KillPoint");
        playerData.team_B_KillPoint = PlayerPrefs.GetInt(playerPrefsKey + "_team_B_KillPoint");
        playerData.userKillPoint = PlayerPrefs.GetInt(playerPrefsKey + "_userKillPoint");

        //Debug.Log(playerData);  

        return playerData;
    }
    private static System.Random random = new System.Random();
    public static string RandomString(int count)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Repeat(chars, count)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }



}

public static class String
{
    public static bool IsNullOrEmpty(string before)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Puts the string into the Clipboard.
    /// </summary>
    public static string ToTrimString(this string str)
    {
        return str.ToString().Trim('"');
    }
}
