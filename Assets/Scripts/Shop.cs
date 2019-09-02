using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<GameObject> gamePieces; // Oyundaki her piece burda tutulacak
    public List<Transform> pieceSlotsList; //Shop slot pozisyonları listesi
    public List<GameObject> tempShopPieces;
    /*public List<GameObject> tempUpgradeUnit;*/ //upgrade için
    public List<GameObject> unitlist;
    //public int starCount = 0;
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
        GameObject pieceSlots = transform.GetChild(0).GetChild(0).gameObject;
        for(int i = 0; i< pieceSlots.transform.childCount ; i++)
        {
            pieceSlotsList.Add(pieceSlots.transform.GetChild(i));
        }
        match = transform.parent.parent.parent.GetComponent<Match>();
        board = transform.parent.parent.GetComponent<Board>();
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
            if(transform.GetChild(0).GetChild(0).GetChild(i).childCount == 2)
            {
                forAddUnitToPool = transform.GetChild(0).GetChild(0).GetChild(i).GetChild(1).gameObject;
                match.piecePool.Add(forAddUnitToPool);
                tempShopPieces.Remove(forAddUnitToPool); //  remove from shop slot
                forAddUnitToPool.transform.position = new Vector3(6000 - match.GetComponent<EventTest>().round%match.piecePool.Capacity * 128, 0, this.transform.position.z + 900);
                forAddUnitToPool.transform.rotation = Quaternion.identity;
                forAddUnitToPool.transform.parent = this.transform.GetChild(0).transform;
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

                    StartCoroutine(UpgradeUnitQue(board, tempSelectedUnit));
                    board.CountPlayerUnits();
                    break;

                }
                
            }
            
        }
        

    }

    public IEnumerator UpgradeUnitQue(Board board, GameObject unit)
    {
        if(unit != null)
        {
            List<GameObject> tempUpgradeUnit = new List<GameObject>();
            int starCount = 0;
            if(match.GetComponent<EventTest>().preaperOrFight == -1)
            {
                starCount = CheckUnitsForUpgrade(board, board.benchPosition, unit, tempUpgradeUnit, starCount);
                if(starCount != 3)
                {
                    tempUpgradeUnit.Clear();
                    starCount = 0;
                    starCount = CheckUnitsForUpgrade(board, board.chessboardPosition, unit, tempUpgradeUnit, starCount);
                    starCount = CheckUnitsForUpgrade(board, board.benchPosition, unit, tempUpgradeUnit, starCount);
                }
            }
            else
            {
                starCount = CheckUnitsForUpgrade(board, board.chessboardPosition, unit, tempUpgradeUnit, starCount);
                starCount = CheckUnitsForUpgrade(board, board.benchPosition, unit, tempUpgradeUnit, starCount);
            }
            
            if (starCount == 3 || (unit.transform.parent.GetComponent<Pieces>().star == 2 && starCount == 2))
            {
                starCount = 0;
                GameObject newUnit = null;
                GameObject upgradedUnit = null;
                string unitName = unit.transform.parent.GetComponent<Pieces>().pieceName;
                int unitStar = unit.transform.parent.GetComponent<Pieces>().star;
                int indexOfUnit = 0;
                if (board.playerBenchList.IndexOf(tempUpgradeUnit[0]) >= 0)
                {
                    indexOfUnit = board.playerBenchList.IndexOf(tempUpgradeUnit[0]);
                }
                else if (board.playerBoardList.IndexOf(tempUpgradeUnit[0]) >= 0)
                {
                    indexOfUnit = board.playerBoardList.IndexOf(tempUpgradeUnit[0]);
                }


                for (int i = 0; i < unitlist.Count; i++)
                {
                    if (unitName == unitlist[i].transform.GetComponent<Pieces>().pieceName && unitStar + 1 == unitlist[i].transform.GetComponent<Pieces>().star)
                    {
                        newUnit = unitlist[i].transform.gameObject;
                    }
                }

                
                if (newUnit != null) // else koşulu tüm unitlerin üst birimleri eklenince silinecek 
                {
                    if (CheckIfUnitsAreOnBoard(tempUpgradeUnit) && match.GetComponent<EventTest>().preaperOrFight == -1)
                    {
                        float time = match.GetComponent<EventTest>().gameTime + 1f;
                        while (time > 0)
                        {
                            time -= 1;
                            yield return new WaitForSeconds(1);
                        }
                        if(tempUpgradeUnit[2] != null)
                            CompleteUpgrading(board, unit, tempUpgradeUnit, newUnit, upgradedUnit,indexOfUnit);
                    }
                    else
                    {
                        CompleteUpgrading(board, unit, tempUpgradeUnit, newUnit, upgradedUnit, indexOfUnit);
                    }
                }
            }
            else
            {
                tempUpgradeUnit.Clear();
                starCount = 0;
            }
        }
    }

    private int CheckUnitsForUpgrade(Board board, List<GameObject> benchOrBoardList, GameObject unit, List<GameObject> tempUpgradeUnit, int starCount)
    {
        for (int i = 0; i < benchOrBoardList.Count; i++)
        {
            if (benchOrBoardList[i].transform.childCount > 0)
            {
                if (!(board.enemyBoardList.Contains(benchOrBoardList[i].transform.GetChild(0).gameObject)) && benchOrBoardList[i].transform.GetChild(0).GetComponent<Pieces>().pieceName == unit.transform.parent.GetComponent<Pieces>().pieceName &&
                    benchOrBoardList[i].transform.GetChild(0).GetComponent<Pieces>().star == unit.transform.parent.GetComponent<Pieces>().star)
                {
                    if (starCount < 3)
                    {
                        tempUpgradeUnit.Add(benchOrBoardList[i].transform.GetChild(0).gameObject);
                        starCount++;
                    }
                    else { break; }
                }
            }
        }

        return starCount;
    }

    private void CompleteUpgrading(Board board, GameObject unit, List<GameObject> tempUpgradeUnit,GameObject newUnit, GameObject upgradedUnit, int indexOfUnit)
    {
        Vector3 newPos = new Vector3(tempUpgradeUnit[0].transform.position.x, tempUpgradeUnit[0].transform.position.y, tempUpgradeUnit[0].transform.position.z);
        upgradedUnit = Instantiate(newUnit, newPos, Quaternion.identity, tempUpgradeUnit[0].transform.parent.transform) as GameObject;
        upgradedUnit.transform.localScale = tempUpgradeUnit[0].transform.localScale;

        if (upgradedUnit.transform.parent.CompareTag("BoardBlock"))
        {
            board.playerBoardList[indexOfUnit] = upgradedUnit;
        }
        if (upgradedUnit.transform.parent.CompareTag("BenchBlock"))
        {
            board.playerBenchList[indexOfUnit] = upgradedUnit;
        }

        for (int i = tempUpgradeUnit.Count - 1; i >= 0; i--)
        {
            StopCoroutine(tempUpgradeUnit[i].GetComponent<PieceAI>().Fight());

            Destroy(tempUpgradeUnit[i].gameObject);
        }

        Destroy(unit.transform.parent.gameObject);

        tempUpgradeUnit.Clear();

        if (upgradedUnit.transform.GetComponent<Pieces>().star == 2)
        {
            StartCoroutine(UpgradeUnitQue(board, upgradedUnit.transform.GetChild(0).gameObject));
        }

    }

    private bool CheckIfUnitsAreOnBoard(List<GameObject> tempUpg)
    {
        foreach(var unit in tempUpg)
        {
            if (unit.transform.parent.CompareTag("BoardBlock"))
            {
                return true;
            }
        }
        return false;
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

                    StartCoroutine(UpgradeUnitQue(board, tempSelectedUnit));
                    board.CountPlayerUnits();

                    tempSelectedUnit = null;
                    break;

                }
            }
        }

    }
    public void RerollShopWithButton()
    {
        if(transform.parent.GetComponent<PlayerPurse>().gold >= 2)
        {
            ClearSlot();
            DrawPieces();
            transform.parent.GetComponent<PlayerPurse>().ModifyGold(-2);
        }

    }
    
}
