using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIDialogBox : MonoBehaviour
{

    public TMPro.TextMeshProUGUI title;
    public TMPro.TextMeshProUGUI text;
    public Image icon;
    public Button button;
    public AuthManager _Auth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnEnable()
    {
        if (AuthManager.Instance == null)
        {
            return;
        }
        Debug.Log(" _Auth.message =" + AuthManager.Instance.message.ToString());

        title.text = AuthManager.Instance.message;
        text.text = "You can Now Go inside a Game!";
    }

    public void OnDisable()
    {
        title.text ="";
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
