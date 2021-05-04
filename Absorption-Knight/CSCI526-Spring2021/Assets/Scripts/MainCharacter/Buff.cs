using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buff : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string buffName;
    private GameObject buffpanel;
    private string buffDetail;


    private void Awake()
    {
        buffpanel= GameObject.Find("Canvas").transform.GetChild(11).gameObject.transform.GetChild(1).gameObject;
    }

    public void SetDetails()
    {
        buffpanel.transform.Find("BuffName").gameObject.GetComponent<Text>().text = buffName;
        buffpanel.transform.Find("BuffDetail").gameObject.GetComponent<Text>().text = BuffContentSelector(buffName);
        buffpanel.SetActive(true);
    }

    public void ResetDetails()
    {
        buffpanel.transform.Find("BuffName").gameObject.GetComponent<Text>().text = String.Empty;
        buffpanel.transform.Find("BuffDetail").gameObject.GetComponent<Text>().text = String.Empty;
        buffpanel.SetActive(false);
    }
    
    private string BuffContentSelector(string buffname)
    {
        string temp = String.Empty;
        switch (buffname)
        {
            case "AddMaxHP":
                temp = "Increases Teodoro's Max HP by 20%.";
                break;
            case "SanityUp":
                temp = "Increases Teodoro's Max Sanity by 20%.";
                break;
            case "SpeedUp":
                temp = "Dramatically increases Teodoro's moving speed.";
                break;
            case "JumpForceUp":
                temp = "Dramatically increases Teodoro's jump force.";
                break;
            case "Healing":
                temp = "Slowly increases Teodoro's HP.";
                break;
            case "TripleATK":
                temp = "Teodoro can attack enemy three times instantly.";
                break;
            case "LifeSteal":
                temp = "Teodoro can recover 10% of his damage to enemy.";
                break;
            case "DEFUp":
                temp = "Teodoro will get less damage taken.";
                break;
            case "ATKUp":
                temp = "Increase Teodoro's Basic Attack Damage.";
                break;

        }

        return temp;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        SetDetails();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetDetails();
    }
}
