using UnityEngine;

public class Pistol : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;
    public float bulletLifetime = 5f; // seconds before bullet is destroyed

    [Header("Audio")]
    public AudioClip clip;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }

        if (AudioManager.Instance != null)
        {
            source.outputAudioMixerGroup = AudioManager.Instance.sfxGroup;
        }
    }
    public void FireBullet()
    {
        // Spawn bullet
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation 
        );

        // Apply velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }

        // Play gun sound
        if (clip != null && source != null)
        {
            source.PlayOneShot(clip);
        }

        // Destroy bullet after time
        Destroy(bullet, bulletLifetime);
    }
}