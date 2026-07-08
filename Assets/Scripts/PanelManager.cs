using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Header("Paneles del menú")]
    public GameObject playPanel;
    public GameObject optionsPanel;

    public void OpenOptions()
    {
        playPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        playPanel.SetActive(true);
    }
}