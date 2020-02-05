using System.IO;
//using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class TopScore : MonoBehaviour
{
    private Text HighScore;
    private const string Path = "./assets/scripts/data.json";
    private void Start()
    {
        HighScore = GetComponentInParent<Text>();
        using (StreamReader r = new StreamReader(Path))
        { 
            string json = r.ReadToEnd();
            Data JsonData = JsonUtility.FromJson<Data>(json);
            HighScore.text = JsonData.HighScore.ToString();
        }
    }
}
