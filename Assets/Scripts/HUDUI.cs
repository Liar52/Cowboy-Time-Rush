using TMPro;
using UnityEngine;

public class HUDUI : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text scoreText;

    private void OnEnable()
    {
        GameManager.Instance.OnTimerUpdated += UpdateTimer;
        GameManager.Instance.OnScoreUpdated += UpdateScore;
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnTimerUpdated -= UpdateTimer;
        GameManager.Instance.OnScoreUpdated -= UpdateScore;
    }

    private void UpdateTimer(float secondsRemaining)
    {
        int seconds = Mathf.CeilToInt(secondsRemaining);
        timerText.text = $"{seconds / 60:00}:{seconds % 60:00}";
    }

    private void UpdateScore(int score)
    {
        scoreText.text = $"Puntaje: {score}";
    }
}