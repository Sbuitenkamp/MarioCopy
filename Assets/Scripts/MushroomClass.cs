using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MushroomClass :  MonoBehaviour, Item
{
    public int SizeIndex { get; set; }
    public int ScoreWorth { get; set; }
    public Rigidbody2D RigidBody;
    public CapsuleCollider2D WallCheck;
    public bool Moving;
    public bool Direction; // true = right; false = left;
    public float Speed = 10.0f;
    public bool Grounded;
    public float MaxVelocity = 2f;

    public void Start()
    {
        gameObject.SetActive(false);
        SizeIndex = 2;
        Moving = false;
        Direction = true;
        RigidBody = GetComponent<Rigidbody2D>();
        WallCheck = GetComponentInChildren<CapsuleCollider2D>();
    }
    
    void FixedUpdate()
    {
        if (Moving) {
            Vector2 velocity = RigidBody.velocity;
            velocity.x = Mathf.Clamp(velocity.x, -MaxVelocity, MaxVelocity);
            RigidBody.velocity = velocity;
            Vector2 direction = Direction ? Vector2.right : Vector2.left;
            if (Grounded) RigidBody.AddForce(direction * Speed);
        }
    }

    public void OnReveal()
    {
        gameObject.SetActive(true);
        OnActivate();
    }

    public void OnActivate()
    {
        Moving = true;
        Grounded = true;
    }
    
    public void OnCollisionExit2D(Collision2D col)
    {
        GroundCheck(col);
    }

    public void OnCollisionStay2D(Collision2D col)
    {
        GroundCheck(col);
    }

    protected void GroundCheck(Collision2D col)
    {
        List<string> groundTags = new List<string> { "Ground", "Block" };
        Grounded = groundTags.Any(x => x.Contains(col.gameObject.tag));
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        List<string> tags = new List<string> { "Enemy", "Item" };
        if (tags.Any(x => x.Contains(col.gameObject.tag))) {
            Physics2D.IgnoreCollision(col, GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(col, GetComponentInChildren<Collider2D>());
        }
    }
}