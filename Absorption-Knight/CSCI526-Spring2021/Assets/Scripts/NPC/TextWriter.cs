using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    private Text text;
    private string textToWrite;
    private int index;
    private float timePerCharacter;
    private float timer;


    public void AddWriter(Text text, string textToWrite, float timePerCharacter)
    {
        this.text = text;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        index = 0;
    }
    
    
    void Update()
    {
        if (text != null)
        {
            timer -= Time.deltaTime;
            while (timer <= 0f)
            { 
                timer += timePerCharacter;
                index++;
                text.text = textToWrite.Substring(0, index);
                if (index >= textToWrite.Length)
                { 
                    text = null;
                    return;
                }
            }
        }
        
    }
}
