using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    // movement stuff
    private Rigidbody2D RigidBody;
    private bool Grounded;
    private bool PerformJump;
    private bool MoveRight;
    private bool MoveLeft;
    private bool Flipped;
    public float Speed = 25.0f;

    // feet ( ͡° ͜ʖ ͡°)
    private BoxCollider2D Feet;
    private float FeetSize;
    public float JumpHeight = 20.0f;
    
    // game stuff
    public int Size { get; set; }
    private SpriteRenderer SpriteRenderer;
    private CapsuleCollider2D PlayerCollider;
    
    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
        Feet = GetComponentInChildren<BoxCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Grounded = true;
        PerformJump = false;
        MoveRight = false;
        MoveLeft = false;
        Size = 1;
        Flipped = false;
        FeetSize = Feet.size.y;
    }

    void FixedUpdate()
    {
        // prevent double jump and limit jumpheight
        if (PerformJump) {
            if (Grounded) jump();
            PerformJump = false;
            Grounded = false;
        }

        if (MoveRight || MoveLeft) move(MoveRight);

        void jump() {
            RigidBody.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
        }

        void move(bool direction) {
            if (direction) {
                // right
                RigidBody.AddForce(Vector2.right * Speed);
                Flip(false);
            } else {
                // left
                Flip(true);
                RigidBody.AddForce(Vector2.left * Speed);
            }
        }
    }

    void Update()
    {
        // movement
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown("up")) PerformJump = true;
        
        // prevent double jump
        if (Input.GetButtonUp("Jump") || Input.GetKeyUp("up") && Grounded) Grounded = false;

        if (Input.GetKey("right")) MoveRight = true;
        if (Input.GetKeyUp("right")) MoveRight = false;
        if (Input.GetKey("left")) MoveLeft = true;
        if (Input.GetKeyUp("left")) MoveLeft = false;
        
        // prevent sliding
        if (Input.GetKeyUp("right") || Input.GetKeyUp("left")) {
            Vector2 velocity = RigidBody.velocity;
            velocity.x = 0;
            RigidBody.velocity = velocity;
        }
    }
    
    void OnCollisionStay2D(Collision2D Col)
    {
        // ground check; using multiple tags because certain block will get certain tags, but all will be walkable
        List<string> groundTags = new List<string> { "Ground", "Block" };
        if (groundTags.Where(tag => tag.Contains(Col.gameObject.tag)) != null && Col.contacts[0].otherCollider.transform.gameObject.name == "Feet") Grounded = true;
    }

    public void Shrink()
    {
        if (Size == 3) gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite) Resources.Load("Actors/MarioBig", typeof(Sprite));
        else if (Size == 2) {
            gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite) Resources.Load("Actors/MarioSmall", typeof(Sprite));
            Vector2 s = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
            s.y -= FeetSize;
            PlayerCollider.size = s;
            PlayerCollider.offset = new Vector2 (0, FeetSize);
            Feet.offset = new Vector2 (0, -((s.y + FeetSize) / 2));
        }
        Size--;
    }
    
    // resize collider to fit the new sprite
    public void Grow(int SizeIndex)
    {
        Vector2 s = SpriteRenderer.sprite.bounds.size;
        s.y -= FeetSize;
        PlayerCollider.size = s;
        PlayerCollider.offset = new Vector2 (0, FeetSize);
        Size = SizeIndex;
        Feet.offset = new Vector2 (0, -((s.y + FeetSize) / 2));
    }

    // flip sprite
    private void Flip(bool expected)
    {
        if (Flipped != expected) {
            Flipped = !Flipped;
            SpriteRenderer.flipX = Flipped;
        }
    }
}