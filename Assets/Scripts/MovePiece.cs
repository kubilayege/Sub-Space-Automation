﻿using UnityEngine;
using System.Collections.Generic;

public class MovePiece : MonoBehaviour
{
    [SerializeField]
    GameObject selectedUnit = null;
    [SerializeField]
    GameObject candidatePrefab = null;
    GameObject candidateObj;
    PlayerController playerController;
    Transform originOfSelectedUnit;
    float maxRayDistance = 5000f;

    private void Start()
    {
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }
    private void Update()
    {
        PlayerControllSettings();
    }

    public void PlayerControllSettings()
    {
        if (Input.GetMouseButton(0) && selectedUnit == null)
        {
            SelectUnit();
        }
        if (!Input.GetMouseButtonUp(0) && selectedUnit != null)
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

    public RaycastHit SendRayToMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance))
        {
            return hit;
        }
        return hit;
    }

    public void SelectUnit()
    {
        RaycastHit unitCandidate = SendRayToMousePosition();
        if (unitCandidate != null)
        {
            if (unitCandidate.collider.gameObject.CompareTag("Unit"))
            {
                selectedUnit = unitCandidate.collider.gameObject;
                originOfSelectedUnit = selectedUnit.transform;
                candidateObj = GameObject.Instantiate(candidatePrefab, new Vector3(selectedUnit.transform.position.x, 1.0f, selectedUnit.transform.position.z), Quaternion.identity);
            }
        }
    }

    public void MoveUnit(GameObject selectedUnit) //bu kod doğru...
    {
        RaycastHit placeCandidate = SendRayToMousePosition();
        if (placeCandidate.collider.gameObject.CompareTag("Unit"))
        {
            Vector3 newPos = placeCandidate.transform.position;
            placeCandidate.collider.gameObject.transform.parent.position = originOfSelectedUnit.position;
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
    }
    Vector3 placableBoardPosition(RaycastHit candidatePlace)
    {
        Vector3 newPos = originOfSelectedUnit.transform.position;
        //if (candidatePlace.collider.gameObject.CompareTag("Unit"))
        //{
        //    newPos = candidatePlace.transform.position;
        //    candidatePlace.collider.gameObject.transform.parent.position = originOfSelectedUnit.position;
        //    return newPos;
        //}
        for (int i = 0; i < playerController.chessboardSize / 2; i++)
        {
            if(candidatePlace.transform.gameObject.name == playerController.chessboardPieces[i].gameObject.name)
            {
                newPos = new Vector3(candidatePlace.transform.position.x, (selectedUnit.transform.localScale.y / 2) + 3, candidatePlace.transform.position.z);
                return newPos;
            }
        }
        return newPos;
    }
}