using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSimulationController : MonoBehaviour
{
    public List<GameObject> firstBoard;
    public List<GameObject> secondBoard;
    private void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            firstBoard.Add(transform.GetChild(i+1).gameObject);
            firstBoard[i] = transform.GetChild(i + 1).gameObject.GetComponent<Board>().gameObject;
        }
    }
    private void FixedUpdate()
    {
        if(firstBoard[0].transform.GetChild(0).GetChild(0).GetChild(0).gameObject!=null)
        Debug.Log(firstBoard[0].transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
    }
    public void SetOpponent()
    {
        Debug.Log("i called from eventManager");
        int randomNum = Random.Range(1, 7);
        GameObject first;
        for (int i = 0; i < 8; i++)
        {
            first = firstBoard[(randomNum + i) % 8 ];
            secondBoard.Add(first);
        }
        SetUnitForFight();
    }

    public void ResetOpponent()
    {
        if (secondBoard.Count > 0)
        {
            for (int i = 7; i >= 0; i--) //listeden eleman atılınca listede ki indexler kaydığı için bunu yaptım.
            {
                secondBoard.RemoveAt(i);
            }
        }
    }

    //burası rakipleri bizim boarda getirecek kısım çözemedim bir türlü.
    void SetUnitForFight()
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 32; j++)
            {
                //if (secondBoard[i].transform.GetChild(0).GetChild(j).GetChild(0).gameObject != null)
                //{
                //    firstBoard[i].transform.GetChild(0).GetChild(63 - j).GetChild(0).gameObject = secondBoard[i].transform.GetChild(0).GetChild(j).GetChild(0).gameObject;
                //}
            }
        }
        
    }
}
