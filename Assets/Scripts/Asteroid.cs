using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 1f;

    [SerializeField]
    private GameObject _explosion;

    [SerializeField]
    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1f * _rotationSpeed * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Laser")
        {
            GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(explosion, 3f);
            Destroy(collider.gameObject);

            _spawnManager.StartSpawning();

            Destroy(gameObject, 0.25f);
        }
    }
}
