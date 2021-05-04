using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPageController : MonoBehaviour
{


    // x:1344 y: 621 z:-2981.065
    // private Camera mainCamera;
    private AudioSource myAudioSource;
    private float musicSliderValue;
    public GameObject StartBtn;
    public GameObject loadScreen;
    public Slider slider;
    public Text text;
    private bool flag;


    #region Unity Methods

    void Start()
    {


    }

    void Update()
    {
    }
    #endregion



    #region Public Methods

    // After users click the Start button, then camera moves down, and focus on the main character
    // To get better running speed, disable particle system
    public void StartBehavior()
    {
        StartBtn.SetActive(false);
        loadScreen.SetActive(true);
        LoadNextLevel();

    }

    public void LoadNextLevel()
    {
        StartCoroutine(Loadlevel());
    }
    

    #endregion
    
    #region Private Methods
    
    
    private IEnumerator Loadlevel()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            slider.value = operation.progress;
            text.text = operation.progress * 100 + " %";

            if (operation.progress >= 0.9f)
            {
                slider.value = 1;
                text.text = "Touch Screen to Continue...";
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }
            
            yield return null;
            
        }
        
    }
    

    #endregion
}
