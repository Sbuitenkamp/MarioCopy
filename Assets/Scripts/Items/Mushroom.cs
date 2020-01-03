using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mushroom : MonoBehaviour, Item
{
    public int SizeIndex { get; set; }
    public int ScoreWorth { get; set; }
    private bool Moving;
    private bool Direction; // true = right; false = left
    private Rigidbody2D RigidBody;
    public float Speed = 10.0f;
    private bool Grounded;
    public float MaxVelocity = 1.5f;

    void Start()
    {
        gameObject.SetActive(false);
        SizeIndex = 2;
        Moving = false;
        Direction = false;
        RigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Moving) {
            Vector2 velocity = RigidBody.velocity;
            velocity.x = Mathf.Clamp(velocity.x, -MaxVelocity, MaxVelocity);
            RigidBody.velocity = velocity;
            if (Grounded) RigidBody.AddForce(Direction ? Vector2.right : Vector2.left * Speed);
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
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) {
            // swap sprite
            col.gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Actors/MarioBig", typeof(Sprite));
            col.gameObject.GetComponent<PlayerController>().Grow(SizeIndex);
            // the item gone now
            Destroy(gameObject);
        } else GroundCheck(col);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        GroundCheck(col);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        GroundCheck(col);
        if (!Grounded) return;
        if (RigidBody.velocity.x == 0.0f) Direction = !Direction;
    }

    private void GroundCheck(Collision2D col)
    {
        List<string> groundTags = new List<string> { "Ground", "Block" };
        Grounded = groundTags.Where(tag => tag.Contains(col.gameObject.tag)) != null;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy")) Physics2D.IgnoreCollision(col, GetComponent<BoxCollider2D>());
    }
}