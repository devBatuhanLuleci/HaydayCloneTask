using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler
{
    //tab group this button belongs to
    public TabGroup tabGroup;
    //tab button background
    [NonSerialized] public Image background;

    private void Awake()
    {
        //initialize the background Image
        background = GetComponent<Image>();
        //subscribe the button
        tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //on click select the tab
        tabGroup.OnTabSelected(this);
    }
}
