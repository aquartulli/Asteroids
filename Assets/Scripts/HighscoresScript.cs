using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighscoresScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _HighscoreTextPrefab;

    [SerializeField]
    private Text _PressAnyKeyToContinueText;

    [SerializeField]
    private Transform _TopScorePosition;

    [SerializeField]
    private float _Spacing;

    [SerializeField]
    private float _InputDelay;

    [SerializeField]
    private float _FadeInDuration;

    private bool _AcceptingInput;

    private void Start()
    { 
        const string filepath = "highscores";

        var highscores = File.ReadAllText(filepath)
            .Split(',');

        var position = _TopScorePosition.position;

        foreach (var highscore in highscores)
        {
            var highscoreGo = Instantiate(_HighscoreTextPrefab, position, Quaternion.identity, transform);
            highscoreGo.GetComponent<Text>().text = highscore;
            position = position + Vector3.down * _Spacing;
        }

        StartCoroutine(FadeInInput());
    }

    private void Update()
    {
        if (_AcceptingInput && Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    private IEnumerator FadeInInput()
    {
        yield return new WaitForSeconds(_InputDelay);

        _AcceptingInput = true;

        while (_PressAnyKeyToContinueText.color.a < 1f)
        {
            _PressAnyKeyToContinueText.color = new Color(
                _PressAnyKeyToContinueText.color.r,
                _PressAnyKeyToContinueText.color.g,
                _PressAnyKeyToContinueText.color.b,
                Mathf.Min(_PressAnyKeyToContinueText.color.a + Time.deltaTime / _FadeInDuration, 1f));
            yield return new WaitForEndOfFrame();
        }
    }
}