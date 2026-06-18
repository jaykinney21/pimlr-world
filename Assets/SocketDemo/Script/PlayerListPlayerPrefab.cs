using UnityEngine;

public class PlayerListPlayerPrefab : MonoBehaviour
{
    public UnityEngine.UI.Image Player_Icon;
    public TMPro.TMP_Text PlayerName;
    public string playerId;
    public void SetPlayerDetail(string name, string Playerid, Sprite icon = null)
    {
        Debug.Log("inn" + name);
        PlayerName.text = name;
        playerId = Playerid;
        //this.gameObject.SetActive(true);
        if (icon != null)
            Player_Icon.sprite = icon;


    }

}
