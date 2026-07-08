using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public static TargetSpawner Instance { get; private set; }

    [Header("Prefab y puntos de spawn")]
    public GameObject targetPrefab;
    public Transform[] spawnPoints;

    [Header("Timing")]
    public float spawnInterval = 1.5f;
    public int maxActiveTargets = 3;

    private readonly Dictionary<Target, Transform> activeTargets = new Dictionary<Target, Transform>();
    private Coroutine spawnRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void StartSpawning()
    {
        StopSpawning();
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);

        foreach (var kvp in activeTargets)
            if (kvp.Key != null) Destroy(kvp.Key.gameObject);

        activeTargets.Clear();
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (activeTargets.Count < maxActiveTargets)
                SpawnOne();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnOne()
    {
        Transform point = GetFreeSpawnPoint();
        if (point == null) return;

        GameObject obj = Instantiate(targetPrefab, point.position, point.rotation);
        Target target = obj.GetComponent<Target>();

        activeTargets.Add(target, point);
    }

    private Transform GetFreeSpawnPoint()
    {
        List<Transform> free = new List<Transform>();
        foreach (var p in spawnPoints)
            if (!activeTargets.ContainsValue(p)) free.Add(p);

        if (free.Count == 0) return null;
        return free[Random.Range(0, free.Count)];
    }

    public void NotifyTargetDestroyed(Target target)
    {
        activeTargets.Remove(target);
    }
}