using System;
using UnityEngine;

[Serializable]
public class AsteroidSpawnInfo
{
    [SerializeField]
    private float _Weight;
    public float Weight => _Weight;

    [SerializeField]
    private GameObject _AsteroidPrefab;
    public GameObject AsteroidPrefab => _AsteroidPrefab;
}
