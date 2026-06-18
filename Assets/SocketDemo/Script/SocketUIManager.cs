using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SocketScreens : int
{
    Home = 0,
    Lobby = 1,
    GameStarted = 2,
    //playPanal = 3,
    teampanel = 3,
    MapSelectionPanel = 4,
    VehicleSelectionPanel = 5,
    Helix_mainMenu = 6,
    Helix_PreGame = 7,
    Helix_Create_Room = 8,
    Helix_Join_Room = 9,
    Helix_Your_Team = 10,
}


public class SocketUIManager : MonoBehaviour
{
    public static SocketUIManager Instance { get; private set; }
    public List<GameObject> UIScreens = new List<GameObject>();
    public GameObject MassagePanal;
    public SocketLobbyUiManager LobbyPanal;
    public MapSelection mapSelection;
    public GameObject abs;
    public TMPro.TMP_InputField PlayerName;
    //private SocketPlayerManager playerManager;
    public HelixUIManager helixUIManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
        // AllScreenActiveOff();
    }

    private void Start()
    {
        Instance = this;
        SetScreen(SocketScreens.Helix_mainMenu);
    }

    public void OnBackHomePanel()
    {
        SetScreen(SocketScreens.Helix_mainMenu);
    }


    public void SetScreen(SocketScreens screen)
    {

        for (int i = 0; i < UIScreens.Count; i++)
        {
            UIScreens[i].SetActive(false);
        }

        UIScreens[(int)screen].SetActive(true);
        helixUIManager.currentUIScreen = UIScreens[(int)screen];
    }
    public void AllScreenActiveOff()
    {
        for (int i = 0; i < UIScreens.Count; i++)
        {
            UIScreens[i].SetActive(false);
        }

    }


    public void OnJoinRoom()
    {
        for (int i = 0; i < SocketPlayerManager.Instance.allPlayers.Count; i++)
        {
            string _i = i.ToString();
            if (SocketPlayerManager.Instance.allPlayers[_i].helixPlayerInfo.isRoomHostUser)
            {
                LobbyPanal.ShowStartBtn();
            }
            else if (SocketPlayerManager.Instance.allPlayers[_i].helixPlayerInfo.userReadyRoom)
            {
                LobbyPanal.ShowReadyBtn();
            }
        }

    }

    public void JoinLobby()
    {
        try
        {
            //Debug.Log("JoinLobby");
            // Debug.Log("playertext: " + PlayerName.text + "|| socketDemoManager :" + SocketDemoManager.Instance);
            if (PlayerName.text != "" && SocketPlayerManager.Instance != null)
            {
                SocketPlayerManager.Instance.playerName = PlayerName.text;
                SocketPlayerManager.Instance.helixPlayerInfo.userName = PlayerName.text;
            }
            else
            {
                //Debug.Log("Else JoinLobby");
                SocketPlayerManager.Instance.playerName = SystemInfo.deviceUniqueIdentifier;
            }
            SocketNetworkManager.Instance.JoinLobby(SocketPlayerManager.Instance.playerName, "In");
        }
        catch (Exception e)
        {
            Debug.Log("-----> SockerUIManager -----> JoinLobby " + e.ToString());
        }
    }

    public void ShowMassage(string massage)
    {
        if (MassagePanal != null)
        {
            MassagePanal.GetComponent<TMPro.TMP_Text>().text = massage;
            MassagePanal.SetActive(true);
            //Debug.Log("Massage Show " + massage);
            SocketNetworkManager.Instance.StartCoroutine(waitAndCloseMassage());
        }
    }

    private IEnumerator waitAndCloseMassage()
    {

        yield return new WaitForSeconds(5f);
        if (MassagePanal != null)
        {
            MassagePanal?.SetActive(false);
        }

    }

    public void OnSceneLoad(string sceneName)
    {
        if (AuthManager.Instance.fullGame)
        {
            UILodingScreen loadingScreen = UIScreens[2].gameObject.GetComponent<UILodingScreen>();
            loadingScreen.gameObject.SetActive(true);
            SocketNetworkManager.Instance.OnSceneLoad(sceneName);
        }
    }
}
