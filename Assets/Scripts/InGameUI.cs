using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public Text roundInfo;
    public Text middleInfoPanel;
    public GameObject shopPanel;
    public Match match;
    public EventTest events;
    public List<Player> players;
    public List<GameObject> playerInfo;

    public void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        UpdateData();
    }

    public void UpdateData()
    {
        for (int i = 0; i < players.Count; i++)
        {
            playerInfo[i].transform.GetChild(0).GetComponent<Slider>().value = players[i].health;
            playerInfo[i].transform.GetChild(1).GetComponent<Slider>().value = players[i].GetComponent<PlayerPurse>().gold;
        }
        if(match.GetComponent<MatchupController>().secondBoard.Count != 0)
        {
            roundInfo.text = "Round " + events.round + ":\n" + "vs\n" + match.GetComponent<MatchupController>().secondBoard[0].name;
        }
        if(events.preaperOrFight == 1)
        {
            middleInfoPanel.text = "Units: " + match.boards[0].GetComponent<Board>().playerUnitCount + "/" + players[0].level + "    Prepering: " + events.gameTime;
        }
        else
        {
            middleInfoPanel.text = "Units: " + match.boards[0].GetComponent<Board>().playerUnitCount + "/" + players[0].level + "    Fighting: " + events.gameTime;
        }
        
    }


    private void InitializeVariables()
    {
        match = transform.parent.GetComponent<Match>();
        events = match.GetComponent<EventTest>();
        roundInfo = transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>();
        middleInfoPanel = transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>();
        GetPlayers();
        GetInfoPanels();
    }

    private void GetInfoPanels()
    {
        for (int i = 0; i < 8; i++)
        {
            playerInfo.Add(transform.GetChild(1).GetChild(2).GetChild(i).gameObject);
        }
    }

    private void GetPlayers()
    {
        for (int i = 0; i < 8; i++)
        {
            players.Add(match.boards[i].transform.GetChild(3).GetComponent<Player>());
        }
    }

    public void ToggleShop()
    {
        shopPanel = transform.parent.GetChild(1).GetChild(3).GetChild(1).GetChild(0).gameObject;   //todo for every board
        shopPanel.SetActive(!shopPanel.activeInHierarchy);
       // Debug.Log(shopPanel.name + "" );
    }
    public void Reroll()
    {
        shopPanel = transform.parent.GetChild(1).GetChild(3).GetChild(1).gameObject;
        shopPanel.SetActive(true);
        shopPanel.GetComponent<Shop>().RerollShopWithButton();

    }
}
