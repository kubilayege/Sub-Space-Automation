using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAI : MonoBehaviour
{
    public GameObject target;
    public Board board;
    public Match match;

    public List<GameObject> adjacentBlocksOfThisUnit;
    public List<GameObject> adjacentBlocksOfTarget;
    public List<GameObject> emptyBoardPos;


    public IEnumerator Fight()
    {
        InitializePiece();
        if (board.playerUnitCount == 0 || board.enemyUnitCount == 0)
        {
            yield return new WaitForSeconds(match.GetComponent<EventTest>().fightTime);
        }
        while (match.GetComponent<EventTest>().preaperOrFight == -1)
        {
            if (match.GetComponent<MatchupController>().secondBoard[match.GetComponent<MatchupController>().firstBoard.IndexOf(board)].playerUnitCount > 0)
            {
                if (CheckIfThereIsAdjacentToThisUnit())
                {
                    //Attack();
                    yield return new WaitForSeconds(1f);// based on attackspeedof piece
                }
                else
                {
                    FindTarget();
                    if (target != null)
                        MovePieceToTarget();
                    yield return new WaitForSeconds(2.5f); // move speed of unit
                }
            }
        }

    }

    private void MovePieceToTarget()
    {
        float minDistance = float.MaxValue;
        GameObject targetPlace = this.transform.parent.gameObject;
        GameObject pathToTarget = this.transform.parent.gameObject;
        for (int j = 0; j < adjacentBlocksOfThisUnit.Count; j++)
        {
            float distanceOfBlocks = (target.transform.position - adjacentBlocksOfThisUnit[j].transform.position).magnitude;
            if (distanceOfBlocks < minDistance)
            {
                pathToTarget = adjacentBlocksOfThisUnit[j];
                minDistance = distanceOfBlocks;
            }
        }
        for (int i = 0; i < adjacentBlocksOfTarget.Count; i++)
        {
            float distanceOfBlocks = (pathToTarget.transform.position - adjacentBlocksOfTarget[i].transform.position).magnitude;
            if (distanceOfBlocks < minDistance)
            {
                targetPlace = adjacentBlocksOfTarget[i];
                minDistance = distanceOfBlocks;
            }
        }

        minDistance = float.MaxValue;


        if (pathToTarget != this.transform.parent.gameObject && pathToTarget.transform.childCount == 0)
        {
            this.transform.position = new Vector3(pathToTarget.transform.position.x, this.transform.position.y, pathToTarget.transform.position.z);
            this.transform.parent = pathToTarget.transform;
            FindAdjacentBlocks(transform.parent.gameObject, adjacentBlocksOfThisUnit);
        }
    }

    private bool CheckIfThereIsAdjacentToThisUnit()
    {
        int indexOfUnit = board.chessboardPosition.IndexOf(transform.parent.gameObject);
        if (this.gameObject == board.chessboardPosition[indexOfUnit].transform.GetChild(0).gameObject)
            FindAdjacentBlocks(board.chessboardPosition[indexOfUnit], adjacentBlocksOfThisUnit);
        else
            return false;

        if (target != null)
        {
            foreach (var boardPlace in adjacentBlocksOfThisUnit)
            {
                if (boardPlace.transform.childCount != 0)
                {
                    if (board.enemyBoardList.Contains(this.gameObject))
                    {
                        if (target == boardPlace.transform.GetChild(0).gameObject)
                        {

                            return true;
                        }
                    }
                    else if (board.playerBoardList.Contains(this.gameObject))
                    {
                        if (target == boardPlace.transform.GetChild(0).gameObject)
                        {

                            return true;
                        }
                    }
                }
            }
        }

        //foreach (var boardPlace in adjacentBlocksOfThisUnit)
        //{
        //    if(boardPlace.transform.childCount != 0)
        //    {
        //        if (board.enemyBoardList.Contains(this.gameObject))
        //        {
        //            if (board.playerBoardList.Contains(boardPlace.transform.GetChild(0).gameObject))
        //            {
        //                target = boardPlace.transform.GetChild(0).gameObject;
        //                return true;
        //            }
        //        }
        //        else if (board.playerBoardList.Contains(this.gameObject))
        //        {
        //            if (board.enemyBoardList.Contains(boardPlace.transform.GetChild(0).gameObject))
        //            {
        //                target = boardPlace.transform.GetChild(0).gameObject;
        //                return true;
        //            }
        //        }
        //    }
        //}

        return false;
    }

    private void InitializePiece()
    {
        board = transform.parent.parent.parent.GetComponent<Board>();
        match = transform.parent.parent.parent.parent.GetComponent<Match>();
        adjacentBlocksOfTarget.Clear();
        adjacentBlocksOfThisUnit.Clear();
    }

    private void FindTarget()
    {

        if (board.enemyBoardList.Contains(this.gameObject))
        {
            float distance = float.MaxValue;
            foreach (var pos in board.chessboardPosition)
            {

                if (pos.transform.childCount > 0 && board.playerBoardList.Contains(pos.transform.GetChild(0).gameObject) && distance > (pos.transform.position - this.transform.position).magnitude)
                {

                    if (FindAdjacentBlocks(pos.gameObject, adjacentBlocksOfTarget))  //Checks if potential target has free spots near him.
                    {
                        distance = (pos.transform.position - this.transform.position).magnitude;
                        target = pos.transform.GetChild(0).gameObject;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        else if (board.playerBoardList.Contains(this.gameObject))
        {
            float distance = float.MaxValue;
            foreach (var pos in board.chessboardPosition)
            {
                if (pos.transform.childCount > 0 && board.enemyBoardList.Contains(pos.transform.GetChild(0).gameObject) && distance > (pos.transform.position - this.transform.position).sqrMagnitude)
                {
                    if (FindAdjacentBlocks(pos, adjacentBlocksOfTarget))  //Checks if potential target has free spots near him.
                    {
                        distance = (pos.transform.position - this.transform.position).sqrMagnitude;
                        target = pos.transform.GetChild(0).gameObject;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

    }

    private bool FindAdjacentBlocks(GameObject boardPos, List<GameObject> listOfPlaces)
    {
        int indexOfUnit = board.chessboardPosition.IndexOf(boardPos);
        if (indexOfUnit == -1)
        {

            return false;
        }
        int indexOfUnitMod = indexOfUnit % 8;

        if (indexOfUnit == 0)
        {
            FindAdjacentBlocksOf(indexOfUnit, 0, 0, 1, 1, listOfPlaces);
        }
        else if (indexOfUnit == 7)
        {
            FindAdjacentBlocksOf(indexOfUnit, -1, 0, 0, 1, listOfPlaces);
        }
        else if (indexOfUnit == 56)
        {
            FindAdjacentBlocksOf(indexOfUnit, 0, -1, 1, 0, listOfPlaces);
        }
        else if (indexOfUnit == 63)
        {
            FindAdjacentBlocksOf(indexOfUnit, -1, -1, 0, 0, listOfPlaces);
        }
        else if (indexOfUnitMod == 0)
        {
            FindAdjacentBlocksOf(indexOfUnit, 0, -1, 1, 1, listOfPlaces);
        }
        else if (indexOfUnitMod == 7)
        {
            FindAdjacentBlocksOf(indexOfUnit, -1, -1, 0, 1, listOfPlaces);
        }
        else if (indexOfUnit < 8)
        {
            FindAdjacentBlocksOf(indexOfUnit, -1, 0, 1, 1, listOfPlaces);
        }
        else if (indexOfUnit > 56)
        {
            FindAdjacentBlocksOf(indexOfUnit, -1, -1, 1, 0, listOfPlaces);
        }
        else
        {
            FindAdjacentBlocksOf(indexOfUnit, -1, -1, 1, 1, listOfPlaces);
        }

        if (CheckIfThereIsAvailableBlock(listOfPlaces))
        {
            return false;
        }

        return true;
    }

    private void FindAdjacentBlocksOf(int indexOfEnemy, int iStart, int jStart, int iEnd, int jEnd, List<GameObject> listOfPlaces)
    {
        listOfPlaces.Clear();
        for (int j = jStart; j <= jEnd; j++)
        {
            for (int i = iStart; i <= iEnd; i++)
            {
                //if( i == 0 && j == 0)
                //{
                //    continue;
                //}

                listOfPlaces.Add(board.chessboardPosition[indexOfEnemy + 8 * j + i]);
            }
        }

    }

    private bool CheckIfThereIsAvailableBlock(List<GameObject> listToCheck)
    {
        int freeSpots = listToCheck.Count;
        if (freeSpots == 0)
        {
            return true;
        }
        for (int i = freeSpots - 1; i > 0; i--)
        {
            if (listToCheck[i].transform.childCount != 0 && (this.gameObject != listToCheck[i].transform.GetChild(0).gameObject && (target != null && target != listToCheck[i].transform.GetChild(0).gameObject)))
            {
                freeSpots -= 1;
                listToCheck.RemoveAt(i); // remove occupied boardplace from adjacentplace lists.
            }
            else
            {
                freeSpots += 1;
                continue;
            }
        }
        if (freeSpots == 0)
        {
            return true;
        }
        return false;
    }
}
