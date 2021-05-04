using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tempBehaviour : MonoBehaviour
{
    private Button respawn;
    private Button quit;
    
    // Start is called before the first frame update
    void Start()
    {
        respawn = GameObject.Find("Respawn").GetComponent<Button>();
        quit =GameObject.Find("Quit").GetComponent<Button>();
    }

    public void disableBeforeAnim()
    {
        if (transform.parent.name == "GameOverPanel")
        {
            respawn.interactable = false;
            quit.interactable = false;   
        }
        else
        {
            quit.interactable = false;   
        }
    }
    
    
    public void enableAfterAnim()
    {
        if (transform.parent.name == "GameOverPanel")
        {
            respawn.interactable = true;
            quit.interactable = true;
        }
        else
        {
            quit.interactable = true;
        }
    }
}
