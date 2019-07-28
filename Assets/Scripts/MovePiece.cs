using UnityEngine;
using System.Collections.Generic;

public class MovePiece : MonoBehaviour
{
    [SerializeField]
    GameObject candidatePrefab = null;
    GameObject candidateObj;
    GameObject selectedUnit = null;
    
    Board board;
    Transform originOfSelectedUnit;
    float maxRayDistance = 5000f;

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
            
        }
        if (Input.GetMouseButton(0) && selectedUnit != null)
        {
            candidatePlaceHolder();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedUnit != null) MoveUnit(selectedUnit);
            selectedUnit = null;
            originOfSelectedUnit = null;
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
    public void SelectUnit() //birim seçiliver uygun alanı belli eden bir nesne spawnlanır.
    {
        GameObject unitCandidate = SendRayToMousePosition();
        if (unitCandidate != null && unitCandidate.CompareTag("Unit"))
        {
            selectedUnit = unitCandidate.gameObject;
            originOfSelectedUnit = selectedUnit.transform;
            candidateObj = GameObject.Instantiate(candidatePrefab, new Vector3(selectedUnit.transform.position.x, 1.0f, selectedUnit.transform.position.z), Quaternion.identity);
        }
    }
    public void MoveUnit(GameObject selectedUnit) 
    {
        GameObject placeCandidate = SendRayToMousePosition();
        if (placeCandidate != null && placeCandidate.CompareTag("Unit"))
        {
            Debug.Log("hi");
            Vector3 newPos = placeCandidate.transform.position;
            placeCandidate.transform.parent.position = originOfSelectedUnit.position;
            selectedUnit.transform.parent.position = newPos;
        }
        else
        {
            selectedUnit.transform.parent.position = placableBoardPosition(placeCandidate);
        }

        Destroy(candidateObj);
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