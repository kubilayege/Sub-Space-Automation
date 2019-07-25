using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<GameObject> piecesOnPool;
    public List<Transform> pieceSlotsList;
    public List<GameObject> tempShopPieces;

    void Start()
    {
        InitializeVariables();
        DrawPieces();
    }
    
    void Update()
    {
       
    }

    void InitializeVariables()
    {
        GameObject pieceSlots = transform.GetChild(0).gameObject;
        for(int i = 0; i< pieceSlots.transform.childCount ; i++)
        {
            pieceSlotsList.Add(pieceSlots.transform.GetChild(i));
        }
    }

    void DrawPieces()
    {
        for(int i=0; i < 5; i++)
        {
            int maxIndex = piecesOnPool.Count - 1;
            int randomIndex = Random.Range(0, maxIndex);
            Debug.Log(piecesOnPool.Count);
            tempShopPieces.Add(Instantiate(piecesOnPool[randomIndex], pieceSlotsList[i].transform.position, Quaternion.identity, pieceSlotsList[i].transform));
            
            piecesOnPool.RemoveAt(randomIndex);
        }
    }
}
