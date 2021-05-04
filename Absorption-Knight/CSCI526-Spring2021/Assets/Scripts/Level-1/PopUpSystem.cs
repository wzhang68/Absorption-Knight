using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public Text popUpText;
    

    #region Unity Methods

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Public Methods

    public void Popup(string text)
    {
        popUpBox.SetActive(true);
        popUpText.text = text;
        animator.SetTrigger("pop");
    }

    public void Close()
    {
        animator.SetTrigger("close");
        // popUpBox.SetActive(false);
    }

    #endregion



    #region Private Methods

    

    #endregion
    
}
