using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : PlaceableObject, ISource
{
    //all crops that can be planted
    private static Dictionary<Crop, int> allCrops;
    //the default amount of crop produced (can be changed for events, for example)
    private static int amount = 2;
    
    //implementation of the state property
    public State currentState { get; set; }
    //current crop in production
    private Crop currentCrop;
    //timer for the crop
    private Timer timer;
    
    //sprite renderer
    private SpriteRenderer sr;
    //empty field sprite (set after the crop is collected)
    private Sprite emptyFieldSprite;

    protected override void Awake()
    {
        base.Awake();
        //get the somponent
        sr = GetComponent<SpriteRenderer>();
        //save the sprite
        emptyFieldSprite = sr.sprite;
    }

    public static void Initialize(Dictionary<Crop, int> crops)
    {
        //initialize all the crops
        allCrops = crops;
    }

    protected override void OnClick()
    {
        //check the state
        switch (currentState)
        {
            case State.Empty:
                //the field is empty -> display the crops available to plant
                ItemsTooltip.ShowTooltip_Static(gameObject, allCrops);
                break;
            case State.InProgress:
                //a crop is growing on the field -> display the timer
                TimerTooltip.ShowTimer_Static(gameObject);
                break;
            case State.Ready:
                //the field is ready -> display the tooltip to collect
                CollectorTooltip.ShowTooltip_Static(gameObject);
                break;
        }
    }

    //implementation of the produce method
    public void Produce(Dictionary<CollectibleItem, int> itemsNeeded, CollectibleItem itemToProduce)
    {
        //if the field is not empty nothing to do
        if (currentState != State.Empty)
        {
            return;
        }

        //check if the product is a crop
        if (itemToProduce is Crop crop)
        {
            //assign the crop
            currentCrop = crop;
        }
        else
        {
            return;
        }

        //check if the player has enough items
        foreach (var itemPair in itemsNeeded)
        {
            //if not enough -> return
            if (!StorageManager.current.IsEnoughOf(itemPair.Key, itemPair.Value))
            {
                Debug.Log("Not enough items");
                return;
            }
        }
        
        //todo take items from the storage

        //change the state
        currentState = State.InProgress;

        //change the sprite
        sr.sprite = currentCrop.growingCrop;

        //add a timer
        timer = gameObject.AddComponent<Timer>();
        //initialize the timer
        timer.Initialize(currentCrop.name, DateTime.Now, currentCrop.productionTime);
        //add a listener to the timer finished event
        timer.TimerFinishedEvent.AddListener(delegate
        {
            //change the state
            currentState = State.Ready;
            //change the sprite
            sr.sprite = currentCrop.readyCrop;
            
            //destroy the timer
            Destroy(timer);
            //nullify the timer
            timer = null;
        });
        timer.StartTimer();
    }

    public void Collect()
    {
        //todo add crop to storage

        //change the stage
        currentState = State.Empty;
        //change the sprite to empty
        sr.sprite = emptyFieldSprite;
        //remove the current crop
        currentCrop = null;
    }
}
