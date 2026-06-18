using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MusicPurchasePanel : MonoBehaviour
{
    public MusicPurchaseEntry musicPurchaseEntry;


    public RectTransform musicPurchaseEntryHolder;

    public GameObject notEnoughCoinPanel;
    public GameObject notEnoughCoinPopup;

    private void Start()
    {
        MusicData[] musicData = SceneManagerScript.Instance.musicData;
        MusicPurchaseEntry musicPurchaseEntryspwan;
      
        for (int i = 0; i < musicData.Length; i++)
        {
            musicPurchaseEntryspwan = Instantiate(musicPurchaseEntry, musicPurchaseEntryHolder);
            musicPurchaseEntryspwan.OnAssignData(musicData[i]);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(musicPurchaseEntryHolder);
    }


    public void OnCloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    public void NotEnoughCoinMsg()
    {
        notEnoughCoinPopup.transform.localScale = Vector3.zero;
        notEnoughCoinPanel.SetActive(true);
        notEnoughCoinPopup.transform.DOScale(1, 0.5f).SetEase(Ease.OutFlash).OnComplete(() =>
         {
             notEnoughCoinPopup.transform.DOScale(0, 0.5f).SetEase(Ease.InFlash).SetDelay(2).OnComplete(() =>
             {
                 notEnoughCoinPanel.SetActive(false);
             });
         });
    }
}
