using UnityEngine;

public class AsteroidScorerScript : MonoBehaviour
{
    [SerializeField]
    private ScoreScript _Score;

    private void Start()
    {
        foreach (var ass in FindObjectsOfType<AsteroidSpawnerScript>())
        {
            ass.AsteroidSpawnedEvent.AddListener(a =>
            {
                a.AsteroidDestroyedEvent.AddListener(() =>
                {
                    _Score.IncrementScore(a.MaxHealth);
                });
            });
        }
    }
}
