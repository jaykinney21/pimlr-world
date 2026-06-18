using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SocketPregame : MonoBehaviour
{
    public SocketLobbyUiManager socketLobbyUiManager;
    [SerializeField] TextMeshProUGUI playerCount;
    [SerializeField] TextMeshProUGUI startingIn;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        startTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(startTime / 60);
        int seconds = Mathf.FloorToInt(startTime % 60);

        playerCount.text = $"Players Found: {0}/10";

        if (Helix_Team_Selection.Instance)
            playerCount.text = $"Players Found: {Helix_Team_Selection.Instance.PlayerList.Count}/10";

        startingIn.text = string.Format("starting in: {0:00}:{1:00}", minutes, seconds);
    }
}
