using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float _Speed;

    [SerializeField]
    private float _LifetimeInSeconds;

    [SerializeField]
    private AudioClip _HitSound;

    public AudioSource AudioSource { get; set; }

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * _Speed;
        DespawnAfterLifetime();
    }
    
    private void DespawnAfterLifetime()
    {
        Destroy(gameObject, _LifetimeInSeconds);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AsteroidScript asteroidScript;
        asteroidScript = collision.GetComponent<AsteroidScript>();
        if (asteroidScript != null)
        {
            asteroidScript.Hit();
            AudioSource.PlayOneShot(_HitSound);
        }

        Destroy(gameObject);
    }
}
