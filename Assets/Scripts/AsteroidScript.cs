using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField]
    private int _MaxHealth;

    [SerializeField]
    private float _DespawnDistance;

    [SerializeField]
    private AsteroidDestroyedEvent _AsteroidDestroyedEvent;

    private int _CurrentHealth;

    public int MaxHealth => _MaxHealth;

    public AsteroidDestroyedEvent AsteroidDestroyedEvent => _AsteroidDestroyedEvent;
    
    private void Awake()
    {
        _CurrentHealth = _MaxHealth;
    }

    public void Hit()
    {
        if (--_CurrentHealth <= 0)
        {
            AsteroidDestroyedEvent.Invoke();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (transform.position.sqrMagnitude > _DespawnDistance * _DespawnDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerScript>()?.Kill();
    }
}
