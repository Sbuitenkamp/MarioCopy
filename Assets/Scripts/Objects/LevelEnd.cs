using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    private Data JsonData;
    private const string Path = "./assets/scripts/data.json";

    void Start()
    {
        using (StreamReader r = new StreamReader(Path))
        {
            string json = r.ReadToEnd();
            JsonData = JsonUtility.FromJson<Data>(json);
        }   
    }
    private void Update()
    {
        if (GameSystem.Instance.DoneCalculating) {
            Debug.Log("Done");
            int score = Score.Instance.GetScore();
            if (JsonData.HighScore < score) JsonData.HighScore = score;

            using (var sw = new StreamWriter(Path)) {
                string jsonData = JsonUtility.ToJson(JsonData);
                sw.Write(jsonData);
            }

            StartCoroutine(nextScene());
        }
        IEnumerator nextScene() {
            yield return new WaitForSeconds(2);
            // make use of GameSystem.Instance.World and .Level to check the next level
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) GameSystem.Instance.Calculate = true;
    }
}
