using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSoundOnly : MonoBehaviour
{
    private AudioSource audio;

    private Slider SEControl;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        SEControl = GameObject.Find("Canvas").transform.GetChild(9).transform.GetChild(3).transform.GetChild(1)
            .GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        audio.volume = SEControl.value;
    }
}
