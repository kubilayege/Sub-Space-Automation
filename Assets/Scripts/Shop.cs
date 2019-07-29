using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<GameObject> piecesOnPool;
    public List<Transform> pieceSlotsList;
    public List<GameObject> tempShopPieces;
    float maxRayDistance = 5000;
    public Match match;
    public Board board;
    GameObject tempSelectedUnit;
    GameObject forAddUnitToPool;
    void Start()
    {
        InitializeVariables();
        DrawPieces();
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
    }

    public void DrawPieces()
    {
        GameObject pieceSlots = transform.GetChild(0).gameObject;
        for (int i=0; i < pieceSlots.transform.childCount; i++)
        {
            int maxIndex = piecesOnPool.Count - 1;
            int randomIndex = Random.Range(0, maxIndex);
           // Debug.Log(piecesOnPool.Count);
            tempShopPieces.Add(Instantiate(piecesOnPool[randomIndex], pieceSlotsList[i].transform.position, Quaternion.identity, pieceSlotsList[i].transform));
            
            piecesOnPool.RemoveAt(randomIndex);
        }
    }
   public void ClearSlot()
    {
        Debug.Log("i called from event manager");
        for (int i = 0; i < 5; i++)
        {
            if(transform.GetChild(0).GetChild(i).GetChild(1).gameObject != null)
            {
                forAddUnitToPool = transform.GetChild(0).GetChild(i).GetChild(1).gameObject;
                piecesOnPool.Add(forAddUnitToPool);
                Destroy(transform.GetChild(0).GetChild(i).GetChild(1).gameObject); //  destroy from shop slot
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
            tempSelectedUnit = SendRayToMousePosition();
            if (tempSelectedUnit.transform.parent.parent.CompareTag("Slot"))
            {
                for (int i = 0; i < board.benchSize; i++)
                {
                    if (board.playerBenchList[i] == null)
                    {
                        Vector3 newPos = new Vector3(board.benchPosition[i].transform.position.x, (SendRayToMousePosition().transform.localScale.y / 2) + 3, board.benchPosition[i].transform.position.z);
                        SendRayToMousePosition().transform.parent.position = newPos;
                        board.playerBenchList[i] = SendRayToMousePosition().transform.parent.gameObject;
                        SendRayToMousePosition().transform.parent.parent = board.transform.GetChild(3).transform;
                        tempSelectedUnit = null;
                        break;
                        
                    }
                }
            }
        }
    }
}
