using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _bestScoreText;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Image _liveImg;

    [SerializeField]
    private GameObject _gameOver;

    private GameManager _gameManager;

    private int _score = 0;
    private int _bestScore = 0;

    void Start()
    {
        _bestScore = PlayerPrefs.GetInt("BestScore", 0);
        _scoreText.text = "Score: " + 0;

        if (_bestScoreText != null)
        {
            _bestScoreText.text = "Best: " + _bestScore;
        }

        _gameOver.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    public void UpdateScore(int score)
    {
        _score = score;
        _scoreText.text = "Score: " + score;
    }

    public void CheckForBestScore()
    {
        if (_bestScoreText != null)
        {
            if (_score > _bestScore)
            {
                _bestScore = _score;
                _bestScoreText.text = "Best: " + _bestScore;
                PlayerPrefs.SetInt("BestScore", _bestScore);
            }
        }
    }

    public void UpdateLives(int lives)
    {
        _liveImg.sprite = _liveSprites[lives];

        if (lives == 0)
        {
            _gameOver.SetActive(true);
            _gameManager.GameOver();
            CheckForBestScore();
        }
    }

    public void ResumePlay()
    {
        Time.timeScale = 1;
        _gameManager.pauseMenuAnimator.SetBool("isPaused", false);
        ;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
