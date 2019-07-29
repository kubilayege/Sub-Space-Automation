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
    void Start()
    {
        PreaperingRound(preaperingTime);

        shop = transform.parent.GetChild(1).GetChild(0).GetComponent<Shop>();
    }

    void PreaperingRound(float time)
    {
        
        StartCoroutine(DecreaseTime(time));
        //unlockMoveBoard();
        ++round;
        shop.ClearSlot();
        shop.DrawPieces();
        
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