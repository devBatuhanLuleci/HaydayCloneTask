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
    public Arrow arrowPrefab; // Reference to your Arrow UI element prefab
    private Arrow currentArrow; // Reference to the instantiated arrow
    private bool arrowSpawned = false; // Flag to track if the arrow has already been spawned

    protected virtual void Awake()
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

        StopAnimation();
        //call the system to 
        BuildingSystem.current.TakeArea(areaTemp);
    }

    public void CheckPlacement()
    {
        PanZoom.current.UnfollowObject();

        moving = false;
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
        // Set the alpha from 1 (255) to approximately 0.588 (150) for the effect
        from.a = 1f; // fully opaque
        to.a = 0.588f; // semi-transparent

        colorTween?.Kill(); // Kill any active tween before starting a new one
        spriteRenderer.color = from;
        colorTween = spriteRenderer.DOColor(to, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    private void StopAnimation()
    {
        colorTween?.Kill(); // Stop the animation
        spriteRenderer.color = new Color(1, 1, 1, 1); // Reset color to white with full opacity (alpha = 1)
    }

    private void Update()
    {
        if (moving || !Placed)
        {
            bool canPlace = CanBePlaced();

            // If the state changes or dragging just started, animate the color
            if (canPlace != lastCanBePlacedState || colorTween == null || !colorTween.IsActive())
            {
                lastCanBePlacedState = canPlace;

                if (canPlace)
                {
                    AnimateColor(Color.white, Color.white);
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

            if (time > 1f && !arrowSpawned) // Only spawn the arrow once
            {
                SpawnArrow();
                arrowSpawned = true;
            }

            // Update the arrow's fill amount based on the touch duration
            if (currentArrow != null)
            {
                // Lerp the fill amount based on the time held
                currentArrow.UpdateFillAmount(Mathf.Min(time / 2f, 1f), 2f); // Full progress at 2 seconds
            }
            // If the touch is held for 3 seconds, stop the animation and start drag
            if (time > 2f)
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
                AnimateColor(lastCanBePlacedState ? Color.white : Color.white, lastCanBePlacedState ? Color.white : Color.red);
               
                if (arrowSpawned)
                {
                    arrowSpawned = false;
                    if (currentArrow != null)
                    {
                        currentArrow.StopAnimation(); // Stop animation if user releases input
                    }
                }
            }
          
        }
    }
    public void SpawnArrow()
    {
        if (arrowPrefab != null && currentArrow == null)
        {
            currentArrow = Instantiate(arrowPrefab, new Vector3(transform.position.x, transform.position.y + arrowPrefab.transform.position.y, transform.position.z), Quaternion.identity, transform) as Arrow;
            currentArrow.gameObject.SetActive(true);
            currentArrow.UpdateFillAmount(0f, 2f); // Initialize the fill amount to 0 and lerp over 2 seconds
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

        // Instantiate and activate the arrow at the start of the touch
     
    }

    protected virtual void OnClick() { }

    public void OnMouseUpAsButton()
    {
        if (arrowSpawned) 
        {
            arrowSpawned = false;
            if (currentArrow != null)
            {
                currentArrow.StopAnimation(); // Stop animation if user releases input
            }
        }
        if (touching)
        {
            touching = false;   
        }
        if (moving)
        {
         
            moving = false;
            return;
        }

        OnClick();
    }
}
