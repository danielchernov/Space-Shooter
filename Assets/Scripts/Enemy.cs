using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 5f;

    private Player _player;

    private Animator _enemyAnimator;

    [SerializeField]
    private AudioClip _explosionSound;

    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _enemyAudio;

    [SerializeField]
    private GameObject _laserPrefab;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        _enemyAnimator = GetComponent<Animator>();
        _enemyAudio = GetComponent<AudioSource>();

        StartCoroutine(FireLaser());
    }

    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -7)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = (new Vector3(randomX, 8, transform.position.z));
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Player player = collider.GetComponent<Player>();
            if (player != null)
                player.Damage();

            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;

            _enemyAudio.PlayOneShot(_explosionSound, 1f);

            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
        }
        else if (collider.tag == "Laser")
        {
            Destroy(collider.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;

            _enemyAudio.PlayOneShot(_explosionSound, 1f);

            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
        }
    }

    IEnumerator FireLaser()
    {
        float randomTime = Random.Range(2f, 5f);
        yield return new WaitForSeconds(randomTime);

        while (_enemySpeed != 0)
        {
            Instantiate(
                _laserPrefab,
                transform.position + new Vector3(0, -1.7f, 0),
                Quaternion.Euler(0, 0, 180)
            );

            _enemyAudio.PlayOneShot(_laserSound, 0.5f);

            randomTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(randomTime);
        }
    }
}
