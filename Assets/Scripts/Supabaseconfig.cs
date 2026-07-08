using UnityEngine;

/// <summary>
/// Guarda la URL y la Anon Key de tu proyecto Supabase.
/// Poner este script en un GameObject vacío llamado "SupabaseConfig"
/// que exista desde el primer escena (por ejemplo junto a tu GameManager).
/// </summary>
public class SupabaseConfig : MonoBehaviour
{
    public static SupabaseConfig Instance { get; private set; }

    [Header("Credenciales de Supabase")]
    [Tooltip("Project Settings > API > Project URL. Ej: https://xxxx.supabase.co")]
    public string supabaseUrl;

    [Tooltip("Project Settings > API > Project API keys > anon public")]
    public string anonKey;

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
}