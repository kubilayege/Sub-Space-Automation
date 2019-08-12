using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<GameObject> gamePieces; // Oyundaki her piece burda tutulacak
    public List<Transform> pieceSlotsList; //Shop slot pozisyonları listesi
    public List<GameObject> tempShopPieces;
    float maxRayDistance = 5000;
    int poolSize = 100;
    public Match match;
    public Board board;
    GameObject forAddUnitToPool;
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

        match = transform.parent.parent.parent.parent.GetComponent<Match>();
        board = match.boards[0].GetComponent<Board>();
    }

    public void DrawPieces()
    {
        //GameObject pieceSlots = transform.GetChild(0).gameObject;
        for (int i=0; i < pieceSlotsList.Count; i++)
        {
            if(pieceSlotsList.Count != 0)
            {
                int maxIndex = match.piecePool.Count - 1;
                int randomIndex = Random.Range(0, maxIndex);
                // Debug.Log(piecesOnPool.Count);
                if (match.piecePool[randomIndex] == null) return;
                tempShopPieces.Add(match.piecePool[randomIndex]);
                tempShopPieces[tempShopPieces.Count - 1].transform.position = pieceSlotsList[i].transform.position;
                tempShopPieces[tempShopPieces.Count - 1].transform.rotation = pieceSlotsList[i].transform.rotation;
                tempShopPieces[tempShopPieces.Count - 1].transform.parent = pieceSlotsList[i].transform;
                match.piecePool.RemoveAt(randomIndex);
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
                match.piecePool.Add(forAddUnitToPool);
                tempShopPieces.Remove(forAddUnitToPool); //  remove from shop slot
                forAddUnitToPool.transform.position = new Vector3(6000 - match.GetComponent<EventTest>().round%match.piecePool.Capacity * 128, 0, this.transform.position.z + 900);
                forAddUnitToPool.transform.rotation = Quaternion.identity;
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
    public void BuyUnit(Board board, PlayerPurse purse)
    {
        if (!transform.gameObject.activeSelf)
        {
            return;
        }

        GameObject tempSelectedUnit = SendRayToMousePosition();

        if (tempSelectedUnit != null && tempSelectedUnit.transform.parent.parent.CompareTag("Slot") && tempSelectedUnit.CompareTag("Unit"))
        {
            if (tempSelectedUnit.transform.parent.GetComponent<Pieces>().pieceCost > purse.gold)
            {
                return;  //not enough gold
            }

            purse.ModifyGold(-(tempSelectedUnit.transform.parent.GetComponent<Pieces>().pieceCost));

            for (int i = 0; i < board.benchSize; i++)
            {
                if (board.playerBenchList[i] == null)
                {
                    Vector3 newPos = new Vector3(board.benchPosition[i].transform.position.x, (tempSelectedUnit.transform.localScale.y / 2) + 3, board.benchPosition[i].transform.position.z);
                    tempSelectedUnit.transform.parent.position = newPos;
                    tempSelectedUnit.transform.parent.rotation = Quaternion.identity;
                    board.playerBenchList[i] = tempSelectedUnit.transform.parent.gameObject;
                    tempShopPieces.Remove(tempSelectedUnit.transform.parent.gameObject);  //Shop ekranındaki gösterilen listeden siliniyor
                    tempSelectedUnit.transform.parent.parent = board.benchPosition[i].transform; //Bench blokğunun child'ı oluyor
                    tempSelectedUnit = null;
                    break;

                }
            }
        }

    }
    public void BotBuyUnit(Board board, PlayerPurse purse, GameObject tempSelectedUnit)
    {

        if (tempSelectedUnit != null && tempSelectedUnit.transform.parent.parent.CompareTag("Slot") && tempSelectedUnit.CompareTag("Unit"))
        {
            if (tempSelectedUnit.transform.parent.GetComponent<Pieces>().pieceCost > purse.gold)
            {
                return;  //not enough gold
            }

            purse.ModifyGold(-(tempSelectedUnit.transform.parent.GetComponent<Pieces>().pieceCost));

            for (int i = 0; i < board.benchSize; i++)
            {
                if (board.playerBenchList[i] == null)
                {
                    Vector3 newPos = new Vector3(board.benchPosition[i].transform.position.x, (tempSelectedUnit.transform.localScale.y / 2) + 3, board.benchPosition[i].transform.position.z);
                    tempSelectedUnit.transform.parent.position = newPos;
                    tempSelectedUnit.transform.parent.rotation = Quaternion.identity;
                    board.playerBenchList[i] = tempSelectedUnit.transform.parent.gameObject;
                    tempShopPieces.Remove(tempSelectedUnit.transform.parent.gameObject);  //Shop ekranındaki gösterilen listeden siliniyor
                    tempSelectedUnit.transform.parent.parent = board.benchPosition[i].transform; //Bench blokğunun child'ı oluyor
                    tempSelectedUnit = null;
                    break;

                }
            }
        }

    }
    public void RerollShopWithButton()
    {
        if(transform.parent.parent.GetComponent<PlayerPurse>().gold >= 2)
        {
            ClearSlot();
            DrawPieces();
            transform.parent.parent.GetComponent<PlayerPurse>().ModifyGold(-2);

        }

    }
}
