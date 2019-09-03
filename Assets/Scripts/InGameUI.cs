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
    public GameObject sellButton;
    public Match match;
    public EventTest events;
    public List<Player> players;
    public List<GameObject> playerInfo;

    public GameObject unitToSell;
    public List<GameObject> listToUnitSell;
    public Board boardOfUnitSell;


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
            middleInfoPanel.text = "Gold: " + players[0].GetComponent<PlayerPurse>().gold +  "  Units: " + match.boards[0].GetComponent<Board>().playerUnitCount + "/" + players[0].level + "    Prepering: " + events.gameTime;
        }
        else
        {
            middleInfoPanel.text = "Gold: " + players[0].GetComponent<PlayerPurse>().gold + "  Units: " + match.boards[0].GetComponent<Board>().playerUnitCount + "/" + players[0].level + "    Fighting: " + events.gameTime;
        }
        
    }

    private void InitializeVariables()
    {
        match = transform.parent.GetComponent<Match>();
        events = match.GetComponent<EventTest>();
        roundInfo = transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>();
        middleInfoPanel = transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>();
        sellButton = transform.GetChild(1).GetChild(6).gameObject;
        shopPanel = transform.parent.GetChild(1).GetChild(3).GetChild(1).GetChild(0).gameObject;
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

    public void SellUnit()
    {
        if (unitToSell != null)
        {
            if(events.preaperOrFight == -1 && boardOfUnitSell.playerBoardList.Contains(unitToSell))
            {
                sellButton.SetActive(true);
                return;
            }
            int indexOfPieceType = -1;
            int starOfUnit = unitToSell.GetComponent<Pieces>().star;
            int costOfUnit = unitToSell.GetComponent<Pieces>().pieceCost;
            string nameOfUnit = unitToSell.GetComponent<Pieces>().pieceName;

            foreach (var unit in match.gamePieces)
            {
                if (unit.GetComponent<Pieces>().pieceName == nameOfUnit)
                    indexOfPieceType = match.gamePieces.IndexOf(unit);
            }

            int indexOfPiece = listToUnitSell.IndexOf(unitToSell);
            for (int i = 0; i < starOfUnit * 3; i++)
            {
                match.piecePool.Add(Instantiate(match.gamePieces[indexOfPieceType], new Vector3(6000 - i * 128, 0, 1500), Quaternion.identity, transform.parent.GetChild(transform.parent.childCount - 1).transform));

            }
            players[0].GetComponent<PlayerPurse>().ModifyGold(costOfUnit + (starOfUnit - 1) * 2);
            if (unitToSell.transform.parent.CompareTag("BoardBlock"))
            {
                Destroy(unitToSell);
                boardOfUnitSell.playerUnitCount--;
            }
            else
            {
                Destroy(unitToSell);
            }
            sellButton.SetActive(false);
        }

    }

    public void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeInHierarchy);
       // Debug.Log(shopPanel.name + "" );
    }
    public void Reroll()
    {
        shopPanel.SetActive(true);
        shopPanel.transform.parent.GetComponent<Shop>().RerollShopWithButton();
    }
}
