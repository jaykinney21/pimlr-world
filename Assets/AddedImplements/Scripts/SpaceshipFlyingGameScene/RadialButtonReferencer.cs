using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RadialButtonReferencer : MonoBehaviour
{
    public GameObject WholeButton;

    public Image ButtonBGImage, ButtonRoundImage;

    [Space]

    public Vector3 initialPosition;

    public Vector3 originalPosition;

    public Vector3 newPosition;

    public void ResetForAnimation () 
    {
        WholeButton.transform.localScale = Vector3.zero;
    }

    public void StartAnimation () 
    {
        WholeButton.transform.localPosition = initialPosition;

        WholeButton.transform.DOLocalMove(originalPosition, 0.55f);

        WholeButton.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.8f);

    }

    public void HighlightButton () 
    {
        WholeButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f);

        WholeButton.transform.DOLocalMove(newPosition, 0.5f);

        ButtonBGImage.DOColor(Color.green, 0.5f);

        ButtonRoundImage.DOColor(Color.green, 0.5f);
    }

    public void UnHighlightButton () 
    {
        WholeButton.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f);

        WholeButton.transform.DOLocalMove(originalPosition, 0.5f);

        ButtonBGImage.DOColor(Color.white, 0.5f);

        ButtonRoundImage.DOColor(Color.white, 0.5f);
    }
}
