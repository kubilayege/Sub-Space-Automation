using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<GameObject> gamePieces; // Oyundaki her piece burda tutulacak
    public List<Transform> pieceSlotsList; //Shop slot pozisyonları listesi
    public List<GameObject> tempShopPieces;
    public List<GameObject> tempUpgradeUnit; //upgrade için
    public List<GameObject> unitlist;
    public int starCount = 0;
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

                    UpgradeUnit(board, tempSelectedUnit);

                    tempSelectedUnit = null;
                    break;

                }
                
            }
            
        }
        

    }
    public void UpgradeUnit(Board board, GameObject unit)
    {
        // chessboard kontrol ediliyor
        for (int i = 0; i < 32; i++) 
        {
            if (board.chessboardPosition[i].transform.childCount > 0)
            {
                if (board.chessboardPosition[i].transform.GetChild(0).name == unit.transform.parent.name)
                {
                    if (starCount < 3)
                    {
                        //tempUpgradeUnit.Add(unit.transform.parent.gameObject);
                        tempUpgradeUnit.Add(board.chessboardPosition[i].transform.GetChild(0).gameObject);
                        starCount++;
                    }
                }
            }
            
        }

        // bench kontrol ediliyor
        for (int i = 0; i < 8; i++) 
        {
            if (board.benchPosition[i].transform.childCount > 0)
            {
                if (board.benchPosition[i].transform.GetChild(0).name == unit.transform.parent.name)
                {
                    if (starCount < 3)
                    {
                        //tempUpgradeUnit.Add(unit.transform.parent.gameObject);
                        tempUpgradeUnit.Add(board.benchPosition[i].transform.GetChild(0).gameObject);
                        starCount++;
                    }
                }
            }
        }

        //yükseltme işlemi
        if(starCount > 2)
        {
            GameObject newUnit = null;
            string unitName = unit.transform.parent.GetComponent<Pieces>().pieceName;
            int unitStar = unit.transform.parent.GetComponent<Pieces>().star;
           int indexOfUnit =  board.playerBenchList.IndexOf(tempUpgradeUnit[0]);
            for (int i = 0; i < 3; i++)
            {
                if (unitName == unitlist[i].transform.GetComponent<Pieces>().pieceName && unitStar + 1 == unitlist[i].transform.GetComponent<Pieces>().star)
                {
                    newUnit = unitlist[i].transform.gameObject;

                    Debug.Log("New unit " + newUnit);
                }
                
            }

            if(newUnit == null) // if koşulu tüm unitlerin üst birimleri eklenince silinecek doğru kısım else kısmı
            {
                Vector3 newPos = new Vector3(tempUpgradeUnit[0].transform.position.x, tempUpgradeUnit[0].transform.position.y, tempUpgradeUnit[0].transform.position.z);
                GameObject a = Instantiate(tempUpgradeUnit[0].transform.gameObject, newPos, Quaternion.identity, tempUpgradeUnit[0].transform.parent) as GameObject;
            }
            else
            {
                Vector3 newPos = new Vector3(tempUpgradeUnit[0].transform.position.x, tempUpgradeUnit[0].transform.position.y, tempUpgradeUnit[0].transform.position.z);
                GameObject a = Instantiate(newUnit, newPos, Quaternion.identity, tempUpgradeUnit[0].transform.parent) as GameObject;
                a.transform.localScale = tempUpgradeUnit[0].transform.localScale;
                board.playerBenchList[indexOfUnit] = a;
            }
            

            for (int i = tempUpgradeUnit.Count - 1; i >= 0; i--)
            {
                Destroy(tempUpgradeUnit[i].gameObject);
            }
            for (int i = starCount - 1; i >= 0; i--)
            {
                tempUpgradeUnit.RemoveAt(i);
            }
            starCount = 0;
            /*unit.transform.parent.GetComponent<Pieces>().star*/
        }
        else
        {
            for (int i = starCount-1; i >= 0; i--)
            {
                tempUpgradeUnit.RemoveAt(i);
            }
            starCount = 0;
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

                    UpgradeUnit(board, tempSelectedUnit);

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
