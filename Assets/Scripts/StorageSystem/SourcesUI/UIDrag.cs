using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //save the canvas
    public static Canvas canvas;

    //to handle drag
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    //to enable and disable the mask
    protected Image img;

    //save the origin position to return to it
    private Vector3 originPos;
    private bool drag;

    //source which called this UI drag
    protected PlaceableObject source;

    public void Initialize(PlaceableObject src)
    {
        source = src;
    }

    private void Awake()
    {
        //get the components
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        img = transform.Find("Icon").GetComponent<Image>();
        
        //save the origin position
        originPos = rectTransform.anchoredPosition;
    }

    private void FixedUpdate()
    {
        // dragging
        if (drag)
        {
            //get the position and convert it to world point
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //raycast from that point
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.positiveInfinity);

            //if the ray hit something
            if (hit.collider != null)
            {
                //get the placeable object from the hit
                PlaceableObject selected = hit.transform.GetComponent<PlaceableObject>();

                //check if the types match
                if (selected.GetType() == source.GetType())
                {
                    //trigger collision
                    OnCollide(selected);
                }
            }
        }
    }

    protected virtual void OnCollide(PlaceableObject collidedSource) { }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //handle begin drag
        drag = true;
        canvasGroup.blocksRaycasts = false;
        //make the image not maskable so it's visible
        img.maskable = false;
        //disable the pan zoom so we don't accidentally move the map
        PanZoom.current.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //move the object considering the scale factor
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //handle end drag
        drag = false;
        canvasGroup.blocksRaycasts = true;
        //enable pan zoom back
        PanZoom.current.enabled = true;
        //make the image maskable again
        img.maskable = true;
        //return the object to origin position
        rectTransform.anchoredPosition = originPos;
    }
}
