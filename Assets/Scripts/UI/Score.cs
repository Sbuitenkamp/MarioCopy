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

    public Text ScoreText;

    private void Awake()
    {
        ThisInstance = this;
        ScoreText = GetComponentInParent<Text>();
        ScoreText.text = GameSystem.Instance.GameScore.ToString("000000");
    }
    public void AddScore(int amount)
    {
        GameSystem.Instance.GameScore += amount;
        ScoreText.text = GameSystem.Instance.GameScore.ToString("000000");
    }
    public int GetScore()
    {
        return GameSystem.Instance.GameScore;
    }
}