using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class FinishedGame : MonoBehaviour
{
    public Canvas finishedGameCanvas;
    public Canvas UICanvas;
    public Button restartButton, quitButton;
    public TextMeshProUGUI finalScoreText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UICanvas.enabled = false;
        Cursor.visible = true;
        finishedGameCanvas.enabled = true;
        finalScoreText.SetText("Total score: " + PointSystem.Instance.GetPoints());
    }

    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("Start");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
