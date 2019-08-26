using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchupController : MonoBehaviour
{
    public List<Board> firstBoard;
    public List<Board> secondBoard;
    EventTest events;

    private void Start()
    {
        events = GetComponent<EventTest>();
        
        for (int i = 0; i < 8; i++)
        {
            if (transform.GetChild(i + 1).GetChild(3) != null)
            {
                firstBoard.Add(transform.GetChild(i + 1).GetComponent<Board>());
            }
        }
    }

    public void SetOpponent()
    {
       // Debug.Log("i called from eventManager");
        int randomNum = Random.Range(1, 7);
        Board first;
        for (int i = 0; i < firstBoard.Count; i++)
        {
            first = firstBoard[(randomNum + i) % firstBoard.Count];
            secondBoard.Add(first);
        }
        SetUnitForFight();
    }

    public void ResetOpponent()
    {
        if (secondBoard.Count > 0)
        {
            for (int i = secondBoard.Count - 1; i >= 0; i--)
            {
                secondBoard.RemoveAt(i);
            }
        }
        for (int i = 0; i < firstBoard.Count; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                if (firstBoard[i].enemyBoardList[j] != null)
                    Destroy(firstBoard[i].enemyBoardList[j]);
            }

        }
    }

    void SetUnitForFight()
    {
        GameObject boardPlace;
        GameObject unit;
        for (int i = 0; i < firstBoard.Count; i++)
        {
            firstBoard[i].enemyUnitCount = 0;
            for (int j = 0; j < 32; j++)
            {
                boardPlace = firstBoard[i].transform.GetChild(0).GetChild(63 - j).gameObject;

                if (secondBoard[i].transform.GetChild(0).GetChild(j).childCount > 0)
                {
                    unit = secondBoard[i].transform.GetChild(0).GetChild(j).GetChild(0).gameObject;
                    Vector3 newPos = new Vector3(boardPlace.transform.position.x, unit.transform.position.y, boardPlace.transform.position.z);
                    GameObject newObject = Instantiate(unit, newPos, Quaternion.Inverse(Quaternion.identity), firstBoard[i].transform.GetChild(0).GetChild(63 - j).transform) as GameObject;
                    newObject.transform.forward = -newObject.transform.forward;
                    firstBoard[i].enemyBoardList[j] = newObject;
                    firstBoard[i].enemyUnitCount += 1;
                }
            }
        }

    }

    public void Results()
    {
        foreach (var board in firstBoard)
        {
            events.GenerateIncome(board.transform.GetChild(3).GetComponent<PlayerPurse>(), true); // bu silinecek

            //if (board.enemyUnitCount == 0)
            //{
            //    events.GenerateIncome(board.transform.GetChild(3).GetComponent<PlayerPurse>(), true);
            //}
            //else if(board.playerUnitCount == 0)
            //{
            //    events.GenerateIncome(board.transform.GetChild(3).GetComponent<PlayerPurse>(), false);
            //}
        }
    }
}
