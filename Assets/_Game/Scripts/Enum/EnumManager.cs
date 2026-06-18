using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : MonoBehaviour
{
    public GameMode _CurrentGameMode;
    public GameScene _CurrentGameScene;

    void Awake()
    {
        //don't destroy
        DontDestroyOnLoad(gameObject);
    }
}
public enum GameScene
{
    None,
    MainMenu,
    Level1,
    Level2,
    Loading,
    CharacterSelection
}

public enum GameMode
{
    None,
    IdeaLabs,
    Pimlr,
    Helix,
    HumanityRocks,
}



