using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Carrot", menuName = "GameObjects/StorageItems/Carrot")]
public class Carrot : Producible
{
    public Sprite growingCarrot;
    public Sprite readyCarrot;

    private new void OnValidate()
    {
        base.OnValidate();

        ItemsNeeded = new Dictionary<CollectibleItem, int>() {{this, 1}};
    }
}
