using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurse : MonoBehaviour
{
    public int gold = 0;
    public int victoryCount;
    public int loseCount;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModifyGold(int value)
    {
        gold += value;
        Debug.Log("Bot -> " + transform.parent.GetSiblingIndex() + "  " + gold);
    }

    public void CalculateIncome(int round, bool victory)
    {
        if (victory)
        {
            victoryCount++;
            loseCount = 0;
        }
        else
        {
            loseCount++;
            victoryCount = 0;
        }
        int income = Mathf.Min(round, 5);
        int interest = Mathf.FloorToInt(gold / 10);
        int streakBonus = Mathf.FloorToInt(victoryCount / 3) + Mathf.FloorToInt(loseCount / 3);
        int victoryBonus = (victory) ? 1 : 0;

        ModifyGold(income + interest + streakBonus + victoryBonus);
    }
}
