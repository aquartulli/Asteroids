using System.Collections;
using System.Linq;
using UnityEngine;

public class AsteroidSpawnerScript : MonoBehaviour
{
    [SerializeField]
    private float _SpawnIntervalInSeconds;

    [SerializeField]
    private float _SpawnDistance;

    [SerializeField]
    private float _MinAsteroidSpeed;

    [SerializeField]
    private float _MaxAsteroidSpeed;

    [SerializeField]
    private AsteroidSpawnInfo[] _AsteroidSpawnInfos;

    [SerializeField]
    private float _SpawnAngleDeviation;

    [SerializeField]
    private AsteroidSpawnedEvent _AsteroidSpawnedEvent;

    private bool _IsStopped;
    private float _TotalAsteroidSpawnWeights;

    public AsteroidSpawnedEvent AsteroidSpawnedEvent => _AsteroidSpawnedEvent;

    private void Awake()
    {
        _TotalAsteroidSpawnWeights = _AsteroidSpawnInfos.Aggregate(0f, (total, asi) => total + asi.Weight);
    }

    private void Start()
    {
        StartCoroutine(SpawnAsteroids());
        StartCoroutine(ReduceSpawnInterval());
    }

    private GameObject GetWeightedRandomAsteroidPrefab()
    {
        var random = Random.Range(0, _TotalAsteroidSpawnWeights);

        var currentTotal = 0f;
        var i = -1;

        while (random >= currentTotal)
        {
            currentTotal += _AsteroidSpawnInfos[++i].Weight;
        }

        return _AsteroidSpawnInfos[i].AsteroidPrefab;
    }

    IEnumerator SpawnAsteroids()
    {
        while (!_IsStopped)
        {
            SpawnAsteroid();
            yield return new WaitForSeconds(_SpawnIntervalInSeconds);
        }
    }

    IEnumerator ReduceSpawnInterval()
    {
        while (!_IsStopped)
        {
            _SpawnIntervalInSeconds = _SpawnIntervalInSeconds * .99f;
            yield return new WaitForSeconds(1);
        }
    }

    private Vector2 GetRandomizedVelocity(Vector2 position)
    {
        var baseVelocity = -position.normalized;
        var randomVelocityAngle = Random.Range(-_SpawnAngleDeviation, _SpawnAngleDeviation);
        var sin = Mathf.Sin(randomVelocityAngle * Mathf.Deg2Rad);
        var cos = Mathf.Cos(randomVelocityAngle * Mathf.Deg2Rad);

        return new Vector2(cos * baseVelocity.x - sin * baseVelocity.y, sin * baseVelocity.x + cos * baseVelocity.y) * Random.Range(_MinAsteroidSpeed, _MaxAsteroidSpeed);
    }

    private void SpawnAsteroid()
    {
        var spawnPosition = Random.insideUnitCircle.normalized * _SpawnDistance;
        var spawnRotation = Random.Range(0, 360);

        var prefab = GetWeightedRandomAsteroidPrefab();
        var asteroid = Instantiate(prefab, spawnPosition, Quaternion.Euler(0, 0, spawnRotation));
        var asteroidRigidbody2d = asteroid.GetComponent<Rigidbody2D>();
        asteroidRigidbody2d.velocity = GetRandomizedVelocity(spawnPosition);

        var asteroidScript = asteroid.GetComponent<AsteroidScript>();
        _AsteroidSpawnedEvent.Invoke(asteroidScript);
    }
}
