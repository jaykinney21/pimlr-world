using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILodingScreen : MonoBehaviour
{
    public LoadingBarData[] loadingBarDatas;
    public Image backGroundimg;
    public TMPro.TextMeshProUGUI titelText;
    public TMPro.TextMeshProUGUI descriptionText;
    public TMPro.TextMeshProUGUI loadingText;
    public Slider loadingSlider;

  

    private void OnEnable()
    {
        if (AuthManager.Instance)
        {
            if (GameMode.IdeaLabs == AuthManager.Instance.currentGameMode)
            {
                titelText.text = loadingBarDatas[0]._Titel;
                descriptionText.text = loadingBarDatas[0]._Description;
            }
            if (GameMode.Pimlr == AuthManager.Instance.currentGameMode)
            {
                titelText.text = loadingBarDatas[1]._Titel;
                descriptionText.text = loadingBarDatas[1]._Description;
            }
            if (GameMode.Helix == AuthManager.Instance.currentGameMode)
            {
                titelText.text = loadingBarDatas[2]._Titel;
                descriptionText.text = loadingBarDatas[2]._Description;
            }
            if (GameMode.HumanityRocks == AuthManager.Instance.currentGameMode)
            {
                titelText.text = loadingBarDatas[3]._Titel;
                descriptionText.text = loadingBarDatas[3]._Description;
            }
        }
    }

}

[System.Serializable]

public class LoadingBarData
{
    public string _Titel;
    public string _Description;
}

