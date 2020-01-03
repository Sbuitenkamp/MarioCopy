using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goomba : MonoBehaviour, Enemy
{
    public bool Alive { get; set; }
    private CapsuleCollider2D Collider;
    private bool Direction; // true = right; false = left
    private Rigidbody2D RigidBody;
    public float Speed = 5.0f;
    private bool Grounded;
    public float MaxVelocity = 1.5f;

    void Start()
    {
        Alive = true;
        Direction = false;
        Collider = GetComponent<CapsuleCollider2D>();
        RigidBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (Alive) {
            Vector2 velocity = RigidBody.velocity;
            velocity.x = Mathf.Clamp(velocity.x, -MaxVelocity, MaxVelocity);
            RigidBody.velocity = velocity;
            if (Grounded) RigidBody.AddForce(Direction ? Vector2.right : Vector2.left * Speed);
        }
    }
    public void OnJump(Collision2D col)
    {
        Alive = false;
        GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Actors/GoombaDead", typeof(Sprite));
        
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
            // if on top of head
            if (col.contacts[0].otherCollider.gameObject.name == "Head") OnJump(col);
            // check if alive and then reduce mario's size
            else if (Alive) col.gameObject.GetComponent<PlayerController>().Shrink();
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
}
