using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelixUIManager : MonoBehaviour
{
    public List<GameObject> UIScreens = new List<GameObject>();
    public GameObject MassagePanal;
    public SocketLobbyUiManager LobbyPanal;
    public MapSelection mapSelection;
    public GameObject abs;
    public TMPro.TMP_InputField PlayerName;
  
    public GameObject _canvas;
    public UILodingScreen loadingScreen;
    public GameObject currentUIScreen;
    public GameObject paymentScreen;

    private void Start()
    {
        SocketUIManager.Instance.helixUIManager = this;
        SocketUIManager.Instance.UIScreens.Clear();
        SocketUIManager.Instance.UIScreens = UIScreens;
        SocketUIManager.Instance.MassagePanal = MassagePanal;
        SocketUIManager.Instance.LobbyPanal = LobbyPanal;
        SocketUIManager.Instance.mapSelection = mapSelection;
        SocketUIManager.Instance.abs = abs;
        SocketUIManager.Instance.PlayerName = PlayerName;
        
        SceneManagerScript.Instance._canvas = _canvas;
        SceneManagerScript.Instance.loadingScreen = loadingScreen;
        SocketUIManager.Instance.SetScreen(SocketScreens.Helix_mainMenu);
        if (paymentScreen != null)
            paymentScreen.SetActive(false);
    }

    public void JoinLobby()
    {
        SocketUIManager.Instance.JoinLobby();
        //HelixNpcCarManager.Instance.NpcStart();
    }

    public void OnBackHomePanel()
    {
        SocketUIManager.Instance.SetScreen(SocketScreens.Helix_mainMenu);
    }

    public void OnClickRoomCreatePopUpOn()
    {
        SocketUIManager.Instance.SetScreen(SocketScreens.Helix_Create_Room);
    }

}
