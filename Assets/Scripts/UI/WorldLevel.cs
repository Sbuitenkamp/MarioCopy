using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldLevel : MonoBehaviour
{
    private int World;
    private int Level;
    private Text Display;
   
    private void Start()
    {
        World = GameSystem.Instance.World;
        Level = GameSystem.Instance.Level;
        Display = GetComponentInParent<Text>();
        Display.text = Level + "-" + World;
    }
}
