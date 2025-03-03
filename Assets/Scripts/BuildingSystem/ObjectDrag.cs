using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    //start position
    private Vector3 startPos;
    //offset from center mouse position
    private float deltaX, deltaY;
    
    // Start is called before the first frame update
    void Start()
    {
        //save start position
        startPos = Input.mousePosition;
        //convert it from screen to world coordinates
        startPos = Camera.main.ScreenToWorldPoint(startPos);

        //calculate offsets
        deltaX = startPos.x - transform.position.x;
        deltaY = startPos.y - transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //get the current position of the mouse (touch) and convert to world
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //calculate the new position by subtracting the offset
        Vector3 pos = new Vector3(mousePos.x - deltaX, mousePos.y - deltaY);

        //convert to cell position on a grid
        Vector3Int cellPos = BuildingSystem.current.gridLayout.WorldToCell(pos);
        //convert back to local to provide grid snapping
        transform.position = BuildingSystem.current.gridLayout.CellToLocalInterpolated(cellPos);
    }

    private void LateUpdate()
    {
        //touch release - object has to be placed
        if (Input.GetMouseButtonUp(0))
        {
            //check if we can place
            gameObject.GetComponent<PlaceableObject>().CheckPlacement();
            //destroy drag since we don't need it
            Destroy(this);
        }
    }
}
