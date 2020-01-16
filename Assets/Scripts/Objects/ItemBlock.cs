using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlock : MonoBehaviour, Block
{
    private Mushroom Mushroom;
    private FireFlower FireFlower;
    private SpriteRenderer SpriteRenderer;
    public bool Active { get; set; }
    void Awake()
    {
        Mushroom = GetComponentInChildren<Mushroom>();
        FireFlower = GetComponentInChildren<FireFlower>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Active = true;
    }

    public void OnActivate(Collider2D col)
    {
        PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
        if (playerController.StarActive) {
            if (playerController.OldSize == 1) Mushroom.OnReveal();
            else FireFlower.OnReveal();
        }
        else if (playerController.Size == 1) Mushroom.OnReveal();
        else FireFlower.OnReveal();
        SpriteRenderer.sprite = (Sprite) Resources.Load("Objects/BlockInactive", typeof(Sprite));
        Active = false;
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!Active) return;
        if (col.gameObject.CompareTag("Player")) OnActivate(col);
    }
}
