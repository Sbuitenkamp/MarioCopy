using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
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
    public float MaxVelocity = 13;

    // feet ( ͡° ͜ʖ ͡°)
    private BoxCollider2D Feet;
    private float FeetSize;
    public float JumpHeight = 20.0f;
    
    // game stuff
    public int Size { get; private set; }
    private SpriteRenderer SpriteRenderer;
    private CapsuleCollider2D PlayerCollider;
    private bool FireBall;
    public int FireBalls;
    public bool StarActive;
    public int OldSize;
    private float TimeLeft;

    private void Start()
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
        Flipped = false; // false right; true left
        FeetSize = Feet.size.y;
        FireBall = false;
        StarActive = false;
        TimeLeft = 15f;
        FireBalls = 0;
    }

    private void Awake()
    {
        Debug.Log(GameSystem.Instance);
    }

    private void FixedUpdate()
    {
        Vector2 velocity = RigidBody.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -MaxVelocity, MaxVelocity);
        RigidBody.velocity = velocity;
        
        // prevent double jump and limit jumpheight
        if (MoveRight || MoveLeft) move(MoveRight);
        if (PerformJump) {
            if (Grounded) jump();
            PerformJump = false;
            Grounded = false;
        }

        if (StarActive) {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft < 0) {
                endStar();
                TimeLeft = 15f;
            }
        } else if (FireBall && FireBalls < 2) {
            shoot();
            FireBall = false;
        }

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
        void shoot() {
            Debug.Log(FireBalls);
            GameObject fireBall = new GameObject();
            Rigidbody2D fireBody = fireBall.AddComponent<Rigidbody2D>();
            FireBall script = fireBall.AddComponent<FireBall>();
            CircleCollider2D collider = fireBall.AddComponent<CircleCollider2D>();
            SpriteRenderer spriteRenderer = fireBall.AddComponent<SpriteRenderer>();
            Bounds bounds = PlayerCollider.bounds;
            
            fireBall.transform.SetParent(gameObject.transform);
            Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(collider, GetComponentInChildren<Collider2D>());
            fireBody.sharedMaterial = (PhysicsMaterial2D) Resources.Load("Materials/Bouncy", typeof(PhysicsMaterial2D));
            fireBody.gravityScale = 4.0f;
            script.Direction = Flipped;
            spriteRenderer.sprite = (Sprite) Resources.Load("Objects/FireBall", typeof(Sprite));

            // fire direction
            fireBall.transform.position = Flipped ? new Vector2(gameObject.transform.position.x - bounds.extents.x - 0.3f, bounds.center.y) : new Vector2(gameObject.transform.position.x + bounds.extents.x + 0.3f, bounds.center.y);
            FireBalls++;
        }
        // end the star effect
        void endStar() {
            Grow(OldSize);
            string path;
            if (OldSize <= 1) path = "Actors/MarioSmall"; 
            else if (OldSize == 3) path = "Actors/MarioFire";
            else path = "Actors/MarioBig";
            
            Debug.Log(path);
            SpriteRenderer.sprite = (Sprite)Resources.Load(path, typeof(Sprite));
            StarActive = false;
        }
    }
    private void Update()
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

        // fireball
        if (Input.GetButtonDown("Fire1")) {
            // for future updates adding items like ice flower, just put an else if with the corresponding size
            if (Size == 3 && !FireBall) FireBall = true;
        }
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        // ground check; using multiple tags because certain block will get certain tags, but all will be walkable
        List<string> groundTags = new List<string> { "Ground", "Block" };
        if (groundTags.Any(tag => tag.Contains(col.gameObject.tag)) && col.contacts[0].otherCollider.transform.gameObject.name == "Feet") Grounded = true;
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        Grounded = false;
    }
    
    public void Shrink()
    {
        if (Size == 3) SpriteRenderer.sprite = (Sprite) Resources.Load("Actors/MarioBig", typeof(Sprite));
        else if (Size == 2) {
            SpriteRenderer.sprite = (Sprite) Resources.Load("Actors/MarioSmall", typeof(Sprite));
            Grow(Size);
        }
        Size--;
        if (Size <= 0) GameSystem.Instance.MinusLives();
    }
    // resize collider to fit the new sprite
    public void Grow(int sizeIndex)
    {
        Vector2 s = SpriteRenderer.sprite.bounds.size;
        s.y -= FeetSize;
        PlayerCollider.size = s;
        PlayerCollider.offset = new Vector2(0, FeetSize);
        Size = sizeIndex;
        Feet.offset = new Vector2(0, -((s.y + FeetSize) / 2));
    }
    // star
    public void Star(int sizeIndex)
    {
        if (StarActive) return;
        StarActive = true;
        OldSize = Size;
        string path = OldSize <= 1 ? "Actors/MarioStarSmall" : "Actors/MarioStarBig";

        SpriteRenderer.sprite = (Sprite)Resources.Load(path, typeof(Sprite));
        Grow(sizeIndex);
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