using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col);
        if (col.gameObject.CompareTag("Player")) {
            if (col.gameObject.GetComponent<PlayerController>().Size >= 2) Destroy(gameObject, 0.3f);
        }
    }
}
