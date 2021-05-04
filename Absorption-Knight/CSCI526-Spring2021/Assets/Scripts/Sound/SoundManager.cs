using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager: MonoBehaviour
{
    public static AudioClip playerGetHit,
        jumpSound,
        playerAttack,
        dropInWater,
        greetingMale1,
        greetingMale2,
        greetingMale3,
        greetingMale4,
        greetingMale5,
        greetingMale6,
        greetingMale7,
        sanityDrop,
        sanityUp;
    // public static AudioClip BMusic_LV1, BMusic_LV2, BMusic_LV3, BMusic_Village, BMusic_Boss, BMusic_Boss2;   
    private static AudioSource audioSrc;
    private Slider SEControl;
    private void Start()
    {
        playerGetHit = Resources.Load<AudioClip>("Sound_Effect/Male5");
        jumpSound = Resources.Load<AudioClip>("Sound_Effect/jump4");
        playerAttack = Resources.Load<AudioClip>("Sound_Effect/Hit3");
        dropInWater = Resources.Load<AudioClip>("Sound_Effect/BigSplash2");
        greetingMale1 = Resources.Load<AudioClip>("Sound_Effect/greeting1");
        greetingMale2 = Resources.Load<AudioClip>("Sound_Effect/greeting2"); 
        greetingMale3 = Resources.Load<AudioClip>("Sound_Effect/greeting3");
        greetingMale4 = Resources.Load<AudioClip>("Sound_Effect/greeting_2");
        greetingMale5 = Resources.Load<AudioClip>("Sound_Effect/greeting5");
        greetingMale6 = Resources.Load<AudioClip>("Sound_Effect/greeting6");
        greetingMale7 = Resources.Load<AudioClip>("Sound_Effect/greeting7");
        sanityDrop =Resources.Load<AudioClip>("Sound_Effect/sanityDrop");
        sanityUp =Resources.Load<AudioClip>("Sound_Effect/sanityUp");
        
        
        // BMusic_Boss = Resources.Load<AudioClip>("JRPGMusic/Wav/1)");
        // BMusic_Boss2 = Resources.Load<AudioClip>("JRPGMusic/Wav/2");
        // BMusic_LV3 = Resources.Load<AudioClip>("JRPGMusic/Wav/3");
        // BMusic_Village = Resources.Load<AudioClip>("GameMusicPack/Tracks/Medieval/AMedievalTale");
        // BMusic_LV1 = Resources.Load<AudioClip>("GameMusicPack/Tracks/Dark/Lost");
        // BMusic_LV2 = Resources.Load<AudioClip>("GameMusicPack/Tracks/Dark/TheLastofIt");
        
       SEControl = GameObject.Find("Canvas").transform.GetChild(9).transform.GetChild(3).transform.GetChild(1)
            .GetComponent<Slider>();
        
        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSrc.volume = SEControl.value;
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "playerGetHit":
                audioSrc.PlayOneShot(playerGetHit, 1.0f);
                break;
            case "jumpSound":
                audioSrc.PlayOneShot(jumpSound,  1.0f);
                break;
            case "playerAttack":
                audioSrc.PlayOneShot(playerAttack, 1.0f);
                break;
            case "sanityDrop":
                audioSrc.PlayOneShot(sanityDrop, 1.0f);
                break;
            case "sanityUp":
                audioSrc.PlayOneShot(sanityUp,1.0f);
                break;
            case "dropInWater":
                audioSrc.PlayOneShot(dropInWater, 1.0f);
                break;
            case "MGreeting1":
                audioSrc.PlayOneShot(greetingMale1, 1.0f);
                break;
            case "MGreeting2":
                audioSrc.PlayOneShot(greetingMale2, 1.0f);
                break;
            case "MGreeting3":
                audioSrc.PlayOneShot(greetingMale3, 1.0f);
                break;
            case "MGreeting4":
                audioSrc.PlayOneShot(greetingMale4, 1.0f);
                break;
            case "MGreeting5":
                audioSrc.PlayOneShot(greetingMale5, 1.0f);
                break;
            case "MGreeting6":
                audioSrc.PlayOneShot(greetingMale6, 1.0f);
                break;
            case "MGreeting7":
                audioSrc.PlayOneShot(greetingMale7, 1.0f);
                break;
        }
    }
    
}
