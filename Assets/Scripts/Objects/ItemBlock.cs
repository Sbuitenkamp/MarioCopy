using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlock : MonoBehaviour
{
    private Mushroom Mushroom;
    // Start is called before the first frame update
    void Start()
    {
        Mushroom = gameObject.GetComponentInChildren<Mushroom>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) Mushroom.OnReveal();
    }
}
