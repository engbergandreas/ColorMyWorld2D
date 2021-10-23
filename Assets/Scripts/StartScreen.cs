using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public Button startButton, quitButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartPressed);
        quitButton.onClick.AddListener(OnQuitPressed);
    }

    void OnQuitPressed()
    {
        Application.Quit();
    }

    void OnStartPressed()
    {
        SceneManager.LoadScene("Level 1");
    }
}
