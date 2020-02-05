using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text TextArea;
    private float Elapsed;
    
    private static Timer ThisInstance;
    public static Timer Instance
    {
        get {
            if (ThisInstance == null) {
                ThisInstance = FindObjectOfType<Timer>();
                if (ThisInstance == null) {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    ThisInstance = obj.AddComponent<Timer>();
                }
            }
            return ThisInstance;
        }
    }

    private void Start()
    {
        Elapsed = 1f;
    }

    private void Awake()
    {
        TextArea = GetComponentInParent<Text>();
        TextArea.text = GameSystem.Instance.TimeLeft.ToString();
    }

    private void Update() 
    {
        if (GameSystem.Instance.Paused) return;
        Elapsed += Time.deltaTime;
        if (Elapsed >= 1f) {
            Elapsed = 0;
            GameSystem.Instance.TimeLeft--;
            TextArea.text = GameSystem.Instance.TimeLeft.ToString();
            if (GameSystem.Instance.TimeLeft <= 0) {
                Debug.Log("time up");
            }
        }
    }
}
