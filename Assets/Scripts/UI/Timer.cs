using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text TextArea;
    public int TimeLeft;
    private float Elapsed;

    private void Start()
    {
        TimeLeft = 400;
        Elapsed = 1f;
    }

    private void Awake()
    {
        TextArea = GetComponentInParent<Text>();
        TextArea.text = TimeLeft.ToString();
    }

    private void Update() 
    {
        Elapsed += Time.deltaTime;
        if (Elapsed >= 1f) {
            Elapsed = 0;
            TimeLeft--;
            TextArea.text = TimeLeft.ToString();
            if (TimeLeft <= 0) {
                Debug.Log("time up");
            }
        }
    }
}
