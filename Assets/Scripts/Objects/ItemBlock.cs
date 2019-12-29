using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlock : MonoBehaviour, Block
{
    private Mushroom Mushroom;
    private FireFlower FireFlower;
    public bool Active { get; set; }
    void Awake()
    {
        Mushroom = gameObject.GetComponentInChildren<Mushroom>();
        FireFlower = gameObject.GetComponentInChildren<FireFlower>();
        Debug.Log(Mushroom);
        Debug.Log(FireFlower);
        Active = true;
    }

    public void OnActivate(Collision2D col)
    {
        PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
        if (playerController.Size == 1) Mushroom.OnReveal();
        else FireFlower.OnReveal();
        Active = false;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
//        if (!Active) return;
        if (col.gameObject.CompareTag("Player")) OnActivate(col);
    }
}
