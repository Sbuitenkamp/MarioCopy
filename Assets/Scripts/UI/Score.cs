using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private static Score ThisInstance;
    public static Score Instance
    {
        get {
            if (ThisInstance == null) {
                ThisInstance = FindObjectOfType<Score>();
                if (ThisInstance == null) {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    ThisInstance = obj.AddComponent<Score>();
                }
            }
            return ThisInstance;
        }
    }

    private Text ScoreText;
    private int ScoreAmount;

    private void Start()
    {
        ScoreAmount = 0;
    }
    private void Awake()
    {
        ThisInstance = this;
        ScoreText = GetComponentInParent<Text>();
        ScoreText.text = ScoreAmount.ToString("000000");
    }
    public void AddScore(int amount)
    {
        ScoreAmount += amount;
        ScoreText.text = ScoreAmount.ToString("000000");
    }
}