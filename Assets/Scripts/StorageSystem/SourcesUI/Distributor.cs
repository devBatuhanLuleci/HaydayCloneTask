using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Distributor : UIDrag
{
    //items required to start the production
    private Dictionary<CollectibleItem, int> itemsNeeded;
    //item which needs to be produced
    private CollectibleItem itemToProduce;

    //generic initialize method
    public void Initialize<T>(PlaceableObject src, T item, int amount = -1)
        where T : Producible
    {
        //initialize the UIDrag base
        base.Initialize(src);

        //initialize fields
        itemToProduce = item;
        itemsNeeded = item.ItemsNeeded;

        //get the UI components
        transform.Find("Icon").GetComponent<Image>().sprite = item.Icon;
        Transform amountTr = transform.Find("Amount");

        //if the amount is -1 we don't need to display the amount
        if (amount == -1)
        {
            //disable the amount
            amountTr.gameObject.SetActive(false);
        }
        else
        {
            //enable the amount
            amountTr.gameObject.SetActive(false);
            //set the text
            amountTr.GetComponent<TextMeshProUGUI>().text = amount.ToString();
        }
        
        //enable the whole object
        gameObject.SetActive(true);
    }

    protected override void OnCollide(PlaceableObject collidedSource)
    {
        //start production
        collidedSource.GetComponent<ISource>().Produce(itemsNeeded, itemToProduce);
    }
}
