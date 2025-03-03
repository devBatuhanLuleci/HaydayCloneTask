using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : PlaceableObject
{
    public override void Place()
    {
        base.Place();
        
        //add timer to the object
        Timer timer = gameObject.AddComponent<Timer>();
        //initialize timer - name of the process, starting time now, duration 3 minutes
        timer.Initialize("Building", DateTime.Now, TimeSpan.FromMinutes(3));
        //start the timer
        timer.StartTimer();
        //when the timer finished destroy it
        timer.TimerFinishedEvent.AddListener(delegate
        {
            Destroy(timer);
        });
    }

    private void OnMouseUpAsButton()
    {
        //on object click - display the tooltip
        TimerTooltip.ShowTimer_Static(gameObject);
    }
}
