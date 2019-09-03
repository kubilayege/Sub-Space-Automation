using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    Shop inGameShop;
    InGameUI inGameUI;
    Board board;
    EventTest events;
    GameObject selectedUnit;
    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        inGameShop = transform.GetChild(1).GetComponent<Shop>();
        inGameUI = transform.parent.parent.GetChild(0).GetComponent<InGameUI>();
        board = transform.parent.GetComponent<Board>();
        events = transform.parent.parent.GetComponent<EventTest>();
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
        if (Input.GetMouseButtonUp(0))
        {
            SelectUnit();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
    public GameObject SendRayToMousePosition()//mouse pozisyonundaki objeyi geri döndürür.
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 5000f))
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    public void SelectUnit()
    {
        selectedUnit = SendRayToMousePosition();
        if(selectedUnit == null)
        {
            return;
        }
        if(selectedUnit.CompareTag("Unit"))
        {
            if ((board.playerBoardList.Contains(selectedUnit.transform.parent.gameObject)))
            {
                if(events.preaperOrFight == 1)
                {
                    inGameUI.sellButton.SetActive(true);
                    inGameUI.unitToSell = selectedUnit.transform.parent.gameObject;
                    inGameUI.listToUnitSell = board.playerBoardList;
                    inGameUI.boardOfUnitSell = board;
                }
            }
            else if ((board.playerBenchList.Contains(selectedUnit.transform.parent.gameObject)))
            {
                inGameUI.sellButton.SetActive(true);
                inGameUI.unitToSell = selectedUnit.transform.parent.gameObject;
                inGameUI.listToUnitSell = board.playerBenchList;
                inGameUI.boardOfUnitSell = board;
            }
            else
            {
                StartCoroutine(DelayedDeactivation(0.15f));
            }
        }
        else
        {
            StartCoroutine(DelayedDeactivation(0.15f));
        }
    }

    IEnumerator DelayedDeactivation(float delay)
    {
        float time = delay;
        while (time > 0)
        {
            time-=Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        inGameUI.sellButton.SetActive(false);
    }
}
