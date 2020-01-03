using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private BoxCollider2D BoxCollider;
    private CapsuleCollider2D CapsuleCollider;
    
    void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Ground")) return;
        Physics2D.IgnoreCollision(col.collider, BoxCollider);
        Physics2D.IgnoreCollision(col.collider, CapsuleCollider);
    }
}
