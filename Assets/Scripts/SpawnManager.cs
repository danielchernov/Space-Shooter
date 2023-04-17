using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerupPrefabs;

    [SerializeField]
    private Transform _enemyContainer;

    private bool _stopSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }

    IEnumerator SpawnEnemy()
    {
        while (!_stopSpawning)
        {
            yield return new WaitForSeconds(3);

            float randomX = Random.Range(-9f, 9f);
            Vector3 randomPos = new Vector3(randomX, 8, 0);

            Instantiate(_enemyPrefab, randomPos, Quaternion.identity, _enemyContainer);
        }
    }

    IEnumerator SpawnPowerup()
    {
        while (!_stopSpawning)
        {
            float randomWaitTime = Random.Range(5f, 10f);
            yield return new WaitForSeconds(randomWaitTime);

            float randomX = Random.Range(-9f, 9f);
            Vector3 randomPos = new Vector3(randomX, 8, 0);
            int randomPowerup = Random.Range(0, _powerupPrefabs.Length);

            if (_powerupPrefabs[randomPowerup] != null)
            {
                Instantiate(_powerupPrefabs[2], randomPos, Quaternion.identity);
            }
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
