using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float _Acceleration;
    [SerializeField]
    private float _Deceleration;
    [SerializeField]
    private float _MaxSpeed;
    [SerializeField]
    private GameObject _BulletPrefab;
    [SerializeField]
    private Transform _BulletSpawnPosition;
    [SerializeField]
    private PlayerDiedEvent _PlayerDiedEvent;
    [SerializeField]
    private AudioSource _AudioSource;

    private bool _Accelerate;
    private Rigidbody2D _Rigidbody2D;

    public PlayerDiedEvent PlayerDiedEvent => _PlayerDiedEvent;

    public void Kill()
    {
        PlayerDiedEvent.Invoke();
        Destroy(gameObject);
    }

    private void Awake()
    {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        var mousePosition = Input.mousePosition;
        var mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);

        // Get Angle in Radians
        float angleRad = Mathf.Atan2(mousePositionInWorld.y - _Rigidbody2D.position.y, mousePositionInWorld.x - _Rigidbody2D.position.x);
        // Get Angle in Degrees
        float angleDeg = (180 / Mathf.PI) * angleRad;
        // Rotate Object
        _Rigidbody2D.SetRotation(angleDeg - 90);

        _Accelerate = Input.GetKey(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Vector2 newVelocity;

        if (_Accelerate)
        {
            newVelocity = _Rigidbody2D.velocity + (Vector2)transform.up * _Acceleration * Time.fixedDeltaTime;

            if (newVelocity.sqrMagnitude > _MaxSpeed * _MaxSpeed)
            {
                newVelocity = newVelocity.normalized * _MaxSpeed;
            }
        }
        else
        {
            if (_Rigidbody2D.velocity.sqrMagnitude <= _Deceleration * _Deceleration * Time.fixedDeltaTime * Time.fixedDeltaTime)
            {
                newVelocity = Vector2.zero;
            }
            else
            {
                newVelocity = _Rigidbody2D.velocity - _Rigidbody2D.velocity.normalized * _Deceleration * Time.fixedDeltaTime;
            }
        }
        
        _Rigidbody2D.velocity = newVelocity;
    }

    private void Shoot()
    {
        var bullet = Instantiate(_BulletPrefab, _BulletSpawnPosition.position, transform.rotation);
        var bulletScript = bullet.GetComponent<BulletScript>();
        bulletScript.AudioSource = _AudioSource;
    }
}
