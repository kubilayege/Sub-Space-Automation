using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    public GameObject boardPrefab;
    public GameObject botBoardPrefab;
    public List<GameObject> players;
    public GameObject[] boards;
    public List<GameObject> piecePool;
    public List<GameObject> gamePieces;
    void Awake()
    {
        InitializeBoardSpawnPositions();
        InitializePiecePool();
        GetPlayers();
    }

    public void GetPlayers()
    {
        for (int i = 1; i < 8; i++)
        {
            players.Add(transform.GetChild(i).GetChild(3).gameObject);
        }
    }

    public void InitializePiecePool()
    {
        GameObject piecePoolObj = new GameObject();
        piecePoolObj.name = "PiecePoolObj";
        piecePoolObj.transform.parent = this.transform;
        for (int i = 0; i < 500; i++)
        {
            piecePool.Add(Instantiate(gamePieces[i % gamePieces.Count], new Vector3(6000 - i * 128, 0, 1500), Quaternion.identity, transform.GetChild(transform.childCount-1)));
        }

    }

    void InitializeBoardSpawnPositions()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i == 0)
            {
                boards[i] = GameObject.Instantiate(boardPrefab, new Vector3(i * 2048, 0, 0), Quaternion.identity, this.transform);
            }
            else
            {
                boards[i] = GameObject.Instantiate(botBoardPrefab, new Vector3(i * 2048, 0, 0), Quaternion.identity, this.transform);
                boards[i].name = "BotBoard " + i;
            }
        }

        //int k = 1;
        //for (int i = -1; i < 2; i++)
        //{
        //    for (int j = -1; j< 2; j++)
        //    {

        //        if (i == 0 && j == 0)
        //        {
        //            continue;
        //            boards[i] = GameObject.Instantiate(boardPrefab, new Vector3(i * 2048, 0, 0), Quaternion.identity, this.transform);
        //        }
        //        else if(i==-1 && j ==-1)
        //        {
        //            boards[k] = GameObject.Instantiate(boardPrefab, new Vector3(i * 2048, 1000, j*2048), Quaternion.identity, this.transform);

        //        }
        //        else
        //        {
        //            boards[k] = GameObject.Instantiate(botBoardPrefab, new Vector3(i * 2048, 1000, j * 2048), Quaternion.identity, this.transform);
        //            boards[k].name = "BotBoard " + k;
        //        }
        //        k++;
        //    }
        //}
    }
}
