using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> items;

    void Update()
    {
        if (items.Count > 0)
        {
            foreach (GameObject item in items)
            {
                if (item.GetComponent<Item>().isPurchased)
                {
                    Destroy(item);
                    items.Remove(item);
                    break;
                }
                
            }
        }
    }
    
    
}
