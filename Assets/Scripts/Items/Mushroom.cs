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
    private Rigidbody2D Rigidbody;
    public float Speed = 10.0f;
    private bool Grounded;

    void Start()
    {
        gameObject.SetActive(false);
        SizeIndex = 2;
        Moving = false;
        Direction = true;
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Moving) {
            if (Grounded) {
                if (Direction) Rigidbody.AddForce(Vector2.right * Speed);
                else if (!Direction) Rigidbody.AddForce(Vector2.left * Speed);
            }
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

    private void GroundCheck(Collision2D col)
    {
        List<string> groundTags = new List<string> { "Ground", "Block" };
        if (groundTags.Where(tag => tag.Contains(col.gameObject.tag)) != null) Grounded = true;
    }
}