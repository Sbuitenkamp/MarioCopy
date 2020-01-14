using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUp : MushroomClass
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) {
            // todo lives++;
            // the item gone now
            Destroy(gameObject);
        } else if (col.contacts[0].otherCollider.gameObject.name == "Wallcheck") Direction = !Direction;
        else GroundCheck(col);
    }
}
