using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public bool Direction { private get; set; } // false = right; true = left;
    private Rigidbody2D RigidBody;
    private float MaxVelocity = 10.0f;
    private float Speed = 15.0f;

    private void Start()
    {
        Destroy(gameObject, 4);
        RigidBody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Vector2 velocity = RigidBody.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -MaxVelocity, MaxVelocity);
        RigidBody.velocity = velocity;
        if (Direction) RigidBody.AddForce(Vector2.left * Speed);
        else RigidBody.AddForce(Vector2.right * Speed);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy")) {
            col.gameObject.GetComponent<Goomba>().OnFireBall();
            Destroy(gameObject);
        } else if (Math.Abs(col.contacts[0].normal.x) > 0.9f) {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        GetComponentInParent<PlayerController>().FireBalls--;
    }
}