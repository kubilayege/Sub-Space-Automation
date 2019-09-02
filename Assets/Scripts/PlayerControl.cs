using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    Shop inGameShop;

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        inGameShop = transform.GetChild(1).GetComponent<Shop>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();
    }

    private void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inGameShop.BuyUnit(transform.parent.GetComponent<Board>(), this.GetComponent<PlayerPurse>());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
