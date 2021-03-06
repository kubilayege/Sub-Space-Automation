﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourierMovement : MonoBehaviour
{
    Vector3 targetPosition;
    Vector3 lookAtTarget;
    Quaternion playerRot;

    float maxDistance = 2000;
    float rotSpeed = 20f;
    float moveSpeed = 400f;
    bool isMoving;
    
    private void Update()
    {
        PlayerControllSettings();
    }

    void PlayerControllSettings()
    {
        if(Input.GetMouseButton(1))
        {
            SetTargetPositionForMove();
        }
        if(isMoving)
        {
            Move();
        }
    }

    void SetTargetPositionForMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            targetPosition = hit.point;
            lookAtTarget = new Vector3(targetPosition.x - transform.position.x , 
                                       0 ,
                                       targetPosition.z - transform.position.z);

            if(lookAtTarget != Vector3.zero)
                playerRot = Quaternion.LookRotation(lookAtTarget);

            isMoving = true;
        }
    }

    private void Move()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                playerRot,
                                                rotSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position , 
                                                new Vector3(targetPosition.x, transform.localScale.y/2 +1 ,targetPosition.z) , 
                                                moveSpeed * Time.deltaTime );

        if(transform.position == targetPosition)
        {
            isMoving = false;
        }
    }
    
}
