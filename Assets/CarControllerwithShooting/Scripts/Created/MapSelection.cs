using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelection : MonoBehaviour
{
    public string mapName;
   public void MapSelected(int mapInt)
    {
        //Debug.Log("GameStart");
        //SocketDemoManager.Instance.UIManager.SetScreen(SocketScreens.GameStarted);

        if (mapInt == 0)
        {
            //SceneManager.LoadScene(1);
            mapName = "Gameplay(terrain)";
            //Debug.Log("Map Name :- "+mapName);
        }
        else
        {
            //SceneManager.LoadScene(2);
            mapName = "Gameplay(colloseum)";
            //mapName = "Colloseum";
            // Debug.Log("Map Name :- "+mapName);
        }
        SocketUIManager.Instance.SetScreen(SocketScreens.Helix_Your_Team);
    }
}
