using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISource
{
    //the current state of the source
    State currentState { get; set; }
    //start production method
    void Produce(Dictionary<CollectibleItem, int> itemsNeeded, CollectibleItem itemToProduce);
    //collect the produce
    void Collect();
}
