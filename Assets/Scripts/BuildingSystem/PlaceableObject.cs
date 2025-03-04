using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    private bool lastCanBePlacedState = false;
    //if the building is placed or not
    public bool Placed { get; private set; }
    //position on which an object was placed
    //(save it if the new position is not available)
    private Vector3 origin;
    
    //area under the house - stores position and building size
    public BoundsInt area;

    //time elapsed since the touch begun
    private float time = 0f;
    private bool touching;
    private bool moving;
    private SpriteRenderer spriteRenderer;
    private Tween colorTween; // DoTween Tween Reference
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        PanZoom.current.FollowObject(transform);
        lastCanBePlacedState = CanBePlaced();
    }

    public void Load()
    {
        PanZoom.current.UnfollowObject();
        Destroy(GetComponent<ObjectDrag>());
        Place();
    }
    
    /*
     * Check if the building can be placed at its current position
     */
    public bool CanBePlaced()
    {
        //create an area under the building
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        bool canPlacable = BuildingSystem.current.CanTakeArea(areaTemp);
    

        //call the GridBuildingSystem to check the area
        return canPlacable;
    }
    
    /*
     * Make the building placed
     */
    public virtual void Place()
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
        PanZoom.current.UnfollowObject();
        
        //object is new an haven't been placed before
        if (!Placed)
        {
            //if it can be placed
            if (CanBePlaced())
            {
                Place();
                //AnimateColor(Color.green, Color.white);
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
                //AnimateColor(Color.white, Color.red);
            }
            
            Place();
        }
    }
    private void AnimateColor(Color from, Color to)
    {
        colorTween?.Kill(); // Kill any active tween before starting a new one
        spriteRenderer.color = from;
        colorTween = spriteRenderer.DOColor(to, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopAnimation()
    {
        Debug.Log(colorTween==null);
        colorTween?.Kill(); // Stop the animation
        spriteRenderer.color = Color.white; // Reset color to default
    }

    private void Update()
    {
        if (moving)
        {
            bool canPlace = CanBePlaced();

            // If the state changes or dragging just started, animate the color
            if (canPlace != lastCanBePlacedState || colorTween == null || !colorTween.IsActive())
            {
                lastCanBePlacedState = canPlace;

                if (canPlace)
                {
                    AnimateColor(Color.green, Color.white);
                }
                else
                {
                    AnimateColor(Color.white, Color.red);
                }
            }
        }

        if (touching && Input.GetMouseButton(0))
        {
            time += Time.deltaTime;
            if (time > 3f)
            {
                touching = false;
                moving = true;
                gameObject.AddComponent<ObjectDrag>();

                Vector3Int positionInt = BuildingSystem.current.gridLayout.WorldToCell(transform.position);
                BoundsInt areaTemp = area;
                areaTemp.position = positionInt;

                BuildingSystem.current.ClearArea(areaTemp, BuildingSystem.current.MainTilemap);

                // Start the animation as soon as the drag starts
                lastCanBePlacedState = CanBePlaced();
                AnimateColor(lastCanBePlacedState ? Color.green : Color.white, lastCanBePlacedState ? Color.white : Color.red);
            }
        }
    }


    private void OnDestroy()
    {
        colorTween?.Kill();
    }
    private void OnMouseDown()
    {
        time = 0;
        touching = true;
    }

    protected virtual void OnClick() { }

    private void OnMouseUpAsButton()
    {
        if (moving)
        {
            moving = false;
            StopAnimation(); // Stop animation and reset color
            return;
        }

        OnClick();
    }
}
