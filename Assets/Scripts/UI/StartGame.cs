using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button StartButton;

    private void Start()
    {
        StartButton = GetComponentInParent<Button>();
        Button btn = StartButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        
        void TaskOnClick() {
            SceneManager.LoadScene("Overview", LoadSceneMode.Single);
        }
    }
}
