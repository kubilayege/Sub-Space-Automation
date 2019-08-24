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
        shopPanel = transform.parent.GetChild(1).GetChild(3).GetChild(1).GetChild(0).gameObject;   //todo for every board
        shopPanel.SetActive(!shopPanel.activeInHierarchy);
       // Debug.Log(shopPanel.name + "" );
    }
    public void Reroll()
    {
        shopPanel = transform.parent.GetChild(1).GetChild(3).GetChild(1).GetChild(0).gameObject;
        shopPanel.SetActive(true);
        shopPanel.GetComponent<Shop>().RerollShopWithButton();

    }
}
