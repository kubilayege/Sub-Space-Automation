using UnityEngine;

public class UnitsMove : MonoBehaviour
{
    [SerializeField]
    GameObject selectedUnit;
    Board playerController;

    float maxRayDistance = 5000f;

    private void Start()
    {
        playerController = GameObject.Find("PlayerController").GetComponent<Board>();
    }
    private void Update()
    {
        PlayerControllSettings();
    }

    public void PlayerControllSettings()
    {
        if (Input.GetMouseButton(0))
        {
            SendRayToMousePosition();
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(selectedUnit != null) FindClosestPlace(selectedUnit);
            selectedUnit = null;
        }
        
    }

    public void SendRayToMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance))
        {
            SelectUnit(hit);
        }
    }

    public void SelectUnit(RaycastHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Unit"))
        {
            if (selectedUnit == null)
            {
                selectedUnit = hit.collider.gameObject;
            }
        }
        MoveUnit(hit);
    }

    public void MoveUnit(RaycastHit hit) //bu kod doğru...
    {
        if (selectedUnit != null)
        {
            Vector3 mousePos = new Vector3(hit.point.x, (selectedUnit.transform.localScale.y / 2) + 3, hit.point.z);
            selectedUnit.transform.parent.position = mousePos;
        }
    }

    void FindClosestPlace(GameObject selected)
    {
        float distance = 0.0f;
        float minDistance = 5000.0f;
        Vector3 bestPosition = Vector3.zero;

        for (int i = 0; i < playerController.chessboardSize / 2; i++)
        {
            distance = (selected.transform.position - playerController.chessboardPieces[i].transform.position).magnitude;
            if (distance <= minDistance)
            {
                minDistance = distance;
                bestPosition = playerController.chessboardPieces[i].transform.position;
            }
        }
        if (bestPosition != Vector3.zero)
        {
            selected.transform.position = new Vector3(bestPosition.x,
                                                      (selected.transform.localScale.y / 2) + 1,
                                                      bestPosition.z);
        }
        
    }
}