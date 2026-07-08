using UnityEngine;
using UnityEngine.SceneManagement;


public class ExitToMenu : MonoBehaviour
{
   
    [SerializeField] private string menuSceneName = "Menu";

    public void CargarMenu()
    {
        if (string.IsNullOrEmpty(menuSceneName))
        {
            Debug.LogError("El nombre de la escena del menu esta vacio.");
            return;
        }

        Debug.Log("Volviendo al menu: " + menuSceneName);
        SceneManager.LoadScene(menuSceneName);
    }
}