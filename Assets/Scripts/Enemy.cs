using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 5f;

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
            Destroy(gameObject);
        }
        else if (collider.tag == "Laser")
        {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
}
