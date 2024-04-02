using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pause_menu : MonoBehaviour
{
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject Crosshair;
    public GameObject Player;
    public GameObject Pistol;
    public GameObject settingsPanel;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (GameIsPaused)
            {
                if (menusetings.isOpenSettings)
                {
                    settingsPanel.SetActive(false);
                    pauseMenuUI.SetActive(true);
                    menusetings.isOpenSettings = false;
                } else
                {

                    Pistol.GetComponent<shoot>().enabled = true;
                    Player.GetComponent<SC_FPSController>().enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Crosshair.gameObject.SetActive(true);
                    Resume();
                }

            }
            else
            {
                Pistol.GetComponent<shoot>().enabled = false;
                Player.GetComponent<SC_FPSController>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Crosshair.gameObject.SetActive(false);
                Pause();
            }

        }

    }
    public void Resume()
    {
        Pistol.GetComponent<shoot>().enabled = true;
        Player.GetComponent<SC_FPSController>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Crosshair.gameObject.SetActive(true);
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Restart()
    {

        Time.timeScale = 1f;
        Application.LoadLevel("1 Level");
        GameIsPaused = false;
    }

    public void QuitGame()
    {
        
        Application.Quit();
    }
}