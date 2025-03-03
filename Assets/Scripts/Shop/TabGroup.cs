using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    //tab button states - Idle and Active
    public Sprite tabIdle;
    public Sprite tabActive;

    //all buttons that belong to this group
    public List<TabButton> tabButtons = new List<TabButton>();
    //objects to swap when switching tabs
    public List<GameObject> objectsToSwap = new List<GameObject>();

    //selected tab
    [NonSerialized] public TabButton selectedTab;

    private void Start()
    {
        //select the first tab on start
        OnTabSelected(tabButtons[0]);
    }

    public void Subscribe(TabButton button)
    {
        //add button to the list
        tabButtons.Add(button);
    }

    private void ResetTabs()
    {
        foreach (var button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }

            //set the background to Idle
            button.background.sprite = tabIdle;
        }
    }

    public void OnTabSelected(TabButton button)
    {
        //set selected button
        selectedTab = button;
        //reset other tabs
        ResetTabs();
        //change the sprite
        button.background.sprite = tabActive;

        //enable the right tab and disable other
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }
}
