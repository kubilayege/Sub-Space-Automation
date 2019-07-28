using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject chessboard;
    public GameObject bench;
    public GameObject[] chessboardPosition;
    public GameObject[] benchPosition;
    public int chessboardSize = 64;
    public int benchSize = 8;
    public GameObject[] playerBenchList;
    public GameObject[] playerBoardList;
    public GameObject[] enemyBenchList;
    public GameObject[] enemyBoardList;
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
            chessboardPosition[i] = chessboard.transform.GetChild(i).gameObject;
        }
        for(int i = 0; i< benchSize; i++)
        {
            benchPosition[i] = bench.transform.GetChild(i).gameObject;
        }
    }
}
