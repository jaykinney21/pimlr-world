using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TS.Generics;
using UnityEngine;

public class RadialPauseMenuForSpaceship : MonoBehaviour
{
    public ButtonCustom buttonCustom;

    public ButtonVariousMethods buttonVariousMethods;

    public RadialButtonReferencer resumeArea, restartArea, quitArea, settingsArea;

    public Vector2 moveInput;

    bool IsAnimationOver = false;

    bool IsSelectedScreen = false;


    public GameObject RestartPanel;

    public GameObject QuitPanel;

    private void Awake () 
    {
        DOTween.SetTweensCapacity(1000, 1000);
    }

    private void OnEnable () 
    {
        IsSelectedScreen = true;

        StartCoroutine(StartAnimation());
    }

    private void OnDisable () 
    {
        IsAnimationOver = false;
    }

    float angle = 0f;
    private void Update () 
    {

        if (!IsAnimationOver || !IsSelectedScreen)
        {
            return;
        }

        if (this.gameObject.activeSelf)
        {
            moveInput.x = Input.mousePosition.x - (Screen.width / 2f);
            moveInput.y = Input.mousePosition.y - (Screen.height / 2f);
            moveInput.Normalize();

            if(moveInput != Vector2.zero)
            {
                angle = Mathf.Atan2(moveInput.y, -moveInput.x) / Mathf.PI;

                angle *= 180;

                if(angle < 0)
                {
                    angle += 360;
                }

                switch (angle)
                {
                    case float n when n >= 45 && n < 135:

                        resumeArea.HighlightButton();
                        restartArea.UnHighlightButton();
                        quitArea.UnHighlightButton();
                        settingsArea.UnHighlightButton();

                        break;
                    case float n when n >= 135 && n < 225:

                        resumeArea.UnHighlightButton();
                        restartArea.HighlightButton();
                        quitArea.UnHighlightButton();
                        settingsArea.UnHighlightButton();

                        break;
                    case float n when n >= 225 && n < 315:

                        resumeArea.UnHighlightButton();
                        restartArea.UnHighlightButton();
                        settingsArea.HighlightButton();
                        quitArea.UnHighlightButton();

                        break;
                    case float n when n >= 315 || n < 45:

                        resumeArea.UnHighlightButton();
                        restartArea.UnHighlightButton();
                        settingsArea.UnHighlightButton();
                        quitArea.HighlightButton();

                        break;
                }
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            switch (angle)
            {
                case float n when n >= 45 && n < 135:

                    IsSelectedScreen = false;

                    buttonCustom.CallPause();

                    break;
                case float n when n >= 135 && n < 225:

                    IsSelectedScreen = false;

                    RestartPanel.SetActive(true);

                    //buttonCustom.DisplayNewPage(13);

                    break;
                case float n when n >= 225 && n < 315:

                    IsSelectedScreen = false;

                    buttonCustom.DisplayNewPage(2);

                    break;
                case float n when n >= 315 || n < 45:

                    IsSelectedScreen = false;

                    QuitPanel.SetActive(true);

                    //buttonVariousMethods.OpenQuitIGPage();

                    break;
            }
        }
    }

    public void ClosePanel(string panelName) 
    {
        switch (panelName)
        {
            case "RestartPanel":

                RestartPanel.SetActive(false);

                IsSelectedScreen = true;

                break;
            case "QuitPanel":

                QuitPanel.SetActive(false);

                IsSelectedScreen = true;

                break;
        }

        IsSelectedScreen = true;
    }

    public void PanelTaskPerform (string panelName) 
    {
        switch (panelName)
        {
            case "RestartPanel":

                buttonCustom.RestartGame();

                break;
            case "QuitPanel":


                //ADD THE SCENE NAME THAT YOU WANT TO LOAD!!!
                //UILodingScreen loadingScreen = FindObjectOfType<UILodingScreen>();
                //loadingScreen.currentGameMode = GameMode.IdeaLabs;
                buttonCustom.QuitToHome();



                break;
        }
    }

    public IEnumerator StartAnimation () 
    {
        resumeArea.ResetForAnimation();
        restartArea.ResetForAnimation();
        settingsArea.ResetForAnimation();
        quitArea.ResetForAnimation();

        resumeArea.StartAnimation();

        yield return new WaitForSeconds(0f);

        restartArea.StartAnimation();

        settingsArea.StartAnimation();

        quitArea.StartAnimation();

        ResumeOriginalAnimation();
    }

    public void ResumeOriginalAnimation () 
    {
        IsAnimationOver = true;
    }

}

