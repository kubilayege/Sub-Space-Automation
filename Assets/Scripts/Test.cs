using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    string[] classorrace = new string[3];
    public GameObject testPrefab;
    //classları ve raceleri otomatik almak için hazırlanmış script
    void Start()
    {
        FindClassAndRace();
    }

    private void Update()
    {
        UseAbility();
    }

    void UseAbility()
    {
        if(transform.gameObject.GetComponent<Pieces>().mana > 100)
        {
            Instantiate(testPrefab, new Vector3(0,60, 0), Quaternion.identity);
            transform.gameObject.GetComponent<Pieces>().mana = 0;
        }
    }

    void FindClassAndRace()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.CompareTag("ClassOrRace"))
            {
               // Debug.Log("My Info " + transform.GetChild(i).gameObject.name);
                classorrace[i] = transform.GetChild(i).gameObject.name;
            }
        }
        transform.gameObject.GetComponent<Pieces>().firstRace = classorrace[0];
        transform.gameObject.GetComponent<Pieces>().pieceClass = classorrace[1];
        if (classorrace[2] != null) transform.gameObject.GetComponent<Pieces>().secondaryRace = classorrace[2];
    }
}
