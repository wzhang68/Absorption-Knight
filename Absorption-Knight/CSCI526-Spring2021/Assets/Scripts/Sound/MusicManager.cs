using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    // public static string BMusic_Boss, BMusic_LV1, BMusic_LV2, BMusic_LV3, BMusic_Village,BMusic_Boss2;
    public Sound[] sounds;
    public static MusicManager instance;
    private Slider musicControl;



    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            UnityEngine.Object.Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playAwake;
        }
        
    }
    
    private void Update()
    {        
        musicControl = GameObject.Find("Canvas").transform.GetChild(9).transform.GetChild(2).transform.GetChild(1)
            .GetComponent<Slider>();
        foreach (Sound s in sounds)
        {
            s.source.volume = musicControl.value;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        foreach (Sound c in sounds)
        {
            if (c.source.isPlaying)
            {
                c.source.Stop();
            }

        }

        s.source.Play();
        Debug.Log(s.source.clip.name);
        Debug.Log(s.source.isPlaying);

    }

    public void SetVol(float val)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = musicControl.value;
        }
    }


}

