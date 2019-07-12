using UnityEngine;

public class UnitsMove : MonoBehaviour
{
    float maxDistance = 5000f;
    bool onMove = false;
   [SerializeField] GameObject selectedUnit;

    private void Update()
    {
        PlayerControllSettings();
        SendRayToMousePosition();
    }

    public void SendRayToMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            SelectUnit(hit);
        }
    }

    public void SelectUnit(RaycastHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Unit") && onMove)
        {
            if (selectedUnit == null)
            {
                selectedUnit = hit.collider.gameObject;
            }
        }

        if(onMove)
            MoveUnit(hit);
    }

    public void MoveUnit(RaycastHit hit)
    {
        if (selectedUnit != null)
        {
            Vector3 mousePos = new Vector3(hit.point.x, (hit.transform.localScale.y / 2) + 3, hit.point.z);

            selectedUnit.transform.parent.position = mousePos;
        }
    }

    public void PlayerControllSettings()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
        {
            if (onMove)
            {
                onMove = false;
                selectedUnit = null;
            }
            else
                onMove = true;
        }
    }

}
