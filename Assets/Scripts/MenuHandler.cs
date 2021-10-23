using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuHandler : MonoBehaviour
{
    public Canvas ui, menu;
    public Button quitButton, resumeButton;
    private bool showingMenu = false;

    private void Start()
    {
        quitButton.onClick.AddListener(QuitGame);
        resumeButton.onClick.AddListener(ToggleMenu);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        ui.enabled = false;
        menu.enabled = true;
    }
    private void ResumeGame()
    {
        Time.timeScale = 1;
        ui.enabled = true;
        menu.enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
        
    }

    private void ToggleMenu()
    {
        if(!showingMenu)
        {
            PauseGame();
        }else
        {
            ResumeGame();
        }

        showingMenu = !showingMenu;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleMenu();
    }
}
