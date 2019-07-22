using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    public GameObject boardPrefab;
    public GameObject botBoardPrefab;

    public GameObject[] boards;

    void Awake()
    {
        InitializeBoardSpawnPositions();
    }

    void InitializeBoardSpawnPositions()
    {
        for(int i = 0; i <8; i++)
        {
            if(i == 0)
            {
                boards[i] = GameObject.Instantiate(boardPrefab, new Vector3(i * 2048, 0, 0), Quaternion.identity, this.transform);
            }
            else
            {
                boards[i] = GameObject.Instantiate(botBoardPrefab, new Vector3(i * 2048, 0, 0), Quaternion.identity, this.transform);
            }
        }
    }
}
