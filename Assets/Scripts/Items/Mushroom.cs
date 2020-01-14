using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mushroom : MushroomClass
{
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
            else spriteRenderer.sprite = (Sprite)Resources.Load("Actors/MarioBig", typeof(Sprite));
            playerController.Grow(SizeIndex);
            // the item gone now
            Destroy(gameObject);
        } else if (col.contacts[0].otherCollider.gameObject.name == "Wallcheck") Direction = !Direction;
        else GroundCheck(col);
    }
}