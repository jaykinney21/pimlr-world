using UnityEngine;
using System;
using UnityEngine.UI;

public class CoinManager : Singleton<CoinManager>
{
    int coinValue = 0;
    public Text coinText;
    int Coins
    {
        get { return coinValue; }
        set
        {
            coinValue = value;
            sendDataToServer();

            if (coinValueChanged != null)
            {               
                coinValueChanged.Invoke(coinValue);
            }
            if (coinText != null)
            {
                coinText.text = coinValue.ToString();
            }

            if (SceneManagerScript.Instance && SceneManagerScript.Instance._coins != null)
            {
                SceneManagerScript.Instance._coins.text = value.ToString();
            }
        }
    }


    //internal delegate void CoinDelegate(int value);
    //internal CoinDelegate coinValueChanged;


    internal Action<int> coinValueChanged;


    public override void Awake()
    {

        base.Awake();

        if (AuthManager.Instance)
        {
            StartCoroutine(AuthManager.Instance.GetUserData(Constant.coinKey, (value) =>
            {
                if (value != null)
                {
                    //Debug.Log(value);
                    coinValue = int.Parse(value);
                    if(coinText!=null)
                    {
                        coinText.text = coinValue.ToString();
                    }
                    if (coinValueChanged != null)
                    {
                        coinValueChanged.Invoke(coinValue);
                    }
                }
            }));
        }
    }


   
    void sendDataToServer()
    {
        StartCoroutine(AuthManager.Instance.SetUserData(Constant.coinKey, coinValue.ToString(), (value) =>
         {
             //Debug.Log("Set::" + value);
         }));
    }


    

    //private int Coins
    //{
    //    get { return _coins; }
    //    set { _coins = value; }
    //}


    public void SetCoins(int newcoins)
    {
        if (newcoins < 0)
        {
            Coins = 0;
        }
        else
            Coins = newcoins;
    }

    public int GetCoins()
    {
        return Coins;
    }
}
