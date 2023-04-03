using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject PauseMenuPanel;

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
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Exit Game ...");
    }
}
