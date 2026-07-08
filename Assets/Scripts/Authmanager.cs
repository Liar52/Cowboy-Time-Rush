using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Maneja registro y login contra las funciones register_player / login_player de Supabase.
/// Poner en el mismo GameObject que SupabaseConfig, o en uno propio persistente.
/// </summary>
public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }

    public string CurrentPlayerId { get; private set; }
    public string CurrentUsername { get; private set; }
    public bool IsLoggedIn => !string.IsNullOrEmpty(CurrentPlayerId);

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

    public void Register(string username, string password, Action<bool, string> callback)
    {
        StartCoroutine(CallAuthFunction("register_player", username, password, callback));
    }

    public void Login(string username, string password, Action<bool, string> callback)
    {
        StartCoroutine(CallAuthFunction("login_player", username, password, callback));
    }

    public void Logout()
    {
        CurrentPlayerId = null;
        CurrentUsername = null;
    }

    private IEnumerator CallAuthFunction(string functionName, string username, string password, Action<bool, string> callback)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            callback?.Invoke(false, "Usuario y contraseña son obligatorios");
            yield break;
        }

        string hash = HashPassword(password);
        string url = $"{SupabaseConfig.Instance.supabaseUrl}/rest/v1/rpc/{functionName}";
        string body = JsonUtility.ToJson(new AuthRequest { p_username = username, p_password_hash = hash });

        using (UnityWebRequest req = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("apikey", SupabaseConfig.Instance.anonKey);
            req.SetRequestHeader("Authorization", "Bearer " + SupabaseConfig.Instance.anonKey);
            req.SetRequestHeader("Prefer", "return=representation");

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                callback?.Invoke(false, "Error de conexión: " + req.error);
                yield break;
            }

            RpcResult[] results = JsonHelper.FromJson<RpcResult>(req.downloadHandler.text);
            if (results == null || results.Length == 0)
            {
                callback?.Invoke(false, "Respuesta inesperada del servidor");
                yield break;
            }

            RpcResult result = results[0];
            if (result.success)
            {
                CurrentPlayerId = result.id;
                CurrentUsername = username;
            }
            callback?.Invoke(result.success, result.message);
        }
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    [Serializable]
    private class AuthRequest
    {
        public string p_username;
        public string p_password_hash;
    }

    [Serializable]
    private class RpcResult
    {
        public string id;
        public bool success;
        public string message;
    }
}