using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 1f;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = 0;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private int _health = 3;

    private SpawnManager _spawnManager;

    private bool _hasTripleShot = false;
    private bool _hasSpeed = false;

    [SerializeField]
    private int _score;

    [SerializeField]
    private GameObject _shield;

    private GameManager _gameManager;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject[] _fireEngine;

    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _playerAudio;

    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;

    float horizontalInput;
    float verticalInput;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _playerAudio = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
        if (_playerAudio == null)
        {
            Debug.LogError("Audio Source is NULL");
        }

        if (!_gameManager.IsCoopMode)
        {
            transform.position = Vector3.zero;
        }
    }

    void Update()
    {
        CalculateMovement();

        if (isPlayerOne)
        {
            if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
        else if (isPlayerTwo)
        {
            if (Input.GetKey(KeyCode.Return) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
    }

    void CalculateMovement()
    {
        if (isPlayerOne)
        {
            horizontalInput = Input.GetAxis("Horizontal1");
            verticalInput = Input.GetAxis("Vertical1");
        }
        else if (isPlayerTwo)
        {
            horizontalInput = Input.GetAxis("Horizontal2");
            verticalInput = Input.GetAxis("Vertical2");
        }

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_hasSpeed)
        {
            _speedMultiplier = 2;
        }
        else
        {
            _speedMultiplier = 1;
        }

        transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);

        float leftBound = -12;
        float rightBound = 12;
        float upBound = 0;
        float downBound = -4;

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, downBound, upBound),
            0
        );

        if (transform.position.x >= rightBound)
        {
            transform.position = new Vector3(-rightBound, transform.position.y, 0);
        }
        else if (transform.position.x <= leftBound)
        {
            transform.position = new Vector3(-leftBound, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_hasTripleShot)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(
                _laserPrefab,
                transform.position + new Vector3(0, 1.05f, 0),
                Quaternion.identity
            );
        }

        _playerAudio.PlayOneShot(_laserSound, 0.5f);
    }

    public void Damage()
    {
        if (_shield.activeSelf)
        {
            _shield.SetActive(false);
        }
        else
        {
            _health -= 1;

            if (_health == 2)
            {
                _fireEngine[0].SetActive(true);
            }
            else if (_health == 1)
            {
                _fireEngine[1].SetActive(true);
            }

            _uiManager.UpdateLives(_health);
        }

        if (_health <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    public void PickedTripleShot()
    {
        _hasTripleShot = true;

        StartCoroutine(turnOffTripleShot());
    }

    public void PickedSpeed()
    {
        _hasSpeed = true;

        StartCoroutine(turnOffSpeed());
    }

    public void PickedShield()
    {
        _shield.SetActive(true);
    }

    IEnumerator turnOffTripleShot()
    {
        yield return new WaitForSeconds(5);

        _hasTripleShot = false;
    }

    IEnumerator turnOffSpeed()
    {
        yield return new WaitForSeconds(5);

        _hasSpeed = false;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "EnemyLaser")
        {
            Destroy(collider.gameObject);
            Damage();
        }
    }
}
