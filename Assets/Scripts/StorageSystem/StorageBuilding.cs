using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StorageBuilding : PlaceableObject
{
    //UI for the storage building
    private StorageUI storageUI;

    //amount of items the player has
    private int currentTotal = 0;
    //maximum amount of items that the player can have
    private int storageMax = 100;
    
    //storage building name
    public string Name { get; private set; }

    //UI window prefab 
    [SerializeField] private GameObject windowPrefab;
    //tools needed to increase the storage space
    [SerializeField] private List<Tool> itemsToIncrease;

    //items that the building has
    private Dictionary<CollectibleItem, int> items;
    //tools needed to increase storage
    private Dictionary<CollectibleItem, int> tools;

    /*
     * Initialize the storage building
     */
    public void Initialize(Dictionary<CollectibleItem, int> itemAmounts, string name)
    {
        //set the name
        Name = name;

        //instantiate UI
        GameObject window = Instantiate(windowPrefab, GameManager.current.canvas.transform);
        //make it invisible
        window.SetActive(false);
        //get the storage ui script
        storageUI = window.GetComponent<StorageUI>();
        
        //set the name text
        storageUI.SetNameText(name);

        //initialize the items
        items = itemAmounts;
        //calculate the current total
        currentTotal = itemAmounts.Values.Sum();

        //initialize tools dictionary
        tools = new Dictionary<CollectibleItem, int>();
        foreach (var item in itemsToIncrease)
        {
            //add the amount of items needed
            tools.Add(item, 1);
        }
        
        //initialize the UI
        storageUI.Initialize(currentTotal, storageMax, items, tools, IncreaseStorage);
    }

    /*
     * Increase the storage space
     */
    private void IncreaseStorage()
    {
        //go through each tool in the tools dictionary
        foreach (var toolPair in tools)
        {
            //check if the player has enough of it
            if (!StorageManager.current.IsEnoughOf(toolPair.Key, toolPair.Value))
            {
                Debug.Log("Not enough tools");
                return;
            }
        }
        
        //todo take items from storage

        //increase storgae
        storageMax += 50;
        
        //initialize storage UI again
        storageUI.Initialize(currentTotal, storageMax, items, tools, IncreaseStorage);
    }

    public virtual void onClick()
    {
        //make the UI visible after the click on the building
        storageUI.gameObject.SetActive(true);
    }

    private void OnMouseUpAsButton()
    {
        onClick();
    }
}
