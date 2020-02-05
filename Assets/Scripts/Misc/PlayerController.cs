using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour 
{
    // movement stuff
    private Rigidbody2D RigidBody;
    private bool Grounded;
    private bool PerformJump;
    private bool MoveRight;
    private bool MoveLeft;
    public bool Flipped;
    public float Speed = 20.0f;
    public float MaxVelocity = 13;
    private bool JumpRelease;

    // feet ( ͡° ͜ʖ ͡°)
    private BoxCollider2D Feet;
    private float FeetSize;
    public float JumpHeight = 10.0f;
    
    // game stuff
    public int Size { get; private set; }
    private SpriteRenderer SpriteRenderer;
    private CapsuleCollider2D PlayerCollider;
    private bool FireBall;
    public int FireBalls;
    public bool StarActive;
    public int OldSize;
    private float TimeLeft;
    public bool Invincible;
    private float InvincibleCounter;
    private bool EndGameWalk;
    private bool HasFired;

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
        Invincible = false;
        InvincibleCounter = 0.0f;
        JumpRelease = false;
        EndGameWalk = false;
        HasFired = false;
    }
    private void FixedUpdate()
    {
        if (EndGameWalk) {
            Flip(false);
            MoveLeft = false;
            MoveRight = false;
            if (Grounded) RigidBody.velocity = Vector2.right * 3.0f;
        } 
        if (GameSystem.Instance.Paused) return;
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
            JumpRelease = true;
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
            StartCoroutine(fire());
        }
        IEnumerator fire() {
            HasFired = true;
            yield return new WaitForSeconds(0.5f);
            HasFired = false;
        }
        // end the star effect
        void endStar() {
            Grow(OldSize);
            string path;
            
            if (OldSize <= 1) path = "Actors/MarioSmall"; 
            else if (OldSize == 3) path = "Actors/MarioFire";
            else path = "Actors/MarioBig";
            
            SpriteRenderer.sprite = (Sprite)Resources.Load(path, typeof(Sprite));
            StarActive = false;
        }
    }
    private void Update()
    {
        if (GameSystem.Instance.Paused) return;
        InvincibleCounter -= Time.deltaTime;
        if (InvincibleCounter <= 0) Invincible = false;
        
        // movement
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown("up")) {
            PerformJump = true;
        }
        // prevent double jump
        if (Input.GetButtonUp("Jump") || Input.GetKeyUp("up")) {
            PerformJump = false;
            Grounded = false;
            if (JumpRelease) {
                float maxVelocity = 10;
                RigidBody.velocity = Vector2.ClampMagnitude(RigidBody.velocity, maxVelocity);
                JumpRelease = false;
            }
        }

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
            if (Size == 3 && !FireBall && !HasFired) FireBall = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("GameStopper")) EndGameWalk = false;
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
        Invincible = true;
        InvincibleCounter = 2.0f;
        if (Size == 0) GameSystem.Instance.MinusLives();
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
    public void EndGame(Vector2 pos)
    {
        RigidBody.gravityScale = 2.5f;
        StartCoroutine(start());

        IEnumerator start() {
            yield return new WaitForSeconds(1.3f);
            transform.position = pos;
            EndGameWalk = true;
        }
    }
    // flip sprite
    public void Flip(bool expected)
    {
        if (Flipped != expected) {
            Flipped = !Flipped;
            SpriteRenderer.flipX = Flipped;
        }
    }
}