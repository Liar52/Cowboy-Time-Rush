using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour
{
    [Header("Puntaje")]
    public int pointValue = 100;
    public GameObject hitEffectPrefab;

    [Header("Tiempo de vida")]
    [Tooltip("Segundos que el target permanece visible si no lo destruyen")]
    public float lifetime = 3f;

    private bool wasHit;

    private void Start()
    {
        // Si nadie le pega en 'lifetime' segundos, desaparece solo
        Destroy(gameObject, lifetime);
    }

    public void Hit()
    {
         if (wasHit || !GameManager.Instance.IsPlaying) return;
            wasHit = true;

       GameManager.Instance.AddScore(pointValue);

        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

        // Avisa al spawner que este target ya no está, para que spawnee otro
        TargetSpawner.Instance?.NotifyTargetDestroyed(this);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Por si se destruye por timeout (no por Hit), igual avisamos al spawner
        TargetSpawner.Instance?.NotifyTargetDestroyed(this);
    }
}