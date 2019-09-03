using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class MovePiece : MonoBehaviour
{
    [SerializeField]
    GameObject candidatePrefab = null;
    GameObject candidateObj;
    GameObject selectedUnit = null;
    
    Board board;
    Transform originOfSelectedUnit;
    float maxRayDistance = 5000f;
    public bool movementLock = false;
    private void Start()
    {
        board = transform.parent.gameObject.GetComponent<Board>();
    }
    private void Update()
    {
        PlayerControllSettings();
    }

    public void PlayerControllSettings()
    {
        if (Input.GetMouseButtonDown(0) && selectedUnit == null)
        {
            SelectUnit();
            StartCoroutine(CloseShop());
        }
        if (Input.GetMouseButton(0) && selectedUnit != null)
        {
            candidatePlaceHolder();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedUnit != null)
            {
                MoveUnit(selectedUnit);
            }
            
            selectedUnit = null;
            originOfSelectedUnit = null;
            Destroy(candidateObj); //upgrade sonrası bugdan dolayı eklendi
        }
    }

    public IEnumerator CloseShop()
    {
        if (board.transform.parent.GetChild(0).GetComponent<InGameUI>().shopPanel.activeInHierarchy)
        {
            yield return new WaitForSeconds(0.4f);
            if(board.transform.parent.GetChild(0).GetComponent<InGameUI>().shopPanel.activeInHierarchy && selectedUnit != null)
                board.transform.parent.GetChild(0).GetComponent<InGameUI>().ToggleShop();
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
    public void SelectUnit() //birim seçilir ve uygun alanı belli eden bir nesne spawnlanır.
    {
        GameObject unitCandidate = SendRayToMousePosition();
        if (unitCandidate != null && (board.playerBenchList.Contains(unitCandidate.transform.parent.gameObject) || board.playerBoardList.Contains(unitCandidate.transform.parent.gameObject)))
        {
            selectedUnit = unitCandidate.gameObject;
            originOfSelectedUnit = selectedUnit.transform;
            candidateObj = GameObject.Instantiate(candidatePrefab, new Vector3(selectedUnit.transform.position.x, 1.0f, selectedUnit.transform.position.z), Quaternion.identity);
        }
    }
    public void MoveUnit(GameObject selectedUnit) 
    {
        GameObject placeCandidate = SendRayToMousePosition();
        
        if ((placeCandidate != null && (board.playerBenchList.Contains(placeCandidate.transform.parent.gameObject) || board.playerBoardList.Contains(placeCandidate.transform.parent.gameObject))))
        {
            if((selectedUnit.transform.parent.parent.CompareTag("BoardBlock") || placeCandidate.transform.parent.parent.CompareTag("BoardBlock")) && !movementLock)
            {
                return; 
            }
            Vector3 newPos = placeCandidate.transform.position;
            placeCandidate.transform.parent.position = originOfSelectedUnit.position;
            selectedUnit.transform.parent.position = newPos;

            SawpUnitsPositionOnLists(selectedUnit.transform.parent.gameObject, placeCandidate.transform.parent.gameObject);

        }
        else if(placeCandidate != null)
        {
            if ((selectedUnit.transform.parent.parent.CompareTag("BoardBlock") || placeCandidate.CompareTag("BoardBlock")) && !movementLock)
            {
                return;
            }
            if (RelocateUnitsPositionOnLists(selectedUnit.transform.parent.gameObject, placeCandidate))
            {
                selectedUnit.transform.parent.position = placableBoardPosition(placeCandidate);
            }
        }

        Destroy(candidateObj);
    }
    

    public bool RelocateUnitsPositionOnLists(GameObject selectedUnit, GameObject targetUnit)
    {
        GameObject tempSwapVar;
        int indexOf_SelectedUnit = 0;
        int indexOf_TargetUnit = 0;

        if (selectedUnit.transform.parent.gameObject.CompareTag("BoardBlock"))
        {
            indexOf_SelectedUnit = board.playerBoardList.IndexOf(selectedUnit);
            if (targetUnit.CompareTag("BoardBlock"))
            {
                indexOf_TargetUnit = board.chessboardPosition.IndexOf(targetUnit);
                if(indexOf_TargetUnit != indexOf_SelectedUnit && indexOf_TargetUnit <= 32)
                {
                   // Debug.Log(indexOf_SelectedUnit + "     " + indexOf_TargetUnit);
                    tempSwapVar = board.playerBoardList[indexOf_SelectedUnit];

                    board.playerBoardList[indexOf_TargetUnit] = board.playerBoardList[indexOf_SelectedUnit];
                    board.playerBoardList[indexOf_TargetUnit].transform.parent = board.chessboardPosition[indexOf_TargetUnit].transform;
                    board.playerBoardList[indexOf_SelectedUnit] = null;
                    return true;
                }
                //Destroy(tempSwapVar);
            }
            else if (targetUnit.CompareTag("BenchBlock"))
            {
                indexOf_TargetUnit = board.benchPosition.IndexOf(targetUnit);
                //Debug.Log(indexOf_SelectedUnit + "     " + indexOf_TargetUnit);
                tempSwapVar = board.playerBoardList[indexOf_SelectedUnit];

                board.playerBenchList[indexOf_TargetUnit] = board.playerBoardList[indexOf_SelectedUnit];
                board.playerBenchList[indexOf_TargetUnit].transform.parent = board.benchPosition[indexOf_TargetUnit].transform;
          
                board.playerBoardList[indexOf_SelectedUnit] = null;
                //Destroy(tempSwapVar);

                board.playerUnitCount -= 1;
                return true;
            }
        }
        else if (selectedUnit.transform.parent.gameObject.CompareTag("BenchBlock"))
        {
            indexOf_SelectedUnit = board.playerBenchList.IndexOf(selectedUnit);
            if (board.playerUnitCount < this.GetComponent<Player>().level && targetUnit.CompareTag("BoardBlock"))
            {
                indexOf_TargetUnit = board.chessboardPosition.IndexOf(targetUnit);
                if(indexOf_TargetUnit <= 32)
                {
                    //Debug.Log(indexOf_SelectedUnit + "     " + indexOf_TargetUnit);
                    tempSwapVar = board.playerBenchList[indexOf_SelectedUnit];

                    board.playerBoardList[indexOf_TargetUnit] = board.playerBenchList[indexOf_SelectedUnit];
                    board.playerBoardList[indexOf_TargetUnit].transform.parent = board.chessboardPosition[indexOf_TargetUnit].transform;
                    board.playerBenchList[indexOf_SelectedUnit] = null;

                    board.playerUnitCount += 1;
                    return true;
                }
                //Destroy(tempSwapVar);
            }
            else if (targetUnit.CompareTag("BenchBlock"))
            {
                indexOf_TargetUnit = board.benchPosition.IndexOf(targetUnit);
                if(indexOf_TargetUnit != indexOf_SelectedUnit)
                {
                    //Debug.Log(indexOf_SelectedUnit + "     " + indexOf_TargetUnit);
                    tempSwapVar = board.playerBenchList[indexOf_SelectedUnit];

                    board.playerBenchList[indexOf_TargetUnit] = board.playerBenchList[indexOf_SelectedUnit];

                    board.playerBenchList[indexOf_TargetUnit].transform.parent = board.benchPosition[indexOf_TargetUnit].transform;
                    board.playerBenchList[indexOf_SelectedUnit] = null;
                    return true;
                }
                //Destroy(tempSwapVar);
            }
        }
        return false;
    }

    public void SawpUnitsPositionOnLists(GameObject selectedUnit, GameObject targetUnit)
    {
        GameObject tempSwapVar;
        int indexOf_SelectedUnit = 0;
        int indexOf_TargetUnit = 0;

        if (selectedUnit.transform.parent.gameObject.CompareTag("BoardBlock"))
        {
            indexOf_SelectedUnit = board.playerBoardList.IndexOf(selectedUnit);
            if (targetUnit.transform.parent.CompareTag("BoardBlock"))
            {
                indexOf_TargetUnit = board.playerBoardList.IndexOf(targetUnit);
                //Debug.Log(indexOf_SelectedUnit + "     " + indexOf_TargetUnit);
                tempSwapVar = board.playerBoardList[indexOf_TargetUnit];

                board.playerBoardList[indexOf_TargetUnit] = board.playerBoardList[indexOf_SelectedUnit];
                board.playerBoardList[indexOf_SelectedUnit] = tempSwapVar;

                board.playerBoardList[indexOf_SelectedUnit].gameObject.transform.parent = board.chessboardPosition[indexOf_SelectedUnit].transform;
                board.playerBoardList[indexOf_TargetUnit].transform.parent = board.chessboardPosition[indexOf_TargetUnit].transform;
                //Destroy(tempSwapVar);
            }
            else if (targetUnit.transform.parent.CompareTag("BenchBlock"))
            {
                indexOf_TargetUnit = board.playerBenchList.IndexOf(targetUnit);
                //Debug.Log(indexOf_SelectedUnit + "     " + indexOf_TargetUnit);
                tempSwapVar = board.playerBenchList[indexOf_TargetUnit];

                board.playerBenchList[indexOf_TargetUnit] = board.playerBoardList[indexOf_SelectedUnit];
                board.playerBoardList[indexOf_SelectedUnit] = tempSwapVar;


                board.playerBoardList[indexOf_SelectedUnit].transform.parent = board.chessboardPosition[indexOf_SelectedUnit].transform;
                board.playerBenchList[indexOf_TargetUnit].transform.parent = board.benchPosition[indexOf_TargetUnit].transform;
                //Destroy(tempSwapVar);
            }
        }
        else if (selectedUnit.transform.parent.gameObject.CompareTag("BenchBlock"))
        {
            indexOf_SelectedUnit = board.playerBenchList.IndexOf(selectedUnit);
            if (targetUnit.transform.parent.CompareTag("BoardBlock"))
            {
                indexOf_TargetUnit = board.playerBoardList.IndexOf(targetUnit);
               // Debug.Log(indexOf_SelectedUnit + "     " + indexOf_TargetUnit);
                tempSwapVar = board.playerBoardList[indexOf_TargetUnit];

                board.playerBoardList[indexOf_TargetUnit] = board.playerBenchList[indexOf_SelectedUnit];
                board.playerBenchList[indexOf_SelectedUnit] = tempSwapVar;

                board.playerBenchList[indexOf_SelectedUnit].transform.parent = board.benchPosition[indexOf_SelectedUnit].transform;
                board.playerBoardList[indexOf_TargetUnit].transform.parent = board.chessboardPosition[indexOf_TargetUnit].transform;
                //Destroy(tempSwapVar);
            }
            else if (targetUnit.transform.parent.CompareTag("BenchBlock"))
            {
                indexOf_TargetUnit = board.playerBenchList.IndexOf(targetUnit);
               // Debug.Log(indexOf_SelectedUnit + "     " + indexOf_TargetUnit);
                tempSwapVar = board.playerBenchList[indexOf_TargetUnit];

                board.playerBenchList[indexOf_TargetUnit] = board.playerBenchList[indexOf_SelectedUnit];
                board.playerBenchList[indexOf_SelectedUnit] = tempSwapVar;

                board.playerBenchList[indexOf_SelectedUnit].transform.parent = board.benchPosition[indexOf_SelectedUnit].transform;
                board.playerBenchList[indexOf_TargetUnit].transform.parent = board.benchPosition[indexOf_TargetUnit].transform;
                //Destroy(tempSwapVar);
            }
        }
    }


    public void candidatePlaceHolder()
    {
        Vector3 tempPos = placableBoardPosition(SendRayToMousePosition());
        candidateObj.transform.position = new Vector3(tempPos.x, 1, tempPos.z);
        //selectedUnit.transform.parent.position = new Vector3(tempPos.x, (selectedUnit.transform.localScale.y/2  ) + 3, tempPos.z); // işe yarayabilir.
    }
    public Vector3 placableBoardPosition(GameObject candidatePlace) //
    {
        Vector3 newPos = originOfSelectedUnit.transform.position;

        if(candidatePlace != null)
        {
            for (int i = 0; i < board.chessboardSize / 2; i++)
            {
                if (candidatePlace.transform.gameObject.name == board.chessboardPosition[i].gameObject.name)
                {
                    newPos = new Vector3(candidatePlace.transform.position.x, (selectedUnit.transform.localScale.y / 2) + 3, candidatePlace.transform.position.z);
                    return newPos;
                }
            }
            for (int i = 0; i < board.benchSize; i++)
            {
                if (candidatePlace.transform.gameObject.name == board.benchPosition[i].gameObject.name)
                {
                    newPos = new Vector3(candidatePlace.transform.position.x, (selectedUnit.transform.localScale.y / 2) + 3, candidatePlace.transform.position.z);
                    return newPos;
                }
            }
        }
        return newPos;
    }
    
}