using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop", menuName = "GameObjects/StorageItems/Crop")]
public class Crop : Producible
{
    public Sprite growingCrop;
    public Sprite readyCrop;

    private new void OnValidate()
    {
        base.OnValidate();

        ItemsNeeded = new Dictionary<CollectibleItem, int>() {{this, 1}};
    }
}
