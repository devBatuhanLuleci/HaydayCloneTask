using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    //singletone pattern
    public static StorageManager current;

    //prefabs for storage buildings
    [SerializeField] private GameObject barnPrefab;
    [SerializeField] private GameObject siloPrefab;

    //path to load collectible items from resources
    private string itemsPath = "Storage";
    //dictionaries to store different types of items
    private Dictionary<AnimalProduct, int> animalProducts;
    private Dictionary<Crop, int> crops;
    private Dictionary<Feed, int> feeds;
    private Dictionary<Fruit, int> fruits;
    private Dictionary<Product, int> products;
    private Dictionary<Tool, int> tools;

    //dictionary for barn items
    private Dictionary<CollectibleItem, int> barnItems;
    //dictionary for barn items
    private Dictionary<CollectibleItem, int> siloItems;

    //storage buildings
    private SampleBuilding Barn;
    private SampleBuilding Silo;

    private void Awake()
    {
        //initialize the singletone
        current = this;
        //load all items from resources
        Dictionary<CollectibleItem, int> itemsAmounts = LoadItems();
        //sort all items into different dictionaries
        Sort(itemsAmounts);
        
        //initialize the field with all the crops
        Field.Initialize(crops);
    }

    /*
     * Load collectible items from resources
     */
    private Dictionary<CollectibleItem, int> LoadItems()
    {
        //create a dictionary for all items
        Dictionary<CollectibleItem, int> itemAmounts = new Dictionary<CollectibleItem, int>();
        //load collectible items from resources
        CollectibleItem[] allItems = Resources.LoadAll<CollectibleItem>(itemsPath);

        for (int i = 0; i < allItems.Length; i++)
        {
            //check if the level is less or equal to the current
            if (allItems[i].Level >= LevelSystem.Level)
            {
                //todo remove 2 in a real game
                itemAmounts.Add(allItems[i], 2);
            }
        }

        //return dictionary with items
        return itemAmounts;
    }

    /*
     * Sort items into different categories
     */
    private void Sort(Dictionary<CollectibleItem, int> itemsAmounts)
    {
        //initialize dictionaries
        animalProducts = new Dictionary<AnimalProduct, int>();
        crops = new Dictionary<Crop, int>();
        feeds = new Dictionary<Feed, int>();
        fruits = new Dictionary<Fruit, int>();
        products = new Dictionary<Product, int>();
        tools = new Dictionary<Tool, int>();

        siloItems = new Dictionary<CollectibleItem, int>();
        barnItems = new Dictionary<CollectibleItem, int>();

        //go through each item and determine the type
        foreach (var itemPair in itemsAmounts)
        {
            if (itemPair.Key is AnimalProduct animalProduct)
            {
                //add item to the appropriate dictionaries
                animalProducts.Add(animalProduct, itemPair.Value);
                barnItems.Add(animalProduct, itemPair.Value);
            }
            else if (itemPair.Key is Crop crop)
            {
                crops.Add(crop, itemPair.Value);
                siloItems.Add(crop, itemPair.Value);   
            }
            else if (itemPair.Key is Feed feed)
            {
                feeds.Add(feed, itemPair.Value);
                barnItems.Add(feed, itemPair.Value);
            }
            else if (itemPair.Key is Fruit fruit)
            {
                fruits.Add(fruit, itemPair.Value);
                siloItems.Add(fruit, itemPair.Value);
            }
            else if (itemPair.Key is Product product)
            {
                products.Add(product, itemPair.Value);
                barnItems.Add(product, itemPair.Value);
            }
            else if (itemPair.Key is Tool tool)
            {
                tools.Add(tool, itemPair.Value);
                barnItems.Add(tool, itemPair.Value);
            }
        }
    }

    //put Barn and Silo on the map
    private void Start()
    {
        //instantiate the silo object
        GameObject siloObject = BuildingSystem.current.InitializeWithObject(siloPrefab, new Vector3(7.25f, -0.25f));
        //get the storage building component and save it
        Silo = siloObject.GetComponent<SampleBuilding>();
        //place the building onto the map
        Silo.Load();
        //initialize with items and a name
        Silo.Initialize(siloItems,"Silo");

        //instantiate the barn object
        GameObject barnObject = BuildingSystem.current.InitializeWithObject(barnPrefab, new Vector3(6f, -0.25f));
        //get the storage building component and save it
        Barn = barnObject.GetComponent<SampleBuilding>();
        //place the building onto the map
        Barn.Load();
        //initialize with items and a name
        Barn.Initialize(barnItems,"Barn");
    }

    /*
     * Get amount of an item that the player has
     */
    public int GetAmount(CollectibleItem item)
    {
        //initialize the amount with default value
        int amount = 0;
        //determine the type of an object requested
        if (item is AnimalProduct animalProduct)
        {
            //try get the amount
            animalProducts.TryGetValue(animalProduct, out amount);
        }
        else if (item is Crop crop)
        {
            crops.TryGetValue(crop, out amount);
        }
        else if (item is Feed feed)
        {
            feeds.TryGetValue(feed, out amount);
        }
        else if (item is Fruit fruit)
        {
            fruits.TryGetValue(fruit, out amount);
        }
        else if (item is Product product)
        {
            products.TryGetValue(product, out amount);
        }
        else if (item is Tool tool)
        {
            tools.TryGetValue(tool, out amount);
        }

        //return the amount
        return amount;
    }

    /*
     * Check if the player has enough of an item
     * @returns true if the amount the player has is more or equal to the amount required
     */
    public bool IsEnoughOf(CollectibleItem item, int amount)
    {
        return GetAmount(item) >= amount;
    }
}
