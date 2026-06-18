using TMPro;
using UnityEngine;

public class PlmrAvatarCreatePanel : MonoBehaviour
{
    public GameObject sceneStaticEU_Manager;
    public UIManager _UIManager;
    [SerializeField] TextMeshProUGUI coinText;
    // Start is called before the first frame update
    void Start()
    {
        sceneStaticEU_Manager = GameObject.Find("SceneManager");
        if (sceneStaticEU_Manager != null)
        {
            StartCoroutine(sceneStaticEU_Manager.GetComponent<SceneStaticEU_Manager>().Panel_Execution());
        }

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
            CoinManager.Instance.coinValueChanged -= OnCoinValueChange;
    }

    public void OnCoinValueChange(int value)
    {
        coinText.text = value.ToString();
    }
    public void OnAvaterDoneClick()
    {
        _UIManager.ShowMenu("PLMRMainMenuPanel");
    }
   
}
