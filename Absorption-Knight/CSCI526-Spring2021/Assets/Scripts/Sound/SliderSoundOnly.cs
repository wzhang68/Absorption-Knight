using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSoundOnly : MonoBehaviour
{
    private Slider musiccontrol;
    void Awake()
    {
        musiccontrol = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        MusicManager.instance.SetVol(musiccontrol.value);
    }
}
