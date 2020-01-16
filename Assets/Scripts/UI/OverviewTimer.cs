using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverviewTimer : MonoBehaviour
{
    private float TimeLeft;

    private void Start()
    {
        TimeLeft = 2;
    }

    private void Update() 
    {
        GameSystem gameSystem = GameSystem.Instance;
        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0) SceneManager.LoadScene(gameSystem.Level + "-" + gameSystem.World, LoadSceneMode.Single);
    }
}
