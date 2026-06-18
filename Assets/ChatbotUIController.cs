using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JUTPS.ItemSystem;
using JUTPS;

public class ChatbotUIController : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button sendButton;
    public TextMeshProUGUI chatText;
    public ScrollRect scrollRect; // Reference to the ScrollRect

    private FlowiseAPI flowiseApi;

    [SerializeField] AiChatInteraction aiChatInteraction;

    int AnswerCountNumber = 0;

    // PIMLR #3: baked scripted dialogue replaces the Flowise/AI-API chat.
    // OWNER: finalize this copy. After the last line is buttoned through, the water gun unlocks.
    [TextArea] public string[] bakedLines = new string[]
    {
        "I've detected your haters from the BM Hive gathering around this area. You may need to protect yourself with a hydraulic tool.",
        "Take this water gun - it's loaded with soapy water, the one thing these haters can't stand.",
        "Head downtown and wash them off the streets. I'll be tracking your progress - good luck."
    };
    int currentLine = 0;

    void Start()
    {
        // PIMLR #3: scripted dialogue. The Send/Next button advances lines; after the last line the water gun unlocks.
        chatText.text = "\n";
        if (sendButton != null)
        {
            sendButton.onClick.RemoveAllListeners();
            sendButton.onClick.AddListener(AdvanceDialogue);
        }
        currentLine = 0;
        ShowCurrentLine();
    }

    void ShowCurrentLine()
    {
        if (currentLine >= 0 && currentLine < bakedLines.Length)
            AppendToChat($"<color=yellow>Sirihanna: {bakedLines[currentLine]} </color>\n");
    }

    // Advance to the next scripted line; unlock the water gun after the last one.
    public void AdvanceDialogue()
    {
        currentLine++;
        if (currentLine < bakedLines.Length)
            ShowCurrentLine();
        else
            EndDialogBoxAndSpawnZonbies();
    }

    [System.Obsolete]
    private void Speak(string text)
    {
        SceneManagerScript.Instance.musicSystem.musicSystem.volume = 0.1f;
        Application.ExternalEval($"speak('{text.Replace("'", "\\'")}');");
    }

    [System.Obsolete]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            aiChatInteraction.OnClosePanel();
        }
    }

    // Legacy Flowise/API send path (no longer wired to the button; kept for reference).
    void SendMessage()
    {
        string userMessage = inputField.text;
        if (flowiseApi != null && !string.IsNullOrWhiteSpace(userMessage))
        {
            flowiseApi.SendQuestion(userMessage);
            AppendToChat($"<color=green>You: {userMessage}</color> \n");
            inputField.text = "";
        }
    }

    [System.Obsolete]
    // Legacy Flowise response handler (no longer subscribed).
    private void HandleResponse(string response)
    {
        AppendToChat($"<color=yellow>Sirihanna:{response} </color>\n");
        Speak(response);
        AnswerCountNumber++;
        if (AnswerCountNumber == 4)
        {
            EndDialogBoxAndSpawnZonbies();
        }
    }

    public void AppendToChat(string message)
    {
        chatText.text += $"{message}\n";
        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        // Wait for end of frame so that the UI elements can update their positions
        yield return new WaitForEndOfFrame();
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    [System.Obsolete]
    void OnDestroy()
    {
        if (flowiseApi != null)
        {
            flowiseApi.OnResponseReceived -= HandleResponse;
        }
    }



    UILevelCompletePopUp uILevelCompletePopUp;

    [System.Obsolete]
    //[ContextMenu("EndDialogBoxAndSpawnZonbies")]
    public void EndDialogBoxAndSpawnZonbies()
    {
        Debug.Log(":::>>>>EndDialogBoxAndSpawnZonbies");

        SceneManagerScript.Instance.goalPanel.OnCompleteGoal(GoalList.ChatWithSirihanna);

        //DialogueManager.instance.EndDialogue();
        if (uILevelCompletePopUp == null)
        {

            uILevelCompletePopUp = SceneManagerScript.Instance.uiManager.UIMenus[5].UI_Gameobject.GetComponent<UILevelCompletePopUp>();
        }

        if (uILevelCompletePopUp != null)
        {

            GameExecutionManager.Instance.currentZoneMode = Zone.Zone1;
            PlayerPrefs.SetString("currentZoneMode", GameExecutionManager.Instance.currentZoneMode.ToString());

            JUGameManager.InstancedPlayer.StartCoroutine(WaitForscreenfadeOut());
        }

        aiChatInteraction.OnClosePanel();

        aiChatInteraction.transform.parent.gameObject.SetActive(false);

        aiChatInteraction.characterController.GetComponent<ItemSwitchManager>().IsPlayer = true;

        GameExecutionManager.Instance.Zone1Start();


        //gameme

        if (CoinManager.Instance)
            CoinManager.Instance.SetCoins(CoinManager.Instance.GetCoins() - 40);

    }
    private IEnumerator WaitForscreenfadeOut()
    {
        //fadeOut
        UIFader fader = GameObject.FindObjectOfType<UIFader>();
        if (fader != null) fader.Fade(UIFader.FADE.FadeOut, 0.4f, 0.4f);
        yield return new WaitForSeconds(1f);
        SceneManagerScript.Instance.uiManager.ShowMenu("Zone Panel");
        yield return new WaitForSeconds(3f);
        SceneManagerScript.Instance.uiManager.ShowMenu("JUTPS Interface");
    }

}
