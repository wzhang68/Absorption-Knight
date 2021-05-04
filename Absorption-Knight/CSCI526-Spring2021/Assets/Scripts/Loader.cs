using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public MusicManager audioManager;

    private void Awake()
    {
        if (GameManager.instance == null)
        {
            GameObject GM = Instantiate(gameManager);
            GM.name = GM.name.Replace("(Clone)", "");
        }else if (GameManager.instance != null)
        {
            Destroy(gameManager);
        }
        
        if (MusicManager.instance == null)
        {
            Instantiate(audioManager);
        }
    }


}
