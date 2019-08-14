using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    [SerializeField] float preaperingTime = 5f;
    [SerializeField] float fightTime = 10f;
    [SerializeField] float gameTime;
    [SerializeField] public int round = 0;
    [SerializeField] int preaperOrFight = 1; //1 = preaper, -1 = fight;
    [SerializeField] List<Shop> shops;
    [SerializeField] List<BotAI> bots;
    [SerializeField] List<PlayerPurse> purses;
    [SerializeField] Match match;
    [SerializeField] BattleSimulationController battle;
    void Start()
    {
        match = transform.GetComponent<Match>();
        battle = transform.GetComponent<BattleSimulationController>();
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
            shops.Add(transform.GetChild(i).GetChild(3).GetChild(1).GetChild(0).GetComponent<Shop>());
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
        battle.ResetOpponent(); //karşına gelen rakip bilgileri temizler;
        GenerateIncome();
        GenerateExperience();
        ManageShops();
        StartCoroutine(DecreaseTime(time));
        //unlockMoveBoard();
        ++round;
        
    }
    void FightRound(float time)
    {
        battle.SetOpponent(); //karşına rakip atar;
        StartCoroutine(DecreaseTime(time));

        //lockMoveBoard();

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

    private void GenerateIncome()
    {
        for (int i = 0; i < purses.Count; i++)
        {
            purses[i].CalculateIncome(round, true); // bool parametresi matchupların sonucuna göre gönderilecek.
        }
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

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 40;
        if (preaperOrFight == 1)
            GUI.Label(new Rect(Screen.width - 300, Screen.height - Screen.height + 20, Screen.width, Screen.height), "Round  " + round + "\nPreapering: " + gameTime);
        else
            GUI.Label(new Rect(Screen.width - 300, Screen.height - Screen.height + 20, Screen.width, Screen.height), "Round  " + round + "\nFighting: " + gameTime);
    }

}