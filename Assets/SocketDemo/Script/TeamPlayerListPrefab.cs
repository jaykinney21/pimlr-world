using UnityEngine;
using UnityEngine.UI;

public class TeamPlayerListPrefab : MonoBehaviour
{
    public TMPro.TMP_Text RoomIndex;
    public Image Player_Icon;
    public TMPro.TMP_Text PlayerName;
    public string playerId;
    public Button teamA, teamB;

    public void SetDetail(string text, string player_ID,int roomcount)
    {
        PlayerName.text = text;
        playerId = player_ID;
        RoomIndex.text = $"{roomcount}.";

        if (player_ID.Equals(SocketPlayerManager.Instance.player_Id))
        {
            teamA.interactable = true;
            teamB.interactable = true;
        }
        else
        {
            teamA.interactable = false;
            teamB.interactable = false;
        }
    }

    public void OnSelect_Team_A_Btn()
    {
        SocketPlayerManager.Instance.helixPlayerInfo.teamA = true;
        //SocketNetworkManager.Instance.Team_Select("Team A", true, false);
        TeamSelection.Instance.TeamSwitch(0);
    }

    public void OnSelect_Team_B_Btn()
    {
        SocketPlayerManager.Instance.helixPlayerInfo.teamB = true;
        //SocketNetworkManager.Instance.Team_Select("Team B", false, true);
        TeamSelection.Instance.TeamSwitch(1);
    }
}

