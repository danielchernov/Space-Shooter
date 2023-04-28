using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

    public bool IsCoopMode = false;

    private int currentScene;

    public GameObject pauseMenu;

    public Animator pauseMenuAnimator;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        if (pauseMenu != null)
        {
            pauseMenuAnimator = pauseMenu.GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(currentScene);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentScene != 0)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else if (currentScene == 0)
            {
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.P) && !_isGameOver && currentScene != 0)
        {
            Time.timeScale = 0;
            pauseMenuAnimator.SetBool("isPaused", true);
            //pauseMenu.SetActive(true);
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
