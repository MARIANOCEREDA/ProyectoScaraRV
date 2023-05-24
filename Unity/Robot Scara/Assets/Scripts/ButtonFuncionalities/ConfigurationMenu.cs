using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject MainMenuPanel;
    [SerializeField] public GameObject ConfigurationMenuPanel;
    public void BackToMenu()
    {
        ConfigurationMenuPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        Debug.Log("Menu Panel Active.");
    }

    public void SetButton()
    {
        
    }

}
