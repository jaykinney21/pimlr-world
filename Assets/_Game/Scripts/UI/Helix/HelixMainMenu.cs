using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HelixMainMenu : MonoBehaviour
{
    private bool loadSceneInProgress;
    public UILodingScreen loadingScreen;

    [SerializeField] TextMeshProUGUI coinText;

    private void Start()
    {
       
        coinText.text = CoinManager.Instance.GetCoins().ToString();
    }

    private void OnEnable()
    {
        if (CoinManager.Instance)
            CoinManager.Instance.coinValueChanged += OnCoinValueChange;
        
    }
    private void OnDisable()
    {
        if (CoinManager.Instance)
            CoinManager.Instance.coinValueChanged-= OnCoinValueChange;
    }

    public void OnCoinValueChange(int value)
    {
        coinText.text = value.ToString();
    }
    public void LoadScene(string sceneName)
    {
        
        if (!loadSceneInProgress && !AuthManager.Instance.fullGame)
        {
            loadingScreen.gameObject.SetActive(true);
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
    }

    public void OnHomePanel()
    {
        //if (PlayerPrefs.GetInt("trialRound") == 0)
            SocketUIManager.Instance.SetScreen(SocketScreens.Home);
        //else
        //    SocketUIManager.Instance.helixUIManager.paymentScreen.SetActive(true);
    }
    public void OnBackHomePanel()
    {
        SocketUIManager.Instance.SetScreen(SocketScreens.Helix_mainMenu);
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {

        loadSceneInProgress = true;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Disable scene activation to allow loading to complete
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            // Update the loading slider value based on the loading progress
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9 is the max value for progress

            if (loadingScreen != null) loadingScreen.loadingSlider.value = progress;

            // Update loading text
            if (loadingScreen.loadingText != null)
                loadingScreen.loadingText.text = "Loading: " + (progress * 100f).ToString("F0") + "%";
            Debug.Log(loadingScreen.loadingText.text);
            // Check if the loading is almost complete (progress >= 0.9)
            if (asyncOperation.progress >= 0.9f)
            {
                // Enable scene activation to complete the loading
                asyncOperation.allowSceneActivation = true;
                yield return new WaitForSeconds(1f);
                //SocketUIManager.Instance.AllScreenActiveOff();
                loadSceneInProgress = false;
            }

            yield return null;

        }
    }
    public void OnGarageButtonClick()
    {
        try
        {
            SocketUIManager.Instance.SetScreen(SocketScreens.VehicleSelectionPanel);
        }
        catch (Exception e)
        {
            Debug.Log($"SocketNetworkManager ---> OnStartGame ---> {e.ToString()}");
        }
    }

    public void OpenUrlInDefaultBrowser(string targetGame)
    {
        //Debug.Log("%%%%"+targetGame);
        //if (targetGame == "Helix")
        //{
        //    Application.OpenURL(Constant.Helixlink);
        //}
        //else if (targetGame == "HumanityRocks")
        //{
        //    Application.OpenURL(Constant.Humanitylink);
        //}
        //else if (targetGame == "Pimlr")
        //{
        //    Application.OpenURL(Constant.PLMRlink);
        //}

        GameMode currentSelectedGame =(GameMode) Enum.Parse(typeof(GameMode), targetGame);
        
        AuthManager.Instance.currentGameMode = currentSelectedGame;

        if (AuthManager.Instance.fullGame)
        {
            if (currentSelectedGame == GameMode.Helix)
            {
                SceneManagerScript.Instance.LoadScene(Constant.Helix_Scene_Name);
            }
            else if (currentSelectedGame == GameMode.HumanityRocks)
            {
                SceneManagerScript.Instance.LoadScene(Constant.Humanity_Scene_Name);
            }
            else if (currentSelectedGame == GameMode.Pimlr)
            {
                SceneManagerScript.Instance.LoadScene(Constant.PLMR_Scene_Name);
            }
        }
        else
        {
            if (targetGame == "Helix")
            {
                Application.OpenURL(Constant.Helixlink);
            }
            else if (targetGame == "HumanityRocks")
            {
                Application.OpenURL(Constant.Humanitylink);
            }
            else if (targetGame == "Pimlr")
            {
                Application.OpenURL(Constant.PLMRlink);
            }
        }

    }
}
