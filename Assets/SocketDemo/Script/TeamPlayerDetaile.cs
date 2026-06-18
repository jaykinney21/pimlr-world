using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPlayerDetaile : MonoBehaviour
{
    public TMPro.TMP_Text RoomIndex;
    public UnityEngine.UI.Image PlayerReady_Icon;
    public TMPro.TMP_Text PlayerName;
    public string playerId;
    public int playerscore;
   
    public void SetPlayerDetail(string name, string Playerid ,int roomcount, int score)
    {
        //Debug.Log("inn" + name);
        RoomIndex.text = $"{roomcount}.";
        PlayerName.text = name;
        playerId = Playerid;
        playerscore = score;
        //this.gameObject.SetActive(true);
       

        /*if (PlayerReady_Icon != null && isReady)
            PlayerReady_Icon.color = Color.green;*/
    }
}
