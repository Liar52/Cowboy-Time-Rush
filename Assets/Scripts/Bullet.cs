using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Impacto")]
    public GameObject impactEffectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        Target target = other.GetComponentInParent<Target>();

        if (target == null) return;

        target.Hit();

        if (impactEffectPrefab != null)
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}