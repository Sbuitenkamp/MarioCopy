using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    private static GameSystem ThisInstance;
    public static GameSystem Instance
    {
        get {
            if (ThisInstance == null) {
                ThisInstance = FindObjectOfType<GameSystem>();
                if (ThisInstance == null) {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    ThisInstance = obj.AddComponent<GameSystem>();
                }
            }
            return ThisInstance;
        }
    }

    public int Lives { get; set; }
    public int GameScore { get; set; }
    public int Level { get; set; }
    public int World { get; set; }
    public int TimeLeft = 400;
    public bool Paused;
    public bool DoneCalculating;
    private float Elapsed;
    public bool Calculate;
    private float Speed = 0.03f;

    private void Start()
    {
        Lives = 3;
        GameScore = 0;
        Paused = false;
        DoneCalculating = false;
        Time.timeScale = 1.0f;
        Elapsed = Speed;
        Calculate = false;
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ThisInstance = this;
        Level = 1;
        World = 1;
        // TODO custom level and world managing
    }

    private void Update()
    {
        if (Calculate) {
            Elapsed += Time.deltaTime;
            if (Elapsed >= Speed) {
                Elapsed = 0;

                if (TimeLeft > 0) {
                    TimeLeft--;
                    GameScore += 20;
                    Timer.Instance.TextArea.text = TimeLeft.ToString("000");
                    Score.Instance.ScoreText.text = GameScore.ToString("000000");
                } else {
                    Calculate = false;
                    DoneCalculating = true;
                }
            }
        }
    }
    private void OnDestroy()
    {
        ThisInstance = null;
    }

    public void MinusLives()
    {
        // if lives is at 0 the player is at his last life, if they die then this method gets invoked and it's game over;
        if (Lives == 0) {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            Destroy(gameObject); // destroy so the game gets reset
        } else {
            SceneManager.LoadScene("OverView", LoadSceneMode.Single);
            Lives--;
        }
    }
    // this will simulate stopped time without setting timescale
    public void StopTime()
    {
        Paused = true;
        foreach (Rigidbody2D rigidbody in FindObjectsOfType<Rigidbody2D>()) {
            rigidbody.gravityScale = 0;
            rigidbody.velocity = new Vector2(0, 0);
        }
    }
    // use this for a future pause menu function
    public void Pause()
    {
        Time.timeScale = 0;
    }
}
