using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void SinglePlayerGame()
    {
        SceneManager.LoadScene("SinglePlayer");
    }

    public void CoopGame()
    {
        SceneManager.LoadScene("MultiPlayer");
    }
}
