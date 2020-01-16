using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBlock : MonoBehaviour
{
    private Coin Coin;
    private SpriteRenderer SpriteRenderer;
    private bool Active;
    
    public void Awake()
    {
        Coin = GetComponentInChildren<Coin>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Active = true;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!Active) return;
        if (col.gameObject.CompareTag("Player")) {
            Coin.OnReveal();
            SpriteRenderer.sprite = (Sprite) Resources.Load("Objects/BlockInactive", typeof(Sprite));
            Active = false;
        }
    }
}
