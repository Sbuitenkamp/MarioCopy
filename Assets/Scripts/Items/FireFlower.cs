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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnReveal()
    {
        gameObject.SetActive(true);
    }

    public void OnActivate() { } // not used in the FireFlower

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) {
            // swap sprite
            col.gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Actors/MarioFire", typeof(Sprite));
            col.gameObject.GetComponent<PlayerController>().Grow(SizeIndex);
            // the item gone now
            Destroy(gameObject);
        }
    }

}
