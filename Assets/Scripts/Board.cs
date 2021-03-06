﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject chessboard;
    public GameObject bench;
    //public GameObject[] chessboardPosition;
    //public GameObject[] benchPosition; 

    public List<GameObject> chessboardPosition;
    public List<GameObject> benchPosition;

    public int chessboardSize = 64;
    public int benchSize = 8;
    public int playerUnitCount = 0;
    public int enemyUnitCount = 0;
    public int playerBattleUnitCount = 0;
    public int enemyBattleUnitCount = 0;
    //public GameObject[] playerBenchList;
    //public GameObject[] playerBoardList;
    //public GameObject[] enemyBenchList;
    //public GameObject[] enemyBoardList;

    public List<GameObject> playerBenchList;
    public List<GameObject> playerBoardList;
    public List<GameObject> enemyBenchList;
    public List<GameObject> enemyBoardList;

    private void Start()
    {
        GetAllBoardPiece();
    }

    void GetAllBoardPiece()
    {
        chessboard = transform.GetChild(0).gameObject;
        bench = transform.GetChild(1).gameObject;
        

        for (int i = 0; i < chessboardSize; i++)
        {
            chessboardPosition.Add(chessboard.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i< benchSize; i++)
        {
            benchPosition.Add(bench.transform.GetChild(i).gameObject);
        }
    }

    public void CountPlayerUnits()
    {
        int tempUnitCount = 0;
        for (int i = 0; i < 32; i++)
        {
            if(chessboardPosition[i].transform.childCount > 0)
            {
                if (playerBoardList.Contains(chessboardPosition[i].transform.GetChild(0).gameObject))
                {
                    tempUnitCount++;
                }
            }
        }
        playerUnitCount = tempUnitCount;
    }
}
