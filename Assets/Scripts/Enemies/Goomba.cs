using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goomba : MonoBehaviour, Enemy
{
    private bool alive;
    public bool Alive
    {
        get => alive;
        set {
            alive = value;
            if (!value) Score.Instance.AddScore(ScoreWorth);
        }
    }

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
        if (GameSystem.Instance.Paused) return;
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
        foreach (Transform child in transform) Destroy(child.gameObject);
        Destroy(gameObject, 1.5f);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!Alive) return;
        if (col.gameObject.CompareTag("Player")) {
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
            if (playerController.StarActive) {
                Alive = false;
                Physics2D.IgnoreCollision(col.collider, Collider);
                OnFireBall();
            } else if (col.contacts[0].otherCollider.gameObject.name == "Head") { // if on top of head
                Alive = false;
                Physics2D.IgnoreCollision(col.collider, Collider);
                OnJump(col);
            } else if (Alive && !playerController.Invincible) playerController.Shrink(); // check if alive and then reduce mario's size
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
