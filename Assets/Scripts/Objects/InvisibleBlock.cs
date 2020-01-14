using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBlock : MonoBehaviour
{
    private SpriteRenderer Renderer;
    private OneUp OneUp;
    public bool Active { get; set; }

    void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.enabled = false;
        Active = true;
        OneUp = GetComponentInChildren<OneUp>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!Active) return;
        if (col.gameObject.CompareTag("Player")) {
            Renderer.enabled = true;
            OneUp.OnReveal();
            Active = false;
        }
    }
}
