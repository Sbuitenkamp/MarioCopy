﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goomba : MonoBehaviour, Enemy
{
    public bool Alive { get; set; }
    public int ScoreWorth { get; set; }
    private CapsuleCollider2D Collider;
    private bool Direction; // true = right; false = left
    private Rigidbody2D RigidBody;
    public float Speed = 5.0f;
    private bool Grounded;
    public float MaxVelocity = 1.5f;
    private SpriteRenderer SpriteRenderer;

    void Start()
    {
        Alive = true;
        ScoreWorth = 200;
        Direction = false;
        Collider = GetComponent<CapsuleCollider2D>();
        RigidBody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        if (Alive) {
            Vector2 velocity = RigidBody.velocity;
            velocity.x = Mathf.Clamp(velocity.x, -MaxVelocity, MaxVelocity);
            RigidBody.velocity = velocity;
            Vector2 direction = Direction ? Vector2.right : Vector2.left;
            if (Grounded) RigidBody.AddForce(direction * Speed);
        }
    }

    public void OnFireBall()
    {
        Alive = false;
        Destroy(GetComponent<Collider2D>());
        foreach (Transform child in transform) Destroy(child.gameObject);
        SpriteRenderer.flipY = true;
        Destroy(gameObject, 5);
    }
    public void OnJump(Collision2D col)
    {
        SpriteRenderer.sprite = (Sprite)Resources.Load("Actors/GoombaDead", typeof(Sprite));
        
        // resize collider to the sprite
        Vector2 s = GetComponent<SpriteRenderer>().sprite.bounds.size;
        Collider.size = s;
        Collider.offset = Vector2.zero;
        
        // thrust player upwards
        col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 250));
        
        // and then remove it to prevent the player from interacting with it
        Destroy(GetComponent<Rigidbody2D>(), 0.27f);
        Destroy(Collider);
        Destroy(GetComponentInChildren<BoxCollider2D>());
        Destroy(gameObject, 1.5f);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) {
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
            if (playerController.StarActive) OnFireBall();
            // if on top of head
            else if (col.contacts[0].otherCollider.gameObject.name == "Head") {
                OnJump(col);
                Alive = false;
            // check if alive and then reduce mario's size
            } else if (Alive) playerController.Shrink();
        } else if (col.contacts[0].otherCollider.gameObject.name == "Wallcheck") Direction = !Direction;
        else GroundCheck(col);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        GroundCheck(col);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        GroundCheck(col); 
    }

    private void GroundCheck(Collision2D col)
    {
        List<string> groundTags = new List<string> { "Ground", "Block" };
        Grounded = groundTags.Where(tag => tag.Contains(col.gameObject.tag)) != null;
    }
}
