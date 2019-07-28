using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public GameObject shopPanel;

    public void Start()
    {
        
    }
    

    public void ToggleShop()
    {
        shopPanel = transform.GetChild(1).GetChild(0).gameObject;
        shopPanel.SetActive(!shopPanel.activeInHierarchy);
        Debug.Log(shopPanel.name + "" );
    }
}
