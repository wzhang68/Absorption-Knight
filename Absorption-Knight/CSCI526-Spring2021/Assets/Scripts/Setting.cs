using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    private static bool ispause = false;
    private bool mission_isactive = false;
    public GameObject mainPanel;
    public GameObject settingPanel;
    public GameObject teamPanel;
    public GameObject missionPanel;

    public void Resume()
    {
        mainPanel.SetActive(false);
        Time.timeScale = 1f;
        ispause = false;
    }

    public void Pause()
    {
        mainPanel.SetActive(true);
        Time.timeScale = 0f;
        ispause = true;
    }
    

    public void SettingMenu()
    {
        mainPanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    public void Mission()
    {
        if (mission_isactive)
        {
            missionPanel.SetActive(false);
            mission_isactive = false;
        }
        else
        {
            missionPanel.SetActive(true);
            mission_isactive = true;
        }
    }

    public void AboutUs()
    {
        settingPanel.SetActive(false);
        teamPanel.SetActive(true);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.txt"))
        {
            File.Delete(Application.persistentDataPath +"/playerData.txt");   
        }
        Application.Quit();
        Debug.Log("Quit game and Data Deleted");   
    }
    
}
