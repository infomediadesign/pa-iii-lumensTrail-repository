using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject audioLanguageScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject quitScreen;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            if (!isPaused) 
            {
                Pause();
            }
            else 
            {
                Resume();
            }
        }
    }

    private void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

    }

    public void Resume()
    {
        isPaused = false;
        audioLanguageScreen.SetActive(false);
        controlsScreen.SetActive(false);
        quitScreen.SetActive(false);
        pauseScreen.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
