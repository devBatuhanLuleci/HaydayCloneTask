using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorTooltip : MonoBehaviour
{
    //static instance of the tooltip
    private static CollectorTooltip instance;
    
    //camera
    [SerializeField] private Camera uiCamera;
    //item holders on the tooltip
    [SerializeField] private GameObject collectorHolder;
    
    private void Awake() 
    {
        //initialize the static instance
        instance = this;
        //disable the object
        transform.parent.gameObject.SetActive(false);
    }

    private void ShowTooltip(GameObject caller) 
    {
        //initialize the collector holder
        collectorHolder.GetComponent<Collector>().Initialize(caller.GetComponent<PlaceableObject>());
        
        //position the tooltip
        //subtract the camera position to get the correct position (since the camera can move)
        Vector3 position = caller.transform.position - uiCamera.transform.position;
        //convert the position; first - from local to world, second - from world to screen
        position = uiCamera.WorldToScreenPoint(uiCamera.transform.TransformPoint(position));
        //assign the new position
        transform.position = position;
        
        //set the whole tooltip visible (the script is attached to the child object)
        transform.parent.gameObject.SetActive(true);
    }
    
    public void HideTooltip() 
    {
        //disable the whole tooltip
        transform.parent.gameObject.SetActive(false);
    }

    //static tooltip show method
    public static void ShowTooltip_Static(GameObject caller) 
    {
        instance.ShowTooltip(caller);
    }

    //static tooltip hide method
    public static void HideTooltip_Static() 
    {
        instance.HideTooltip();
    }
}
