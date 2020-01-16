using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, Item
{
    public int ScoreWorth { get; set; }
    public int SizeIndex { get; set; }

    void Start()
    {
        gameObject.SetActive(false);
        ScoreWorth = 200;
    }
    public void OnReveal()
    {
        gameObject.SetActive(true);
        Destroy(gameObject, 1f);
        Coins.Instance.AddCoin();
        Score.Instance.AddScore(ScoreWorth);
    }
    public void OnActivate() {} // not used
}
