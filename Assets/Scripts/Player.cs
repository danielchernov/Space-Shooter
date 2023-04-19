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

    private UIManager _uiManager;

    void Start()
    {
        transform.position = Vector3.zero;
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

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
}
