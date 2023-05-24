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
        // Set joint limits
        for (int i = 0; i < ScaraController.Instance.GetNRotationJoints() ; i++)
        {
            ScaraController.Instance.SetProperties(ScaraController.Instance.joints[i].robotPart, -90f, 90f);
        }
    }
}
