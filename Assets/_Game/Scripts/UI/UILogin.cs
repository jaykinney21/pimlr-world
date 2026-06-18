using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    protected TMP_InputField username;
    [SerializeField]
    protected TMP_InputField password;
    [SerializeField]
    protected Toggle rememberMe;
    [SerializeField]
    protected Button loginButton;
    [SerializeField]
    protected GameObject loadingIndicator;




    bool ispasswordvisible;
    [SerializeField] Image eye_password;
    public TMP_InputField passwordText;

    [SerializeField] Sprite Eye, EyeClosed;

    [SerializeField] internal TextMeshProUGUI warningText;


    private void OnEnable()
    {
        username.text="";
        password.text="";
    }
    // Start is called before the first frame update
    void Start()
    {

        username.text = PlayerPrefs.GetString("username", string.Empty);
        password.text = PlayerPrefs.GetString("password", string.Empty);

        //Debug.Log($"username ={username.text}/password ={password.text}");

        if (rememberMe != null)
        {
            rememberMe.isOn = string.IsNullOrEmpty(username.text) ? false : true;
        }

        if (loadingIndicator != null)
        {
            loadingIndicator.SetActive(false);
        }

        if (!string.IsNullOrEmpty(username.text) && !string.IsNullOrEmpty(password.text))
        {
            OnLogin();
        }
        else
            username.Select();
    }

  
    public void EyeBtnClicked()
    {
        passwordText.gameObject.SetActive(false);
        ispasswordvisible = !ispasswordvisible;

        if (ispasswordvisible)
        {
            eye_password.sprite = Eye;
            passwordText.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            eye_password.sprite = EyeClosed;
            passwordText.contentType = TMP_InputField.ContentType.Password;
        }
        passwordText.gameObject.SetActive(true);


    }
    public void OnLoginUsingFields()
    {
        //_Auth.LoginUser(username.text, password.text);
        /* LoginManager.LoginAccount(username.text, password.text);
         loginButton.interactable = false;
         if (loadingIndicator != null)
         {
             loadingIndicator.SetActive(true);
         }*/
        OnLogin();
    }
    private void OnLogin()
    {
        if (string.IsNullOrEmpty(username.text))
        {
            ShowWarningText("Please Enter UserName");
            return;

        }
        else if (string.IsNullOrEmpty(password.text))
        {
            ShowWarningText("Please Enter PASSWORD");
            return;
        }
        else
        {

            SceneManagerScript.Instance.uiManager.Preloader.SetActive(true);

            // Save username and password
            if (rememberMe != null && rememberMe.isOn)
            {
                PlayerPrefs.SetString("username", username.text);
                PlayerPrefs.SetString("password", password.text);
            }
            else
            {
                PlayerPrefs.DeleteKey("username");
                PlayerPrefs.DeleteKey("password");
            }

            AuthManager.Instance.LoginUser(username.text, password.text);
        }
    }

    public void DummyLogin()
    {

    }

    public void OnCreateAccountBtn()
    {
        StartCoroutine(WaitForscreenfadeOut());
    }


    private IEnumerator WaitForscreenfadeOut()
    {
        //fadeOut

        if (SceneManagerScript.Instance.uiManager.UI_fader != null) SceneManagerScript.Instance.uiManager.UI_fader.Fade(UIFader.FADE.FadeOut, 0.4f, 0.4f);
        yield return new WaitForSeconds(1f);
        SceneManagerScript.Instance.uiManager.ShowMenu("SignUp_Panel");
    }

    public void ShowWarningText(string msg)
    {
        warningText.text = "*" + msg;
        warningText.gameObject.SetActive(true);
    }


}
