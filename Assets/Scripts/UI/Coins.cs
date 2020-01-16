using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    private static Coins ThisInstance;
    public static Coins Instance
    {
        get {
            if (ThisInstance == null) {
                ThisInstance = FindObjectOfType<Coins>();
                if (ThisInstance == null) {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    ThisInstance = obj.AddComponent<Coins>();
                }
            }
            return ThisInstance;
        }
    }

    private Text CoinText;
    private int CoinAmount;

    private void Start()
    {
        CoinAmount = 0;
    }

    private void Awake()
    {
        ThisInstance = this;
        CoinText = GetComponentInParent<Text>();
        CoinText.text = CoinAmount.ToString("00");
    }

    public void AddCoin()
    {
        CoinAmount++;
        CoinText.text = CoinAmount.ToString("00");
    }
}
