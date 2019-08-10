using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAI : MonoBehaviour
{
    Shop shop;

    void Start()
    {
        InitializeVariables();
    }

    void InitializeVariables()
    {
        shop = transform.GetChild(1).GetChild(0).GetComponent<Shop>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
