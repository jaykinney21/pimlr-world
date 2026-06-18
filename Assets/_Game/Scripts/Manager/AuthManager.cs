using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Collections;

using UnityEngine.SceneManagement;

public class AuthManager : Singleton<AuthManager>
{
    private const string baseUrl = "https://www.idea-labs.xyz/api/";

    public string message;
    //[HideInInspector]
    public string token;

    public GameMode currentGameMode = GameMode.IdeaLabs;

    public bool fullGame = true;

    public int enemyWashValue
    {
        get
        {
            return PlayerPrefs.GetInt(AchievementType.EnemyWash.ToString());
        }
        set
        {
            PlayerPrefs.SetInt(AchievementType.EnemyWash.ToString(), value);
        }
    }

    public int bossWashValue
    {
        get
        {
            return PlayerPrefs.GetInt(AchievementType.BossWash.ToString());
        }
        set
        {
            PlayerPrefs.SetInt(AchievementType.BossWash.ToString(), value);
        }
    }
    public int policeCarValue
    {
        get
        {
            return PlayerPrefs.GetInt(AchievementType.PoliceCar.ToString());
        }
        set
        {
            PlayerPrefs.SetInt(AchievementType.PoliceCar.ToString(), value);
        }
    }

    public AchievementReward achievementReward = AchievementReward.Nothing;

