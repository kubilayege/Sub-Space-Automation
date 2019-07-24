using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public GameObject shopPanel;

    public void Start()
    {
        shopPanel = transform.GetChild(1).transform.GetChild(1).gameObject;
    }


    public void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeInHierarchy);
    }
}
