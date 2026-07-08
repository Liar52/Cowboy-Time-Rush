using UnityEngine;
using UnityEngine.SceneManagement;
 
public class MenuNavigation : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName.Trim());
    }
 
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
 