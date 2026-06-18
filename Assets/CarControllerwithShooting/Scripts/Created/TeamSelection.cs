using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class TeamSelection : MonoBehaviour
{

    public static TeamSelection Instance { get; private set; }
    public PlayerListPlayerPrefab teamPlayerPrefab;
    public Helix_Your_Team helix_your_team;
    public Transform teamA, teamB;

    [Header("Team List")]
    public List<PlayerListPlayerPrefab> team_AList;
    public List<PlayerListPlayerPrefab> team_BList;

    public PLAYER_LIST_UPDATE playerlist;
    string teamSelect;
    bool isfound;
    string currentName;
    string tName;
    #region Unity Callbacks
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }
   
    #endregion


    #region Team Selection Setup
    /// <summary>
    /// this method works on button clicked by any player on team buttons
    /// </summary>
    /// <param name="team"></param>
    public void TeamSwitch(int team)
    {

        if (team == 0)
        {
            teamSelect = "Team A";
            SocketNetworkManager.Instance.Team_Select(SocketNetworkManager.Instance.playerID,teamSelect, true, false);
        }
        else if (team == 1)
        {
            teamSelect = "Team B";
            SocketNetworkManager.Instance.Team_Select(SocketNetworkManager.Instance.playerID,teamSelect, false, true);
        }

        if (SocketPlayerManager.Instance.helixPlayerInfo.isRoomHostUser)
        {
            SocketUIManager.Instance.SetScreen(SocketScreens.MapSelectionPanel);

            helix_your_team.startButton.gameObject.SetActive(true);
            helix_your_team.ReadyButton.gameObject.SetActive(false);
        }
        else
        {
            SocketUIManager.Instance.SetScreen(SocketScreens.Helix_Your_Team);
            helix_your_team.startButton.gameObject.SetActive(false);
            helix_your_team.ReadyButton.gameObject.SetActive(true);
        }



    }


    #region Booleans to get the presence of the player in team list
    public bool ThereA(string playid)
    {
        for (int i = 0; i < helix_your_team.team_AList.Count; i++)
        {
            if (helix_your_team.team_AList[i].playerId.Equals(playid))
            {
                //Debug.Log("therea");
                //isfound = true;
                return true;
            }

        }
        return false;
    }

    public bool ThereB(string playid)
    {
        for (int i = 0; i < helix_your_team.team_BList.Count; i++)
        {
            if (helix_your_team.team_BList[i].playerId.Equals(playid))
            {
                //Debug.Log("thereb");
                //isfound = true;
                return true;
            }

        }
        return false;
    }
    #endregion


    /// <summary>
    /// this method is used to update the team list on data recieved by server
    /// it updates the latest player joined to the team 
    /// </summary>
    /// <param name="teamdata"></param>
    public void TeamLIstUpdate(ON_TEAM_SELECTION teamdata)
    {
        //foreach (HelixPlayerInfo item in SocketPlayerManager.Instance.helixPlayerInfoList)
        //{
        //    if (item.userID == teamdata.data.player_id)
        //    {
                currentName = SocketPlayerManager.Instance.helixPlayerInfoList[teamdata.data.player_id].userName;
        //    }
        //}

        if (teamdata.data.teamname == "Team A")
        {
            TeamPlayerDetaile t = Instantiate(helix_your_team.teamAPlayer, teamA);
            //t.SetPlayerDetail(teamdata.data.player_name, teamdata.data.player_id, teamdata.data.playerScore);
            helix_your_team.team_AList.Add(t);
            helix_your_team.teamsPlayer.Add(t);
        }
        else if (teamdata.data.teamname == "Team B")
        {
            TeamPlayerDetaile t = Instantiate(helix_your_team.teamBPlayer, teamB);
            //t.SetPlayerDetail(teamdata.data.player_name, teamdata.data.player_id, teamdata.data.playerScore);
            helix_your_team.team_BList.Add(t);
            helix_your_team.teamsPlayer.Add(t);
        }
        else
        {
            Debug.Log("none in simple first process");
        }

        //CoreTeamUpdate();

    }


    /// <summary>
    ///  // used to run the loop to instiate prevoius players joined or selected team before current or before last one player selected
    /// </summary>
    public void CoreTeamUpdate()
    {
        try
        {
            foreach (HelixPlayerInfo item in SocketPlayerManager.Instance.helixPlayerInfoList.Values)
            {

                if (item.teamA == true)
                {
                    if (!ThereA(item.userID))
                    {

                        TeamPlayerDetaile t = Instantiate(helix_your_team.teamAPlayer, teamA);
                        t.PlayerName.text = item.userID;
                        helix_your_team.team_AList.Add(t);
                        helix_your_team.teamsPlayer.Add(t);

                    }

                }
                else if (item.teamB == true)
                {
                    if (!ThereB(item.userID))
                    {

                        TeamPlayerDetaile t = Instantiate(helix_your_team.teamBPlayer, teamB);
                        t.PlayerName.text = item.userName;
                        helix_your_team.team_BList.Add(t);
                        helix_your_team.teamsPlayer.Add(t);


                    }
                }

            }
        }
        catch (System.Exception)
        {
            Debug.Log("TeamSelection -----> CoreTeamUpdate ");
        }

    }

    /// <summary>
    /// used to update the team list on player leave
    /// </summary>
    /// <param name="newownerId"></param>
    public void UpdateTeamOnLeave(string leftPlayerId)
    {

        //if (SocketDemoManager.Instance.PlayerManager.playerList.data.ContainsKey(leftPlayerId))
        //{
        //        tName = SocketDemoManager.Instance.PlayerManager.playerList.data[leftPlayerId].teamname;
        //}

        //foreach (HelixPlayerInfo p in SocketPlayerManager.Instance.helixPlayerInfoList)
        //{
        //    if (p.userID == leftPlayerId)
        //    {
                tName = SocketPlayerManager.Instance.helixPlayerInfoList[leftPlayerId].userName;
        //        break;
        //    }
        //}

        if (tName == "Team A")
        {
            for (int i = 0; i < helix_your_team.team_AList.Count; i++)
            {
                if (helix_your_team.team_AList[i].playerId.Equals(leftPlayerId))
                {
                    Destroy(helix_your_team.team_AList[i].gameObject);
                    helix_your_team.team_AList.RemoveAt(i);
                    helix_your_team.teamsPlayer.RemoveAt(i);

                }

            }
        }
        else if (tName == "Team B")
        {
            for (int i = 0; i < helix_your_team.team_BList.Count; i++)
            {
                if (helix_your_team.team_BList[i].playerId.Equals(leftPlayerId))
                {
                    Destroy(helix_your_team.team_BList[i].gameObject);
                    helix_your_team.team_BList.RemoveAt(i);
                    helix_your_team.teamsPlayer.RemoveAt(i);

                }

            }
        }
        else
        {
            Debug.Log("nonew team");
        }

    }
    #endregion

}

