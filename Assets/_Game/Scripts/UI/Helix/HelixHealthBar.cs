using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CarControllerwithShooting;

public class HelixHealthBar : MonoBehaviour
{
    [Header("UI Health Bar Settings")]
    [SerializeField] private HelixPlayerHealth HealthComponent;
    public CarController controller;
    //[SerializeField] private bool IsPlayerHealthBar = true;
    [SerializeField] private Image HealthBarImage;
    [SerializeField] private Image HealthBarIconImage;
    [SerializeField] private float Speed = 6;
    [SerializeField] private TextMeshProUGUI HealthPointsText;

    [Header("Health Bar Color Change")]
    [SerializeField] private Color EmptyHPColor = Color.red;
    [SerializeField] private Color FullHPColor = Color.green;
    [SerializeField] private Color HPHealingColor = Color.cyan;
    [SerializeField] private Color HPLossColor = Color.yellow;
    [SerializeField] private bool ChangeHPTextColorToo = true;

    private float oldFillAmount;

    void Start()
    {
        //if (IsPlayerHealthBar)
        //{
        //    GameObject pl = GameObject.FindGameObjectWithTag("Car");
        //    HealthComponent = pl.GetComponent<HelixPlayerHealth>();
        //}

        oldFillAmount = HealthBarImage.fillAmount;
    }

    public HelixPlayerHealth helixPlayerHealth
    {
        set { HealthComponent = value; }

        get { return HealthComponent; }
    }



    void Update()
    {
        if (HealthComponent == null || HealthBarImage == null)
        {
            HealthPointsText.text = ($"<size=25>{0}</size>/<size=35>{100}</size>");
            HealthBarImage.fillAmount = 0;
            return;
        }

        float healthValueNormalized = HealthComponent.Health / HealthComponent.MaxHealth;
        //float healthValueNormalized = controller.Health / controller.MaxHealth;
        HealthBarImage.fillAmount = Mathf.MoveTowards(HealthBarImage.fillAmount, healthValueNormalized, Speed * Time.deltaTime);

        HealthBarImage.color = Color.Lerp(EmptyHPColor, FullHPColor, HealthBarImage.fillAmount);

        if (HealthPointsText != null)
        {
            //HealthPointsText.text = HealthComponent.Health.ToString("000") + "/" + HealthComponent.MaxHealth;
            HealthPointsText.text = ($"<size=25>{HealthComponent.Health}</size>/<size=35>{HealthComponent.MaxHealth}</size>");
            if (ChangeHPTextColorToo) HealthPointsText.color = Color.Lerp(HealthBarImage.color, Color.white, 0.6f);
            HealthBarIconImage.color = HealthPointsText.color;
        }
        if (oldFillAmount != HealthBarImage.fillAmount)
        {
            //Health Healing
            if (oldFillAmount < HealthBarImage.fillAmount)
            {
                HealthBarImage.color = HPHealingColor;
                HealthBarIconImage.color = HPHealingColor;
            }
            //Health Loss
            if (oldFillAmount > HealthBarImage.fillAmount)
            {
                HealthBarImage.color = HPLossColor;
                HealthBarIconImage.color = HPLossColor;

            }

            oldFillAmount = HealthBarImage.fillAmount;
        }

    }
}
