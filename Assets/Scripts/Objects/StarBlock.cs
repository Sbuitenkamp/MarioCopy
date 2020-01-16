using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBlock : MonoBehaviour, Block
{
    private Star Star;
    private SpriteRenderer SpriteRenderer;
    public bool Active { get; set; }
    
    public void Awake()
    {
        Star = GetComponentInChildren<Star>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Active = true;
    }

    public void OnActivate(Collider2D col)
    {
        Star.OnReveal();
    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!Active) return;
        if (col.gameObject.CompareTag("Player")) {
            OnActivate(col);
            SpriteRenderer.sprite = (Sprite) Resources.Load("Objects/BlockInactive", typeof(Sprite));
            Active = false;
        }
    }
}
