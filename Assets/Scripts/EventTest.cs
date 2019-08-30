using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    [SerializeField] float preaperingTime = 5f;
    public float fightTime = 20f;
    public float gameTime;
    [SerializeField] public int round = 0;
    public int preaperOrFight = 1; //1 = preaper, -1 = fight;
    [SerializeField] List<Shop> shops;
    [SerializeField] List<BotAI> bots;
    [SerializeField] List<PlayerPurse> purses;
    [SerializeField] Match match;
    [SerializeField] MatchupController matchups;
    void Start()
    {
        match = transform.GetComponent<Match>();
        matchups = transform.GetComponent<MatchupController>();
        GetPurses();
        GetShops();
        GetBots();
          //todo shops<>
        PreaperingRound(preaperingTime);
        
    }

    void GetBots()
    {
        for (int i = 2; i < 9; i++)
        {
            bots.Add(transform.GetChild(i).GetChild(3).GetComponent<BotAI>());
        }

        //Debug.Log(bots.Count);
    }

    void GetShops()
    {
        for (int i = 1; i < 9; i++)
        {
            shops.Add(transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Shop>());
        }
       
    }

    void GetPurses()
    {
        for (int i=1; i < 9; i++)
        {
            purses.Add(transform.GetChild(i).GetChild(3).GetComponent<PlayerPurse>());
        }
    }



    void PreaperingRound(float time)
    {
        if (round > 0)
        {
            StopFights();
            matchups.Results();
            matchups.ResetOpponent(); //karşına gelen rakip bilgileri temizler;
            ResetBoards();
        }
        GenerateExperience();
        ManageShops();
        StartCoroutine(DecreaseTime(time));
        //unlockMoveBoard();
        ++round;
    }
    void FightRound(float time)
    {
        matchups.SetOpponent(); //karşına rakip atar;
        StartCoroutine(DecreaseTime(time));
        StartFighting();
        //lockMoveBoard();

    }
    private void StartFighting()
    {
        for (int i = 0; i < matchups.firstBoard.Count; i++)
        {
            //if (match.boards[i].GetComponent<Board>().playerUnitCount == 0 || match.boards[i].GetComponent<Board>().enemyUnitCount == 0)
            //    continue;
            foreach (var unit in matchups.firstBoard[i].GetComponent<Board>().playerBoardList)
            {
                if (unit != null)
                {
                    StartCoroutine(unit.GetComponent<PieceAI>().Fight());
                }
            }

            foreach (var unit in matchups.firstBoard[i].GetComponent<Board>().enemyBoardList)
            {
                if (unit != null)
                {
                    StartCoroutine(unit.GetComponent<PieceAI>().Fight());
                }
                
            }
        }
        
    }

    void StopFights()
    {
        for (int i = 0; i < matchups.firstBoard.Count; i++)
        {
            //if (match.boards[i].GetComponent<Board>().playerUnitCount == 0 || match.boards[i].GetComponent<Board>().enemyUnitCount == 0)
            //    continue;
            foreach (var unit in matchups.firstBoard[i].GetComponent<Board>().playerBoardList)
            {
                if (unit != null)
                {
                    if (!unit.transform.parent.CompareTag("Graveyard"))
                    {
                        StopCoroutine(unit.GetComponent<PieceAI>().Fight());
                    }
                }
                 
            }

            foreach (var unit in matchups.firstBoard[i].GetComponent<Board>().enemyBoardList)
            {
                if (unit != null)
                {
                    if (!unit.transform.parent.CompareTag("Graveyard"))
                    {
                        StopCoroutine(unit.GetComponent<PieceAI>().Fight());
                    }
                }
            }
        }
    }

    private void ResetBoards()
    {
        int index = 0;
        for (int i = 0; i < matchups.firstBoard.Count; i++)
        {
            //foreach (var item in match.boards[i].GetComponent<Board>().chessboardPosition)
            //{
            //    if (item.transform.childCount != 0)
            //    {
            //        index = match.boards[i].GetComponent<Board>().playerBoardList.IndexOf(item.transform.GetChild(0).gameObject);
            //        if (index != -1)
            //        {
            //            item.transform.GetChild(0).transform.parent = match.boards[i].GetComponent<Board>().chessboardPosition[index].transform;
            //            match.boards[i].GetComponent<Board>().playerBoardList[index].transform.position = new Vector3(match.boards[i].GetComponent<Board>().playerBoardList[index].transform.parent.position.x,
            //                                                                                                          match.boards[i].GetComponent<Board>().playerBoardList[index].transform.position.y,
            //                                                                                                          match.boards[i].GetComponent<Board>().playerBoardList[index].transform.parent.position.z);
            //        }
            //    }
            ////}
            //if (match.boards[i].GetComponent<Board>().playerUnitCount == 0)
            //    continue;
            foreach (var item in matchups.firstBoard[i].GetComponent<Board>().playerBoardList)
            {
                if (item != null)
                {
                    index = matchups.firstBoard[i].GetComponent<Board>().playerBoardList.IndexOf(item.gameObject);
                    if (index != -1)
                    {
                        item.transform.parent = matchups.firstBoard[i].GetComponent<Board>().chessboardPosition[index].transform;
                        //match.boards[i].GetComponent<Board>().playerBoardList[index].transform.position = new Vector3(match.boards[i].GetComponent<Board>().playerBoardList[index].transform.parent.position.x,
                        //                                                                                              match.boards[i].GetComponent<Board>().playerBoardList[index].transform.position.y,
                        //                                                                                              match.boards[i].GetComponent<Board>().playerBoardList[index].transform.parent.position.z);
                        item.transform.localPosition = new Vector3(0, 64, 0);
                        item.transform.rotation = Quaternion.identity;
                    }
                }
            }

        }
    }

    private void GenerateExperience()
    {
        for (int i = 0; i < 8; i++)
        {
            purses[i].GetComponent<Player>().IncreaseExp();
        }
    }

    private void ManageShops()
    {
        for (int i = 0; i < 8; i++)
        {
            shops[i].ClearSlot();
            shops[i].DrawPieces();
        }
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].BotDecideUnitToBuy();
        }
    }

    public void GenerateIncome(PlayerPurse purse, bool victory)
    {
        purse.CalculateIncome(round, victory); // bool parametresi matchupların sonucuna göre gönderilecek.     
    }
   
    IEnumerator DecreaseTime(float time)
    {
        gameTime = time;
        while (gameTime > 0)
        {
            gameTime -= 1;
            yield return new WaitForSeconds(1);
        }

        preaperOrFight *= -1;

        if (preaperOrFight == 1)
            PreaperingRound(preaperingTime);

        else
            FightRound(fightTime);
    }

    //private void OnGUI()
    //{
    //    GUI.skin.label.fontSize = 40;
    //    if (preaperOrFight == 1)
    //        GUI.Label(new Rect(Screen.width - 300, Screen.height - Screen.height + 20, Screen.width, Screen.height), "Round  " + round + "\nPreapering: " + gameTime);
    //    else
    //        GUI.Label(new Rect(Screen.width - 300, Screen.height - Screen.height + 20, Screen.width, Screen.height), "Round  " + round + "\nFighting: " + gameTime);
    //}

}