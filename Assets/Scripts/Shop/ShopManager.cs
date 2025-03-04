using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //singletone pattern
    public static ShopManager current;
    
    //currency sprites for initialization
    public static Dictionary<CurrencyType, Sprite> currencySprites = new Dictionary<CurrencyType, Sprite>();
    [SerializeField] private List<Sprite> sprites;

    //fields for animating the shop window
    private RectTransform rt;
    private RectTransform prt;
    private bool opened;

    //prefab for displaying shop info
    [SerializeField] private GameObject itemPrefab;
    //al shop items
    private Dictionary<ObjectType, List<ShopItem>> shopItems = new Dictionary<ObjectType, List<ShopItem>>(1);

    //tabs with items
    [SerializeField] public TabGroup shopTabs;

    private void Awake()
    {
        //initialize fields
        current = this;

        rt = GetComponent<RectTransform>();
        prt = transform.parent.GetComponent<RectTransform>();
        
        //subscribe method to an event
        EventManager.Instance.AddListener<LevelChangedGameEvent>(OnLevelChanged);
    }

    private void Start()
    {
        //add sprites
        currencySprites.Add(CurrencyType.Coins, sprites[0]);
        currencySprites.Add(CurrencyType.Crystals, sprites[1]);
        
        //load shop items and initialize the shop
        Load();
        Initialize();
        
        //disable the shop window so the tabs are not visible
        gameObject.SetActive(false);
    }

    private void Load()
    {
        //load every shop item from resources
        ShopItem[] items = Resources.LoadAll<ShopItem>("Shop");
        
        //initialize the dictionary
        shopItems.Add(ObjectType.ProductionBuildings, new List<ShopItem>());


        //add all shop items to the dictionary
        foreach (var item in items)
        {
            shopItems[item.Type].Add(item);
        }
    }

    private void Initialize()
    {
        for (int i = 0; i < shopItems.Keys.Count; i++)
        {
            foreach (var item in shopItems[(ObjectType)i])
            {
                //create an item holder and initialize it
                GameObject itemObject = Instantiate(itemPrefab, shopTabs.objectsToSwap[i].transform);
                itemObject.GetComponent<ShopItemHolder>().Initialize(item);
            }
        }
    }

    private void OnLevelChanged(LevelChangedGameEvent info)
    {
        //when the player gets a new level
        for (int i = 0; i < shopItems.Keys.Count; i++)
        {
            ObjectType key = shopItems.Keys.ToArray()[i];
            for (int j = 0; j < shopItems[key].Count; j++)
            {
                ShopItem item = shopItems[key][j];

                if (item.Level == info.newLvl)
                {
                    //unlock item if its level matches the new one
                    shopTabs.transform.GetChild(i).GetChild(j).GetComponent<ShopItemHolder>().UnlockItem();
                }
            }
        }
    }
    
    public void ShopButton_Click()
    {
        //animation time
        float time = 0.2f;
        if (!opened)
        {
            //open the shop
            LeanTween.moveX(prt, prt.anchoredPosition.x + rt.sizeDelta.x, time);
            opened = true;
            gameObject.SetActive(true);
        }
        else
        {
            //close the shop
            LeanTween.moveX(prt, prt.anchoredPosition.x - rt.sizeDelta.x, time)
                .setOnComplete(delegate()
                {
                    gameObject.SetActive(false);
                });
            opened = false;
        }
    }

    //make the shop close when the player click on the area
    private bool dragging;

    public void OnBeginDrag()
    {
        dragging = true;
    }

    public void OnEndDrag()
    {
        dragging = false;
    }

    public void OnPointerClick()
    {
        if (!dragging)
        {
            ShopButton_Click();
        }
    }
}
