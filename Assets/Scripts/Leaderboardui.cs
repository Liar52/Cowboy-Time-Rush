using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Muestra el top de puntajes en una lista vertical.
/// - contentParent: objeto con Vertical Layout Group donde se van a instanciar las filas.
/// - rowPrefab: prefab con (al menos) 3 componentes TMP_Text hijos, en este orden:
///     [0] posición (1º, 2º, 3º...)
///     [1] nombre de usuario
///     [2] puntaje
/// </summary>
public class LeaderboardUI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform contentParent;
    public GameObject rowPrefab;
    public int topCount = 10;

    private void Start()
    {
        RefreshLeaderboard();
    }

    public void RefreshLeaderboard()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        Supabasescoremanager.Instance.GetLeaderboard(topCount, OnLeaderboardLoaded);
    }

    private void OnLeaderboardLoaded(List<ScoreEntry> entries)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentParent);
            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

            string username = entries[i].players != null ? entries[i].players.username : "???";

            if (texts.Length >= 3)
            {
                texts[0].text = (i + 1) + "º";
                texts[1].text = username;
                texts[2].text = entries[i].score.ToString();
            }
        }
    }
}