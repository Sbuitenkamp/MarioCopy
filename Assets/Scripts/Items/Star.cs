using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Star : MonoBehaviour, Item
{
    public int SizeIndex { get; set; }
    public int ScoreWorth { get; set; }
    private bool Moving;
    public Rigidbody2D RigidBody;
//    public CapsuleCollider2D WallCheck;
    public bool Direction; // true = right; false = left;
    public float Speed = 10.0f;
    public float MaxVelocity = 5.0f;
    
    void Start()
    {
        gameObject.SetActive(false);
        SizeIndex = 50;
        Moving = false;
        Direction = true;
        RigidBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (Moving) {
            Vector2 velocity = RigidBody.velocity;
            velocity.x = Mathf.Clamp(velocity.x, -MaxVelocity, MaxVelocity);
            RigidBody.velocity = velocity;
            Vector2 direction = Direction ? Vector2.right : Vector2.left;
            RigidBody.AddForce(direction * Speed);
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        List<string> tags = new List<string> { "Enemy", "Item" };
        if (tags.Any(x => x.Contains(col.gameObject.tag))) {
            Physics2D.IgnoreCollision(col, GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(col, GetComponentInChildren<Collider2D>());
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) {
            // star power active
            col.gameObject.GetComponent<PlayerController>().Star(SizeIndex);
            // the item gone now
            Destroy(gameObject);
        }
    }
    public void OnReveal()
    {
        gameObject.SetActive(true);
        Moving = true;
    }
    public void OnActivate() { }
}