    // PIMLR #2: auto-skip the login screen on the main menu and boot straight into the living room.
    private void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "00_MainMenu")
            DummyLogin();
    }

    public void RegisterUser(string name, string email, string password, string passwordConfirmation)
    {
        StartCoroutine(RegisterUser(name, email, password, passwordConfirmation, HandleRegistrationResponse));
    }

    private void HandleRegistrationResponse(string response)
    {
        if (response != null)
        {
            // Parse and handle the registration response
            Debug.Log("Registration Response: " + response);
            JSONObject temp = new JSONObject(response);
            message = temp.GetField("message").ToString();
            //Debug.Log("Registration message: " + message);

            token = temp.GetField("token").ToString().Trim('"');
            //StartCoroutine(WaitForscreenfadeOut());
            SceneManagerScript.Instance.LoadScene();
        }
        else
        {
            DummyLogin();
            Debug.LogError("Registration failed.");
        }
    }

    // Function to handle user registration
    public IEnumerator RegisterUser(string targetname, string email, string password, string passwordConfirmation, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", targetname);
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("password_confirmation", passwordConfirmation);


        //Debug.Log(form.ToString() + ":::::::::::::::::" + email);
        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + "register", form))
        {
            yield return www.SendWebRequest();


            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseText = www.downloadHandler.text;

                callback(responseText);
                CoinManager.Instance.SetCoins(CoinManager.Instance.GetCoins() + 50);
            }
            else
            {
                JSONObject res = new JSONObject(www.downloadHandler.text);
                Debug.Log(res.Print());
                if (res.HasField("errors") && res["errors"].HasField("email"))
                {
                    SceneManagerScript.Instance.uISignUp.ShowWarningText(res["errors"]["email"][0].ToString().Trim('"'));
                }
                else
                {
                    SceneManagerScript.Instance.uISignUp.ShowWarningText("Enter Valied Details");
                }
                callback(null);
            }
            if (SceneManagerScript.Instance && SceneManagerScript.Instance.uiManager && SceneManagerScript.Instance.uiManager.Preloader != null)
                SceneManagerScript.Instance.uiManager.Preloader.SetActive(false);
        }
    }

    public void LoginUser(string email, string password)
    {
        StartCoroutine(LoginUser(email, password, HandleLoginResponse));
    }

    [ContextMenu("Dummy Login")]
    public void DummyLogin()
    {
        PlayerPrefs.SetString("username", "developer");
        PlayerPrefs.SetString("password", "developer");
        // PIMLR #2: load the living room directly (no-arg LoadScene() resolves to buildIndex+1 = the disabled 03_LevelSelection).
        SceneManagerScript.Instance.LoadScene("SceneStaticEU");
    }
    private void HandleLoginResponse(string response)
    {
        if (response != null)
        {
            // Parse and handle the registration response
            //Debug.Log("Login Response: " + response);
            JSONObject temp = new JSONObject(response);
            message = temp.GetField("message").ToString();
            //Debug.Log("Login message: " + message);

            token = temp.GetField("token").ToString().Trim('"');

            Debug.Log(token);
            //StartCoroutine(WaitForscreenfadeOut());





            // PIMLR #2: load the living room directly.
            SceneManagerScript.Instance.LoadScene("SceneStaticEU");

        }
        else
        {
            DummyLogin();
            Debug.LogError("Registration failed.");
            SceneManagerScript.Instance.uILogin.ShowWarningText("Enter Valied Details");
        }

        if (SceneManagerScript.Instance && SceneManagerScript.Instance.uiManager && SceneManagerScript.Instance.uiManager.Preloader != null)
            SceneManagerScript.Instance.uiManager.Preloader.SetActive(false);
    }

    // Function to handle user login
    public IEnumerator LoginUser(string email, string password, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);



        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + "login", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseText = www.downloadHandler.text;
                callback(responseText);

            }
            else
            {
                Debug.LogError("Login failed: " + www.error);
                callback(null);
            }
        }
    }

    // Function to handle user logout
    public IEnumerator LogoutUser(string token, System.Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(baseUrl + "logout"))
        {
            www.SetRequestHeader("Authorization", "Bearer " + token);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseText = www.downloadHandler.text;
                callback(responseText);
            }
            else
            {
                Debug.LogError("Logout failed: " + www.error);
                callback(null);
            }
        }
    }

    private IEnumerator WaitForscreenfadeOut()
    {
        //fadeOut
        UIFader fader = GameObject.FindObjectOfType<UIFader>();
        if (fader != null) fader.Fade(UIFader.FADE.FadeOut, 0.4f, 0.4f);
        yield return new WaitForSeconds(1f);
        // GameObject.FindObjectOfType<UIManager>().ShowMenu("Dialog Box Panel");
    }



    //public IEnumerator OnPostServerWithAuthToken(JSONObject data, string type, Action<string> callback)
    //{
    //    if (baseUrl == null)
    //    {
    //        Debug.LogError("URL is not");
    //    }
    //    string newURL = baseUrl + "/" + type;
    //    //Debug.Log(newURL + ", Data :: " + data);
    //    //With En?De
    //    JSONObject enData = new JSONObject();

    //    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(enData.Print());
    //    //Without En?De
    //    //byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(data.Print());
    //    var www = new UnityWebRequest(newURL, "POST");
    //    www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
    //    www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    //    www.SetRequestHeader("Content-Type", "application/json");
    //    www.SetRequestHeader("Authorization", "Bearer " + token);
    //    yield return www.SendWebRequest();
    //    if (www.isNetworkError)
    //    {
    //        NodeNetworkManager.Instance.EmitPlayerLogData("Error While Sending: " + www.error);
    //    }
    //    else
    //    {
    //        //With En?De
    //        JSONObject resData = new JSONObject(www.downloadHandler.text);
    //        var deDataText = AESEncryptionDecryption.Instance.AESDecryption(resData["data"].ToString().Trim('"'));
    //        JSONObject temp = new JSONObject(deDataText);
    //        resData.RemoveField("data");
    //        resData.AddField("data", temp);
    //        callback(resData.Print());
    //        //Whithout En?De
    //        //callback(www.downloadHandler.text);
    //    }
    //}







    #region Fetch User Data
    public IEnumerator GetUserData(string sendKey, System.Action<string> callback)
    {

        //Debug.Log("GET USER DATA:>>");
        WWWForm form = new WWWForm();
        form.AddField("meta_key", sendKey);


        Debug.Log(baseUrl + "get-activity");


        UnityWebRequest www = UnityWebRequest.Post(baseUrl + "get-activity", form);

        www.SetRequestHeader("Authorization", "Bearer " + token);
        Debug.Log("Bearer " + token);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {

            Debug.Log("Success::::::>" + www.downloadHandler.text);
            JSONObject responceData = new JSONObject(www.downloadHandler.text);



            if (responceData["user_activities"][0].HasField("meta_value"))
                callback(responceData["user_activities"][0]["meta_value"].ToString().Trim('"'));
            else
                callback(null);

        }
        else
        {

            Debug.Log("Error" + www.downloadHandler.text);
            callback(null);
        }

        www.Dispose();
    }





    public IEnumerator SetUserData(string sendKey, string value, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("meta_key", sendKey);
        form.AddField("meta_value", value);
        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + "save-activity", form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + token);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {

                JSONObject responceData = new JSONObject(www.downloadHandler.text);

                if (responceData.HasField("meta_value"))
                    callback(responceData["meta_value"].ToString().Trim('"'));
                else
                    callback(null);

            }
            else
            {
                callback(null);
            }
        }
    }
    #endregion


    public void PowerChange(AchievementReward newAchievementReward)
    {
        achievementReward = newAchievementReward;
        //Debug.LogError("PowerChange:>>" + newAchievementReward.ToString());
        ResetPowerups();
        if (newAchievementReward == AchievementReward.MovementSpeed)
        {
            if (GameExecutionManager.Instance)
            {
                GameExecutionManager.Instance.playerHandler.jUCharacterController.Speed = 6;
            }
        }
        //else if (newAchievementReward == AchievementReward.AmmoSize)
        //{

        //}
        else if (newAchievementReward == AchievementReward.VehicleSpeed)
        {
            if (GameExecutionManager.Instance)
            {
                GameExecutionManager.Instance.carController.VehicleEngine.MaxVelocity = 600;
                GameExecutionManager.Instance.carController.VehicleEngine.TorqueForce = 1200;
            }
        }
        //else if (newAchievementReward == AchievementReward.Defense)
        //{

        //}
        else if (newAchievementReward == AchievementReward.MaxHP)
        {
            if (GameExecutionManager.Instance)
            {
                GameExecutionManager.Instance.playerHandler.jUCharacterController.CharacterHealth.MaxHealth = 150;
                GameExecutionManager.Instance.playerHandler.jUCharacterController.CharacterHealth.Health = 150;
            }
        }
        else if (newAchievementReward == AchievementReward.FreezeRay)
        {

        }



    }
    public void ResetPowerups()
    {
        if (GameExecutionManager.Instance)
        {
            GameExecutionManager.Instance.playerHandler.jUCharacterController.Speed = 3;

            GameExecutionManager.Instance.carController.VehicleEngine.MaxVelocity = 300;
            GameExecutionManager.Instance.carController.VehicleEngine.TorqueForce = 600;
            GameExecutionManager.Instance.playerHandler.jUCharacterController.CharacterHealth.MaxHealth = 100;

        }
    }
}

