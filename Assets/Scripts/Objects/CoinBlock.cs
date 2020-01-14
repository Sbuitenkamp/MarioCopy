using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBlock : MonoBehaviour
{
    private Coin Coin;
    private bool Active;
    
    public void Awake()
    {
        Coin = GetComponentInChildren<Coin>();
        Active = true;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!Active) return;
        if (col.gameObject.CompareTag("Player")) {
            Coin.OnReveal();
            Active = false;
        }
    }
}
