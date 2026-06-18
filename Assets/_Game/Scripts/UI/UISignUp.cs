using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISignUp : MonoBehaviour
{
    [SerializeField]
    protected TMP_InputField username;
    [SerializeField]
    protected TMP_InputField email;
    [SerializeField]
    protected TMP_InputField password;
    [SerializeField]
    protected TMP_InputField confirmPassword;

    [SerializeField]
    protected Toggle termsOfUse;
    [SerializeField]
    protected Button registerButton;
    [SerializeField]
    protected GameObject loadingIndicator;



    bool ispasswordvisible, isconfirmpasswordvisible;
    [SerializeField] Image eye_password;
    [SerializeField] Image eye_confirm_password;
    public TMP_InputField passwordText;
    public TMP_InputField passwordConfirmText;

    public Sprite Eye, EyeClosed;


    [SerializeField] TextMeshProUGUI warningText;

    private void OnEnable()
    {
        username.text = "";
        email.text = "";
        password.text = "";
        confirmPassword.text = "";
        termsOfUse.isOn = false;

    }
    // Start is called before the first frame update
    void Start()
    {
        if (loadingIndicator != null)
        {
            loadingIndicator.SetActive(false);
        }
    }


    public void EyeBtnClicked(bool isconfirm)
    {

        if (!isconfirm)
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
        else
        {
            passwordConfirmText.gameObject.SetActive(false);
            isconfirmpasswordvisible = !isconfirmpasswordvisible;

            if (isconfirmpasswordvisible)
            {
                eye_confirm_password.sprite = Eye;
                passwordConfirmText.contentType = TMP_InputField.ContentType.Standard;
            }
            else
            {
                eye_confirm_password.sprite = EyeClosed;
                passwordConfirmText.contentType = TMP_InputField.ContentType.Password;
            }
            passwordConfirmText.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBackBtn()
    {
        StartCoroutine(WaitForscreenfadeOut());
    }

    public void OnSignUpButton()
    {

        if (string.IsNullOrEmpty(username.text))
        {
            ShowWarningText("Please Enter UserName");
            return;

        }
        else if (string.IsNullOrEmpty(email.text))
        {
            ShowWarningText("Please Enter Email Address");
            return;
        }
        else if (string.IsNullOrEmpty(password.text))
        {
            ShowWarningText("Please Enter Password");
            return;
        }
        else if (string.IsNullOrEmpty(confirmPassword.text))
        {
            ShowWarningText("Please Enter Confirm Password");
            return;
        }
        else if (password.text.Length < 8)
        {
            ShowWarningText("Please Enter minimum 8 characters");
            return;
        }
        else if (password.text != confirmPassword.text)
        {
            ShowWarningText("Could Match Password Or Confirm Password");
            return;
        }

        else
        {

            if (SceneManagerScript.Instance && SceneManagerScript.Instance.uiManager && SceneManagerScript.Instance.uiManager.Preloader != null)
                SceneManagerScript.Instance.uiManager.Preloader.SetActive(true);
            AuthManager.Instance.RegisterUser(username.text, email.text, password.text, confirmPassword.text);
        }

        //StartCoroutine(WaitForscreenfadeOut());
    }
    private IEnumerator WaitForscreenfadeOut()
    {
        //fadeOut
        UIFader fader = GameObject.FindObjectOfType<UIFader>();
        if (fader != null) fader.Fade(UIFader.FADE.FadeOut, 0.4f, 0.4f);
        yield return new WaitForSeconds(1f);
        SceneManagerScript.Instance.uiManager.ShowMenu("Login_Panel");
    }

    public void ShowWarningText(string msg)
    {
        warningText.text = "*" + msg;
        warningText.gameObject.SetActive(true);
    }
}
