using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlower : MonoBehaviour, Item
{
    public int SizeIndex { get; set; }
    public int ScoreWorth { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        SizeIndex = 3;
        ScoreWorth = 1000;
    }

    public void OnReveal()
    {
        gameObject.SetActive(true);
    }

    public void OnActivate() { } // not used in the FireFlower

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) {
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
            SpriteRenderer spriteRenderer = col.gameObject.GetComponent<SpriteRenderer>();
            // swap sprite
            if (playerController.StarActive) {
                spriteRenderer.sprite = (Sprite)Resources.Load("Actors/MarioStarBig", typeof(Sprite));
                playerController.OldSize = SizeIndex;
            }
            else spriteRenderer.sprite = (Sprite)Resources.Load("Actors/MarioFire", typeof(Sprite));
            playerController.Grow(SizeIndex);
            Score.Instance.AddScore(ScoreWorth);
            // the item gone now
            Destroy(gameObject);
        }
    }

}
