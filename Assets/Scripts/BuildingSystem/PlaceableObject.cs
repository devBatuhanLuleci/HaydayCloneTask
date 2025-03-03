using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    //if the building is placed or not
    public bool Placed { get; private set; }
    //position on which an object was placed
    //(save it if the new position is not available)
    private Vector3 origin;
    
    //area under the house - stores position and building size
    public BoundsInt area;

    /*
     * Check if the building can be placed at its current position
     */
    public bool CanBePlaced()
    {
        //create an area under the building
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        //call the GridBuildingSystem to check the area
        return BuildingSystem.current.CanTakeArea(areaTemp);
    }
    
    /*
     * Make the building placed
     */
    public void Place()
    {
        //create an area under the building
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        //set the bool
        Placed = true;
        //save position
        origin = transform.position;
        
        //call the system to 
        BuildingSystem.current.TakeArea(areaTemp);
    }

    public void CheckPlacement()
    {
        //object is new an haven't been placed before
        if (!Placed)
        {
            //if it can be placed
            if (CanBePlaced())
            {
                Place();
            }
            else
            {
                //destroy this object (because it is new)
                Destroy(transform.gameObject);
            }
        
            //open the shop afterwards
            ShopManager.current.ShopButton_Click();
        }
        //editing the map, object has been placed before
        else
        {
            //if cannot be placed
            if (!CanBePlaced())
            {
                //reset the position to origin
                transform.position = origin;
            }
            
            Place();
        }
        
    }

    //time elapsed since the touch begun
    private float time = 0f;
    private bool touching;

    private void Update()
    {
        //detected long touch on a placed object
        if (!touching && Placed)
        {
            //touch begun - start time
            if (Input.GetMouseButtonDown(0))
            {
                time = 0;
            }
            //holding touch
            else if(Input.GetMouseButton(0))
            {
                //increase time elapsed
                time += Time.deltaTime;

                //time limit exceeded
                if (time > 3f)
                {
                    touching = true;
                    //add component to drag
                    gameObject.AddComponent<ObjectDrag>();

                    //prepare area
                    Vector3Int positionInt = BuildingSystem.current.gridLayout.WorldToCell(transform.position);
                    BoundsInt areaTemp = area;
                    areaTemp.position = positionInt;
                    
                    //clear area on which the object was standing on
                    BuildingSystem.current.ClearArea(areaTemp, BuildingSystem.current.MainTilemap);
                }
            }
            
        }

        //touch released
        if (touching && Input.GetMouseButtonUp(0))
        {
            touching = false;
        }
    }
}
