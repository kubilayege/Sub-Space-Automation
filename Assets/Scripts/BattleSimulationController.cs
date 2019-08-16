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
            for (int i = 7; i >= 0; i--)
            {
                secondBoard.RemoveAt(i);
            }
        }
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                if(firstBoard[i].transform.GetChild(0).GetChild(63 - j).childCount > 0)
                Destroy(firstBoard[i].transform.GetChild(0).GetChild(63 - j).GetChild(0).gameObject);
            }
            
        }
    }

    void SetUnitForFight()
    {
        GameObject boardPlace;
        GameObject unit;
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 32; j++)
            {
                boardPlace = firstBoard[i].transform.GetChild(0).GetChild(63 - j).gameObject;
                
                if (secondBoard[i].transform.GetChild(0).GetChild(j).childCount > 0)
                {
                    unit = secondBoard[i].transform.GetChild(0).GetChild(j).GetChild(0).gameObject;
                    Vector3 newPos = new Vector3(boardPlace.transform.position.x, unit.transform.position.y , boardPlace.transform.position.z);
                    GameObject newObject =  Instantiate(unit, newPos, Quaternion.identity, firstBoard[i].transform.GetChild(0).GetChild(63 - j).transform ) as GameObject;
                }
            }
        }
        
    }
}
