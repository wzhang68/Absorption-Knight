using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Text text;
    private AudioSource musicplayer;
    private float musicVol = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        musicplayer = GetComponent<AudioSource>();
        text.text = Decimal.Round((decimal) (musicplayer.volume * 100)) + " %";
    }

    // Update is called once per frame
    void Update()
    {
        musicplayer.volume = musicVol;
        text.text =  Decimal.Round((decimal) (musicplayer.volume * 100)) + " %";
    }

    public void SetVolDown()
    {
        if (musicVol >= 0)
        {
            musicVol -= 0.1f;
        }
    }
    
    public void SetVolUp()
    {
        if (musicVol <= 1)
        {
            musicVol += 0.1f;
        }
    }
    
    
}
