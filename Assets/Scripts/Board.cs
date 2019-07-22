using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject chessboard;
    public GameObject bench;
    public GameObject[] chessboardPieces;
    public GameObject[] benchPieces;
    public int chessboardSize = 64;
    public int benchSize = 8;

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
            chessboardPieces[i] = chessboard.transform.GetChild(i).gameObject;
        }
        for(int i = 0; i< benchSize; i++)
        {
             benchPieces[i] = bench.transform.GetChild(i).gameObject;
        }
    }
}
