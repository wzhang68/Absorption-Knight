using System;
using System.Collections;
using UnityEngine;


public class Additional_Teodoro_LV0 : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject vol;
    [SerializeField] private GameObject help;
    [SerializeField] private GameObject sign;
    [SerializeField] private GameObject volumeText;
    [SerializeField] private GameObject volUp;
    [SerializeField] private GameObject volDown;
    [SerializeField] private GameObject signPanel;
    [SerializeField] private GameObject helpPanel;
    private PlayerUnit m_player;
    private Teodoro teodoro;
    private string tempTag;

    private void Awake()
    {
        m_player = GetComponent<PlayerUnit>();
        teodoro = GetComponent<Teodoro>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        tempTag = other.gameObject.name;
        if (other.gameObject.CompareTag("Level-0"))
        {
            switch (tempTag)
            {
                case "MusicBtn":
                    MusicFlashToYellow();
                    break;
                case "Help":
                    HelpFlashToWhite();
                    break;
                case "Sign":
                    SignFlashToYellow();
                    break;
            }
        }
        
        if (other.gameObject.name == "Level-1"){
            DoorFlashToYellow();
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        tempTag = String.Empty;
        signPanel.SetActive(false);
        helpPanel.SetActive(false);
        volDown.SetActive(false);
        volUp.SetActive(false);
        volumeText.SetActive(false);
        DoorFlashToWhite();
        MusicFlashToWhite();
        SignFlashToWhite();;
        HelpFlashToYellow();
    }
    
    public void InteractionBtn()
    {
        Debug.Log("Triggered");
        if (tempTag=="MusicBtn")
        {
            volDown.SetActive(true);
            volUp.SetActive(true);
            volumeText.SetActive(true);
        }

        if (tempTag=="Sign")
        {
            signPanel.SetActive(true);
        }

        if (tempTag=="Help")
        {
            helpPanel.SetActive(true);
        }
        
        if (tempTag=="teleport")
        {
            teodoro.atkBtn();
        }
    }
    

    public void DoorFlashToYellow()
    {
        StartCoroutine(DoorColorToYellow());
    }
    public void DoorFlashToWhite()
    {
        StartCoroutine(DoorColorToWhite());
    }
    
    public void MusicFlashToYellow()
    {
        StartCoroutine(MusicColorToYellow());
    }
    public void MusicFlashToWhite()
    {
        StartCoroutine(MusicColorToWhite());
    }
    
    
    public void SignFlashToYellow()
    {
        StartCoroutine(SignColorToYellow());
    }
    public void SignFlashToWhite()
    {
        StartCoroutine(SignColorToWhite());
    }
    public void HelpFlashToYellow()
    {
        StartCoroutine(HelpColorToYellow());
    }
    public void HelpFlashToWhite()
    {
        StartCoroutine(HelpColorToWhite());
    }
    
    
    
    private IEnumerator MusicColorToWhite()
    {
        Renderer renderer = vol.GetComponent<Renderer>();
        renderer.material.SetColor("_Color",Color.white);
        yield return null;
    }
    
    private IEnumerator MusicColorToYellow()
    {
        Renderer renderer = vol.GetComponent<Renderer>();
        renderer.material.SetColor("_Color",Color.yellow);
        yield return null;
    }
    
    private IEnumerator SignColorToWhite()
    {
        Renderer renderer = sign.GetComponent<Renderer>();
        renderer.material.SetColor("_Color",Color.white);
        yield return null;
    }
    
    private IEnumerator SignColorToYellow()
    {
        Renderer renderer = sign.GetComponent<Renderer>();
        renderer.material.SetColor("_Color",Color.yellow);
        yield return null;
    }
    
    
    
    private IEnumerator DoorColorToWhite()
    {
        Renderer renderer = door.GetComponent<Renderer>();
        renderer.material.SetColor("_Color",Color.white);
        yield return null;
    }
    
    private IEnumerator DoorColorToYellow()
    {
        Renderer renderer = door.GetComponent<Renderer>();
        renderer.material.SetColor("_Color",Color.yellow);
        yield return null;
    }
    
    private IEnumerator HelpColorToWhite()
    {
        Renderer renderer = help.GetComponent<Renderer>();
        renderer.material.SetColor("_Color",Color.white);
        yield return null;
    }
    
    private IEnumerator HelpColorToYellow()
    {
        Renderer renderer = help.GetComponent<Renderer>();
        renderer.material.SetColor("_Color",Color.yellow);
        yield return null;
    }
    
    
}
