using CarControllerwithShooting;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Canvas_Handler : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI car_SpeedTXT;
    [SerializeField]
    CarController car_Controller;

    [SerializeField]
    GameObject winPanel, mainPanel;

    [SerializeField]
    TextMeshProUGUI winnerNameTxt;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        car_SpeedTXT.text = Convert.ToInt32(car_Controller.speed).ToString();
    }


    public void RaceEnded(string tag)
    {
        winPanel.SetActive(true);
        mainPanel.SetActive(false);
        if (tag == "ignoreSensor")
        {
            winnerNameTxt.text = "AI WINS";
        }
        else
        {

            winnerNameTxt.text = "PLAYER WINS";
        }
        Time.timeScale = 1;
    }
}
