using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private CapsuleCollider2D PoleCollider1;
    private CircleCollider2D PoleCollider2;
    private BoxCollider2D Base;
    private GameObject Player;
    public bool Sliding;
    public float Speed;
    
    private void Start()
    {
        PoleCollider1 = GetComponent<CapsuleCollider2D>();
        PoleCollider2 = GetComponent<CircleCollider2D>();
        Base = GetComponentInChildren<BoxCollider2D>();
        Speed = 5;
    }
    private void FixedUpdate()
    {
        if (Sliding) {
            Player.GetComponent<Rigidbody2D>().velocity = Vector2.down * Speed;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.contacts[0].otherCollider.gameObject.name != "Base") {
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
            col.transform.position = new Vector2(col.contacts[0].point.x + col.collider.bounds.size.x / 2 + PoleCollider1.bounds.size.x, col.contacts[0].point.y);
            playerController.Flip(true);
            Player = col.gameObject;
            GameSystem.Instance.StopTime();
            StartCoroutine(wait());
        }
        
        IEnumerator wait() {
            yield return new WaitForSeconds(1);
            Sliding = true;
        }
    }
}
