using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelCompletePopUp : MonoBehaviour
{
    public LoadingBarData[] loadingBarDatas;
    public TMPro.TextMeshProUGUI titelText;
    public TMPro.TextMeshProUGUI descriptionText;

    public string infiniteModeDescription;

    private void OnEnable()
    {
        if (Zone.Zone1 == GameExecutionManager.Instance.currentZoneMode)
        {
            titelText.text = loadingBarDatas[0]._Titel;
            descriptionText.text = loadingBarDatas[0]._Description;
        }
        if (Zone.ZoneBoss1 == GameExecutionManager.Instance.currentZoneMode)
        {
            titelText.text = loadingBarDatas[1]._Titel;
            descriptionText.text = loadingBarDatas[1]._Description;
        }
        if (Zone.Zone2 == GameExecutionManager.Instance.currentZoneMode)
        {
            titelText.text = loadingBarDatas[2]._Titel;
            descriptionText.text = loadingBarDatas[2]._Description;
        }
        if (Zone.ZoneBoss2 == GameExecutionManager.Instance.currentZoneMode)
        {
            titelText.text = loadingBarDatas[3]._Titel;
            descriptionText.text = loadingBarDatas[3]._Description;
        }
        if(Zone.InfiniteMode == GameExecutionManager.Instance.currentZoneMode)
        {
            titelText.text = "<color=green>Wave "+ InfiniteMode.Instance.currentWave+ " begins</color>";
            descriptionText.text = infiniteModeDescription;
        }

    }
}
[System.Serializable]
public class LevelCompletePopUpData
{
    public string _Titel;
    public string _Description;
}

public enum Zone
{
    Zone1,
    ZoneBoss1,
    Zone2, 
    ZoneBoss2,
    ChatWilly,
    InfiniteMode
}
