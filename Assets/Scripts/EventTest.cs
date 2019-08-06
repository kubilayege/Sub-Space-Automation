using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    [SerializeField] float preaperingTime = 5f;
    [SerializeField] float fightTime = 10f;
    [SerializeField] float gameTime;
    [SerializeField] int round = 0;
    [SerializeField] int preaperOrFight = 1; //1 = preaper, -1 = fight;
    Shop shop;
    [SerializeField] List<PlayerPurse> purses;
    void Start()
    {
        GetPurses();
        shop = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Shop>();
        PreaperingRound(preaperingTime);
        
    }

    private void GetPurses()
    {
        for (int i=1; i < 9; i++)
        {
            purses.Add(transform.GetChild(i).GetChild(3).GetComponent<PlayerPurse>());
        }
    }



    void PreaperingRound(float time)
    {
        GenerateIncome();
        StartCoroutine(DecreaseTime(time));
        //unlockMoveBoard();
        ++round;
        shop.ClearSlot();
        shop.DrawPieces();
        
    }

    private void GenerateIncome()
    {
        for (int i = 0; i < purses.Count; i++)
        {
            purses[i].CalculateIncome(round, true); // bool parametresi matchupların sonucuna göre gönderilecek.
        }
    }

    void FightRound(float time)
    {
        StartCoroutine(DecreaseTime(time));
        
        //lockMoveBoard();

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