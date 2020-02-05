using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBase : MonoBehaviour
{
    private Flag Flag;

    private void Awake()
    {
        Flag = GetComponentInParent<Flag>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && Flag.Sliding) {
            Flag.Sliding = false;
            Vector2 pos = new Vector2(col.contacts[0].point.x + col.collider.bounds.size.x / 2, col.contacts[0].point.y);
            col.gameObject.GetComponent<PlayerController>().EndGame(pos);
        }
    }
}
