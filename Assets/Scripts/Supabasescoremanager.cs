using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ScoreEntry
{
    public int score;
    public string created_at;
    public PlayerRef players;
}

[Serializable]
public class PlayerRef
{
    public string username;
}

/// <summary>
/// Guarda el puntaje final de una partida y obtiene el leaderboard (top scores).
/// Requiere que AuthManager.Instance.IsLoggedIn sea true antes de llamar SubmitScore.
/// </summary>
public class Supabasescoremanager : MonoBehaviour
{
    public static Supabasescoremanager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SubmitScore(int score, Action<bool, string> callback = null)
    {
        StartCoroutine(SubmitScoreCoroutine(score, callback));
    }

    public void GetLeaderboard(int limit, Action<List<ScoreEntry>> callback)
    {
        StartCoroutine(GetLeaderboardCoroutine(limit, callback));
    }

    private IEnumerator SubmitScoreCoroutine(int score, Action<bool, string> callback)
    {
        if (AuthManager.Instance == null || !AuthManager.Instance.IsLoggedIn)
        {
            callback?.Invoke(false, "No hay usuario logueado");
            yield break;
        }

        string url = $"{SupabaseConfig.Instance.supabaseUrl}/rest/v1/scores";
        string body = JsonUtility.ToJson(new ScoreInsert
        {
            player_id = AuthManager.Instance.CurrentPlayerId,
            score = score
        });

        using (UnityWebRequest req = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("apikey", SupabaseConfig.Instance.anonKey);
            req.SetRequestHeader("Authorization", "Bearer " + SupabaseConfig.Instance.anonKey);
            req.SetRequestHeader("Prefer", "return=minimal");

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                callback?.Invoke(false, "Error al guardar puntaje: " + req.error);
            }
            else
            {
                callback?.Invoke(true, "Puntaje guardado");
            }
        }
    }

    private IEnumerator GetLeaderboardCoroutine(int limit, Action<List<ScoreEntry>> callback)
    {
        string url = $"{SupabaseConfig.Instance.supabaseUrl}/rest/v1/scores" +
                      $"?select=score,created_at,players(username)&order=score.desc&limit={limit}";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.SetRequestHeader("apikey", SupabaseConfig.Instance.anonKey);
            req.SetRequestHeader("Authorization", "Bearer " + SupabaseConfig.Instance.anonKey);

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al obtener leaderboard: " + req.error);
                callback?.Invoke(new List<ScoreEntry>());
                yield break;
            }

            ScoreEntry[] entries = JsonHelper.FromJson<ScoreEntry>(req.downloadHandler.text);
            callback?.Invoke(new List<ScoreEntry>(entries ?? new ScoreEntry[0]));
        }
    }

    [Serializable]
    private class ScoreInsert
    {
        public string player_id;
        public int score;
    }
}