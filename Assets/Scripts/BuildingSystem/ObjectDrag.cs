using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private static ObjectDrag currentDragging; // Track current dragging object

    private Vector3 startPos;
    private float deltaX, deltaY;

    void Start()
    {
        if (currentDragging != null && currentDragging != this)
        {
            Destroy(this);
            return;
        }

        currentDragging = this;

        startPos = Input.mousePosition;
        startPos = Camera.main.ScreenToWorldPoint(startPos);

        deltaX = startPos.x - transform.position.x;
        deltaY = startPos.y - transform.position.y;
    }

    void Update()
    {
        if (currentDragging != this) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos = new Vector3(mousePos.x - deltaX, mousePos.y - deltaY);

        Vector3Int cellPos = BuildingSystem.current.gridLayout.WorldToCell(pos);
        transform.position = BuildingSystem.current.gridLayout.CellToLocalInterpolated(cellPos);
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0) && currentDragging == this)
        {
            gameObject.GetComponent<PlaceableObject>().CheckPlacement();
            currentDragging = null; // Reset static reference
            Destroy(this);
        }
    }
}
