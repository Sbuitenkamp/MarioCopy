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
    public int Score { get; set; }
    public int Level { get; set; }
    public int World { get; set; }

    private void Start()
    {
        Lives = 3;
        Score = 0;
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ThisInstance = this;
        Level = 1;
        World = 1;
        // TODO custom level and world managing
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
}
