using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAI : MonoBehaviour
{
    Shop shop;
    Board board;
    //List<GameObject> botBench;
    //List<GameObject> botBoard;


    void Start()
    {
        InitializeVariables();
    }

    void InitializeVariables()
    {
        shop = transform.GetChild(1).GetComponent<Shop>();
        board = transform.parent.GetComponent<Board>();
        //botBench = board.playerBenchList;
        //botBench = board.playerBoardList;
    }
    

    public int CalculateimimumPieceCost()
    {
        int minimumPieceCost = 1;
        foreach (GameObject shopPiece in shop.tempShopPieces)
        {
            minimumPieceCost = Mathf.Min(shopPiece.GetComponent<Pieces>().pieceCost, minimumPieceCost);
        }
        return minimumPieceCost;
    }
    public void BotDecideUnitToBuy()
    {
        int j=0;
        while (CalculateimimumPieceCost() <= this.GetComponent<PlayerPurse>().gold)
        {
            if (j >= shop.tempShopPieces.Count) break;
            int index = j % shop.tempShopPieces.Count;
            //Debug.Log("Round " + board.transform.parent.GetComponent<EventTest>().round+ ":" + j + "  " + shop.tempShopPieces.Count);
            if (board.playerBenchList.Contains(shop.tempShopPieces[index]) || board.playerBoardList.Contains(shop.tempShopPieces[index]))
            {
                shop.BotBuyUnit(board, this.GetComponent<PlayerPurse>(), shop.tempShopPieces[index].transform.GetChild(0).gameObject);
            }else
            {
                shop.BotBuyUnit(board, this.GetComponent<PlayerPurse>(), shop.tempShopPieces[index].transform.GetChild(0).gameObject);
                //Debug.Log(this.gameObject.name);
            }
            int emptyIndex = 0;
            for (int i = UnityEngine.Random.Range(0,31); i<board.playerBoardList.Count; i++)
            {
                if (board.playerBoardList[i] == null)
                {
                    emptyIndex = i;
                    break;
                }
                if (board.playerBoardList[i] != null && i == 31)
                {
                    i = 0;
                }
            }
            for(int i=0; i <8; i++)
            {
                if (board.playerUnitCount <= this.GetComponent<Player>().level && board.playerBenchList[i] != null)
                {
                    MoveUnit(board.playerBenchList[i].transform.GetChild(0).gameObject, board.chessboardPosition[emptyIndex]);
                }
            }
            j++;
        }

    }

    public void MoveUnit(GameObject selectedUnit, GameObject moveUnitTo)
    {

        if(RelocateUnitsPositionOnLists(selectedUnit.transform.parent.gameObject, moveUnitTo))
        {
            selectedUnit.transform.parent.position = placableBoardPosition(selectedUnit, moveUnitTo);


            board.CountPlayerUnits();
        }
        
    }

    public Vector3 placableBoardPosition(GameObject selectedUnit, GameObject candidatePlace) //
    {
        Vector3 newPos = selectedUnit.transform.position;

        if (candidatePlace != null)
        {
            for (int i = 0; i < board.chessboardSize / 2; i++)
            {
                if (candidatePlace.transform.gameObject.name == board.chessboardPosition[i].gameObject.name)
                {
                    newPos = new Vector3(candidatePlace.transform.position.x, (selectedUnit.transform.localScale.y / 2) + 3, candidatePlace.transform.position.z);
                    return newPos;
                }
            }
            for (int i = 0; i < board.benchSize; i++)
            {
                if (candidatePlace.transform.gameObject.name == board.benchPosition[i].gameObject.name)
                {
                    newPos = new Vector3(candidatePlace.transform.position.x, (selectedUnit.transform.localScale.y / 2) + 3, candidatePlace.transform.position.z);
                    return newPos;
                }
            }
        }
        return newPos;
    }

    public bool RelocateUnitsPositionOnLists(GameObject selectedUnit, GameObject targetUnit)
    {
        GameObject tempSwapVar;
        int indexOf_SelectedUnit = 0;
        int indexOf_TargetUnit = 0;

        indexOf_SelectedUnit = board.playerBenchList.IndexOf(selectedUnit);
        if (board.playerUnitCount < this.GetComponent<Player>().level && targetUnit.CompareTag("BoardBlock"))
        {
            indexOf_TargetUnit = board.chessboardPosition.IndexOf(targetUnit);
            if (indexOf_TargetUnit <= 32)
            {
                //Debug.Log(indexOf_SelectedUnit + "     " + indexOf_TargetUnit);
                tempSwapVar = board.playerBenchList[indexOf_SelectedUnit];

                board.playerBoardList[indexOf_TargetUnit] = board.playerBenchList[indexOf_SelectedUnit];
                board.playerBoardList[indexOf_TargetUnit].transform.parent = board.chessboardPosition[indexOf_TargetUnit].transform;
                board.playerBenchList[indexOf_SelectedUnit] = null;
                return true;
            }
            //Destroy(tempSwapVar);
        }
        return false;
    }
}
