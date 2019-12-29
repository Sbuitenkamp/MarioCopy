using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy")) {
            Destroy(col.gameObject.GetComponent<Collider2D>());
            Destroy(col.gameObject.GetComponentInChildren<BoxCollider2D>());
            col.gameObject.GetComponent<SpriteRenderer>().flipY = true;
            Destroy(col.gameObject, 5);
        }
    }
}
