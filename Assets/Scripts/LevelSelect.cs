using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Coloca este script en el Panel/Play (o en un GameObject manager)
/// y llama a los metodos publicos desde:
///  - El evento OnClick() de un Button normal, o
///  - El evento "Select Entered" de un XR Simple Interactable (VR)
/// </summary>
public class LevelSelector : MonoBehaviour
{
    // Nombres EXACTOS de las escenas (deben estar agregadas en
    // File > Build Settings > Scenes In Build)
    [SerializeField] private string nivel1SceneName = "MainVr";
    [SerializeField] private string nivel2SceneName = "Level2";

    // Metodo para el boton "NIVEL 1"
    public void CargarNivel1()
    {
        CargarEscena(nivel1SceneName);
    }

    // Metodo para el boton "NIVEL 2"
    public void CargarNivel2()
    {
        CargarEscena(nivel2SceneName);
    }

    // Metodo generico reutilizable
    private void CargarEscena(string nombreEscena)
    {
        if (string.IsNullOrEmpty(nombreEscena))
        {
            Debug.LogError("El nombre de la escena esta vacio.");
            return;
        }

        Debug.Log("Cargando escena: " + nombreEscena);
        SceneManager.LoadScene(nombreEscena);
    }
}