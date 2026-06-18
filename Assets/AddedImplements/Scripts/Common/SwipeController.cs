using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour
{
    public int maxLevelPages;
    public int currentPage;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPageRect;

    [SerializeField] Ease tweenType;
    float dragThreshould;

    [SerializeField] Button next, previous;


    private void Awake()
    {
        currentPage = 1;
        targetPosition = levelPageRect.localPosition;
        dragThreshould = Screen.width / 15;
        UpdateArrowButton();
    }
    public void Next()
    {
        if (currentPage < maxLevelPages) 
        {
            currentPage++;
            targetPosition += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {

            currentPage--;
            targetPosition -= pageStep;
            MovePage();
        }
    }

    void MovePage()
    {
        levelPageRect.DOLocalMoveX(targetPosition.x, 0.3f);
        UpdateArrowButton();
    }

    void UpdateArrowButton()
    {
        next.interactable = true;
        previous.interactable = true;
        if (currentPage == 1)
        {
            previous.interactable = false;
        }
        else if (currentPage == maxLevelPages)
        {
            next.interactable = false;
        }
    }

    public void OnEndDrag (PointerEventData eventData) {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x)
            {
                Previous();
            }
            else
            {
                Next();
            }
        }
        else
        {
            MovePage();
        }
    }
}
