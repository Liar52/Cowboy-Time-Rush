using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Conecta los InputFields y Botones de tu pantalla de Login/Registro con AuthManager.
/// Requiere TextMeshPro para los inputs y textos.
/// </summary>
public class LoginUI : MonoBehaviour
{
    [Header("Campos de entrada")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    [Header("Botones")]
    public Button loginButton;
    public Button registerButton;

    [Header("Feedback")]
    public TMP_Text messageText;

    [Header("Paneles (opcional, para cambiar de pantalla al loguearse)")]
    public GameObject loginPanel;
    public GameObject gamePanel;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);
    }

    private void OnLoginClicked()
    {
        SetMessage("Conectando...");
        AuthManager.Instance.Login(usernameInput.text, passwordInput.text, OnAuthResult);
    }

    private void OnRegisterClicked()
    {
        SetMessage("Registrando...");
        AuthManager.Instance.Register(usernameInput.text, passwordInput.text, OnAuthResult);
    }

    private void OnAuthResult(bool success, string message)
    {
        SetMessage(message);
        if (success)
        {
            if (loginPanel != null) loginPanel.SetActive(false);
            if (gamePanel != null) gamePanel.SetActive(true);
        }
    }

    private void SetMessage(string msg)
    {
        if (messageText != null) messageText.text = msg;
    }
}