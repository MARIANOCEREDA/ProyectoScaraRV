using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    //public GameObject robot;

    [SerializeField] public GameObject MainMenuPanel;
    [SerializeField] public GameObject ConfigurationMenuPanel;
    public void StartGame()
    {
        MainMenuPanel.SetActive(false);
        Debug.Log("Game Started");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    public void SetConfigurations()
    {
        MainMenuPanel.SetActive(false);
        ConfigurationMenuPanel.SetActive(true);
        Debug.Log("Setting configurations ...");
    }

}
