using UnityEngine;

public class Mushroom : MonoBehaviour, Item
{
    public int SizeIndex { get; set; }
    public int ScoreWorth { get; set; }

    void Start()
    {
        SizeIndex = 2;
    }
    void Update() {}
    public void OnReveal() {}
    public void OnActivate() {}
    void OnCollisionEnter2D(Collision2D Col)
    {
        if (Col.gameObject.CompareTag("Player")) {
            // swap sprite
            Col.gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Actors/MarioBig", typeof(Sprite));
            Col.gameObject.GetComponent<PlayerController>().Grow(SizeIndex);
            // the item is gone now
            Destroy(gameObject);
        }
    }
}