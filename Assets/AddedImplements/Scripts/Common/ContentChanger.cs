
using UnityEngine;
using UnityEngine.UI;

public class ContentChanger : MonoBehaviour
{
    public GameObject shirtColorCatalogue;

    public GameObject hairColorCatalogue;

    public GameObject pantsColorCatalogue;

    public GameObject shoesColorChangeCatalogue;

    public GameObject[] colorsCatalogue;
    public Image[] imageOfButtons;


    [SerializeField]
    changeColor changeColor;

    [SerializeField]
    Color activeBtn;

    private void Start()
    {
        changeColor = GameObject.Find("GO_ScriptChangeColor").GetComponent<changeColor>();
    }

    public void CatalogueChange (string catalogueName) 
    {
        CloseAllPanelsAndColor();
        if(catalogueName.Equals("shirtColorCatalogue"))
        {
            shirtColorCatalogue.SetActive(true);
            changeColor.SelectCloth(0);
            colorsCatalogue[0].SetActive(true);
        }
        else if(catalogueName.Equals("hairColorCatalogue"))
        {
            hairColorCatalogue.SetActive(true);        
            changeColor.SelectCloth(1);
            colorsCatalogue[1].SetActive(true);

        }
        else if(catalogueName.Equals("pantsColorCatalogue"))
        {
            pantsColorCatalogue.SetActive(true);        
            changeColor.SelectCloth(2);
            colorsCatalogue[2].SetActive(true);

        }
        else if(catalogueName.Equals("shoesColorCatalogue"))
        { 
            shoesColorChangeCatalogue.SetActive(true);        
            changeColor.SelectCloth(3);
            colorsCatalogue[3].SetActive(true);
        }
        else
        {
            Debug.Log("------   none catelogue    -----");
        }
    }

    public void colorChange(int j)
    {
        for (int i = 0; i < imageOfButtons.Length; i++)
        {
            imageOfButtons[i].color = Color.white;
        }
        imageOfButtons[j].color = Color.green;
    }


    void CloseAllPanelsAndColor()
    {
        for(int i = 0;i < colorsCatalogue.Length;i++)
        {
            colorsCatalogue[i].SetActive(false);
        }
    }


    public void OnRed()
    {
        changeColor.SetColor(1);
    }

    public void OnBlue()
    {
        changeColor.SetColor(0);
    }

    public void OnGreen()
    {
        changeColor.SetColor(2);
    }

    public void OnBlack()
    {
        changeColor.SetColor(3);
    }
    public void OnBrown()
    {
        changeColor.SetColor(4);
    }
    public void OnOff_White()
    {
        changeColor.SetColor(5);
    }
    public void OnNavy_Blue()
    {
        changeColor.SetColor(6);
    }
    public void OnOrange()
    {
        changeColor.SetColor(7);
    }

    public void OnLeopard()
    {
        changeColor.SetColor(8);
    }

}
