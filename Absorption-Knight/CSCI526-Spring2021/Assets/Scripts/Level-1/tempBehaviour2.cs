using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tempBehaviour2 : MonoBehaviour
{
    private Button quit;
    
    // Start is called before the first frame update
    void Start()
    {
        quit =GameObject.Find("Quit").GetComponent<Button>();
    }

    public void disableBeforeAnim()
    {
        quit.interactable = false;
    }
    
    
    public void enableAfterAnim()
    {
        quit.interactable = true;
    }
}
