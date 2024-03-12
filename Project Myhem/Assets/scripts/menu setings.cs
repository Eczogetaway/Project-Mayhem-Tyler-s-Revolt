using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menusetings : MonoBehaviour
{
    public GameObject settingsPanel;
    public void PlayGame()
    {
        Application.LoadLevel("1 Level");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void SettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void Exit()
    {
        settingsPanel.SetActive(false);
    }

}
