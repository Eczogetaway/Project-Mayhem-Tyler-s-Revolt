using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menusetings : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainPanel;
    static public bool isOpenSettings;
    public void PlayGame()
    {
        Application.LoadLevel("Loading");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void SettingsPanel()
    {
        settingsPanel.SetActive(true);
        mainPanel.SetActive(false);
        isOpenSettings = true;
    }

    public void Exit()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
        isOpenSettings = false;
    }

}
