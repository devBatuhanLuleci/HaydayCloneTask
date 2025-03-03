using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectibleItem : ScriptableObject
{
    //properties of a collectible item
    public string Name;
    public string Description;
    public Sprite Icon;
    public int Level = 0;
}
