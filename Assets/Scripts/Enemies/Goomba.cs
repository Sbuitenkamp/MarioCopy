using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour, Enemy
{
    public bool Alive { get; set; }
    private CapsuleCollider2D Collider;

    void Start()
    {
        Alive = true;
        Collider = GetComponent<CapsuleCollider2D>();
    }
    void Update() { }
    public void Walk() { }
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
        }
    }
}
