using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    private int Lives;
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

    private void Start()
    {
        Lives = 3;
        Debug.Log(Lives);
    }

    void Awake()
    {
        ThisInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        ThisInstance = null;
    }


    public void MinusLives()
    {
        // if lives is at 0 the player is at his last life, if they die then and this method gets invoked it's game over;
        if (Lives == 0) {
            Debug.Log("Game Over");
        }
        Lives--;
        Debug.Log(Lives);
    }

    public int GetLives()
    {
        return Lives;
    }
}
