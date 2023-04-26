using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 5f;

    [SerializeField]
    private int _powerupID;

    [SerializeField]
    private AudioClip _powerupSFX;

    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);

        if (transform.position.y <= -7)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            switch (_powerupID)
            {
                case 0:
                    collider.GetComponent<Player>().PickedTripleShot();
                    break;
                case 1:
                    collider.GetComponent<Player>().PickedSpeed();
                    break;
                case 2:
                    collider.GetComponent<Player>().PickedShield();
                    break;
                default:
                    break;
            }

            AudioSource.PlayClipAtPoint(_powerupSFX, Vector3.zero, 1f);

            Destroy(gameObject);
        }
    }
}
