using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject PauseMenuPanel;
    [SerializeField] public GameObject MainMenuPanel;
    [SerializeField] public GameObject ConfigurationPausePanel;


    // Update is called once per frame
    public void Pause()
    {
        PauseMenuPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        PauseMenuPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SampleScene");
        MainMenuPanel.SetActive(false);

    }

    public void DisplayConfiguration()
    {
        PauseMenuPanel.SetActive(false);
        ConfigurationPausePanel.SetActive(true);
    }

    public void Exit()
    {
        MainMenuPanel.SetActive(true);
        Application.Quit();
        Debug.Log("Exit Game ...");
    }

}
