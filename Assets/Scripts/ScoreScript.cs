using System;
using System.Collections;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Text))]
public class ScoreScript : MonoBehaviour
{
    public int Score { get; private set; }

    private Text _Text;
    private bool _IsTickingStopped;

    private void Awake()
    {
        _Text = GetComponent<Text>();
    }

    private void Start()
    {
        FindObjectOfType<PlayerScript>().PlayerDiedEvent.AddListener(() =>
        {
            StopTicking();
            WriteHighscores();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });

        StartCoroutine(IncreaseScoreOverTime());
    }

    private IEnumerator IncreaseScoreOverTime()
    {
        while (!_IsTickingStopped)
        {
            IncrementScore(1);
            yield return new WaitForSeconds(1);
        }
    }

    private void StopTicking()
    {
        _IsTickingStopped = true;
    }
    
    private void WriteHighscores()
    {
        const string filepath = "highscores";

        int[] highscores;
        if (File.Exists(filepath))
        {
            highscores = File.ReadAllText(filepath)
                .Split(',')
                .Select(int.Parse)
                .Concat(new[] { Score })
                .OrderByDescending(i => i)
                .Take(5)
                .ToArray();
        }
        else
        {
            highscores = new[] { Score };
        }

        var serialized = string.Join(",", highscores.Select(i => i.ToString()));
        File.WriteAllText(filepath, serialized);
    }

    public void IncrementScore(int delta)
    {
        if (delta <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(delta));
        }

        Score = Score + delta;
        _Text.text = "Score: " + Score;
    }
}
