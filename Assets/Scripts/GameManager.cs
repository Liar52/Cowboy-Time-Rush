using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configuración de partida")]
    public float matchDuration = 60f;

    public event Action<float> OnTimerUpdated;
    public event Action<int> OnScoreUpdated;
    public event Action OnGameStarted;
    public event Action OnGameEnded;

    public bool IsPlaying { get; private set; }
    public int CurrentScore { get; private set; }

    private float timeRemaining;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!IsPlaying) return;

        timeRemaining -= Time.deltaTime;
        OnTimerUpdated?.Invoke(Mathf.Max(timeRemaining, 0f));

        if (timeRemaining <= 0f)
        {
            EndGame();
        }
    }

    public void StartGame()
    {
        Debug.Log("StartGame() se ejecutó"); // 👈 temporal

        if (IsPlaying) return;

        CurrentScore = 0;
        timeRemaining = matchDuration;
        IsPlaying = true;

        OnScoreUpdated?.Invoke(CurrentScore);
        OnTimerUpdated?.Invoke(timeRemaining);
        OnGameStarted?.Invoke();

        TargetSpawner.Instance.StartSpawning();
    }

    public void AddScore(int points)
    {
        if (!IsPlaying) return;

        CurrentScore += points;
        OnScoreUpdated?.Invoke(CurrentScore);
    }

    private void EndGame()
    {
        IsPlaying = false;
        timeRemaining = 0f;

        TargetSpawner.Instance.StopSpawning();

        OnGameEnded?.Invoke();
        Debug.Log($"Partida terminada. Puntaje final: {CurrentScore}");

        // Acá más adelante conectamos SupabaseManager.SubmitScore(...)
    }
}