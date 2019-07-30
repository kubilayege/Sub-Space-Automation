using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<GameObject> gamePieces; // Oyundaki her piece burda tutulacak
    public List<Transform> pieceSlotsList; //Shop slot pozisyonları listesi
    public List<GameObject> tempShopPieces;
    public List<GameObject> piecePool;
    float maxRayDistance = 5000;
    int poolSize = 100;
    public Match match;
    public Board board;
    GameObject forAddUnitToPool;
    void Start()
    {
        InitializeVariables();
        //DrawPieces();
        match = transform.parent.parent.parent.GetComponent<Match>();
        board = match.boards[0].GetComponent<Board>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BuyedUnitMove();
        }
    }

    void InitializeVariables()
    {
        GameObject pieceSlots = transform.GetChild(0).gameObject;
        for(int i = 0; i< pieceSlots.transform.childCount ; i++)
        {
            pieceSlotsList.Add(pieceSlots.transform.GetChild(i));
        }

        for (int i = 0; i < poolSize; i++)
        {
            piecePool.Add(Instantiate(gamePieces[i%2], new Vector3(6000 - i * 128, 0, 1500), Quaternion.identity, this.transform));
        }

    }

    public void DrawPieces()
    {
        //GameObject pieceSlots = transform.GetChild(0).gameObject;
        for (int i=0; i < pieceSlotsList.Count; i++)
        {
            if(pieceSlotsList.Count != 0)
            {
                int maxIndex = piecePool.Count - 1;
                int randomIndex = Random.Range(0, maxIndex);
                // Debug.Log(piecesOnPool.Count);
                tempShopPieces.Add(piecePool[randomIndex]);
                tempShopPieces[tempShopPieces.Count - 1].transform.position = pieceSlotsList[i].transform.position;
                tempShopPieces[tempShopPieces.Count - 1].transform.parent = pieceSlotsList[i].transform;
                piecePool.RemoveAt(randomIndex);
            }
        }
    }
   public void ClearSlot()
    {
        for (int i = 0; i < 5; i++)
        {
            if(transform.GetChild(0).GetChild(i).childCount == 2)
            {
                forAddUnitToPool = transform.GetChild(0).GetChild(i).GetChild(1).gameObject;
                piecePool.Add(forAddUnitToPool);
                tempShopPieces.Remove(forAddUnitToPool); //  remove from shop slot
                forAddUnitToPool.transform.position = new Vector3(6000 - i * 128, 0, 1600);
                forAddUnitToPool.transform.parent = this.transform;
            }
        }
    }

    public GameObject SendRayToMousePosition()//mouse pozisyonundaki objeyi geri döndürür.
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance))
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }
    void BuyedUnitMove()
    {
        if(transform.gameObject.activeSelf)
        {

            GameObject tempSelectedUnit = SendRayToMousePosition();
            if (tempSelectedUnit.transform.parent.parent.CompareTag("Slot") && tempSelectedUnit.CompareTag("Unit"))
            {
                for (int i = 0; i < board.benchSize; i++)
                {
                    if (board.playerBenchList[i] == null)
                    {
                        Vector3 newPos = new Vector3(board.benchPosition[i].transform.position.x, (SendRayToMousePosition().transform.localScale.y / 2) + 3, board.benchPosition[i].transform.position.z);
                        SendRayToMousePosition().transform.parent.position = newPos;
                        board.playerBenchList[i] = SendRayToMousePosition().transform.parent.gameObject;
                        SendRayToMousePosition().transform.parent.parent = board.transform.GetChild(3).transform;
                        tempShopPieces.Remove(tempSelectedUnit.transform.parent.gameObject);
                        tempSelectedUnit = null;
                        break;
                        
                    }
                }
            }
        }
    }
}
