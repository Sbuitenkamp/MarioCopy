using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    private int Amount;
    private Text Display;

    private void Start()
    {
        Amount = GameSystem.Instance.Lives;
        Display = GetComponentInParent<Text>();
        Debug.Log(Amount);
        Display.text = Amount.ToString();
    }
}