using UnityEngine;
using UnityEngine.EventSystems;

public class PanZoom : MonoBehaviour
{
    //singletone to access script from others
    public static PanZoom current;
    
    //camera scroll limits
    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;
    [SerializeField] private float bottomLimit;
    [SerializeField] private float upperLimit;

    //zoom limits
    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;
    
    //camera with which we view the map
    private Camera cam;

    //map scrolling allowed (no UI overlap)
    private bool moveAllowed;
    //last touch position
    private Vector3 touchPos;

    //transform of the object the camera has to follow
    private Transform objectToFollow;
    //bound of the object we follow
    private Bounds objectBounds;
    //previous position of the object
    private Vector3 prevPos;

    private void Awake()
    {
        //initialize the camera (has to be attached to the camera)
        cam = GetComponent<Camera>();
        //initialize singletone
        current = this;
    }

    private void Update()
    {
        //if there is an object to follow
        if (objectToFollow != null)
        {
            Vector3 objPos = cam.WorldToViewportPoint(objectToFollow.position + objectBounds.max);
            if (objPos.x >= 0.7f || objPos.x <= 0.3f || objPos.y >= 0.7f || objPos.y <= 0.3f)
            {
                Vector3 pos = cam.ScreenToWorldPoint(objectToFollow.position);
                Vector3 direction = pos - prevPos;
                cam.transform.position += direction;
                prevPos = pos;
                
                transform.position = new Vector3
                (
                    Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                    Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
                    transform.position.z
                );
            }
            else
            {
                Vector3 pos = cam.ScreenToWorldPoint(objectToFollow.position);
                prevPos = pos;
            }
            return;
        }
        
        //if we detect touch input
        if (Input.touchCount > 0)
        {
            //if there are two 
            if (Input.touchCount == 2)
            {
                //get both fingers' touches
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                //check if they don't overlap UI
                if (EventSystem.current.IsPointerOverGameObject(touchOne.fingerId)
                    || EventSystem.current.IsPointerOverGameObject(touchZero.fingerId))
                {
                    //if they do - return, we can't zoom
                    return;
                }

                //calculate last positions of the touches
                Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;

                //calculate last distance between touches
                float distTouch = (touchZeroLastPos - touchOneLastPos).magnitude;
                //calculate current distance between touches
                float currentDistTouch = (touchZero.position - touchOne.position).magnitude;

                //calculate difference between last distance and current distance between fingers
                float difference = currentDistTouch - distTouch;
                
                //zooming (0.01f - for smoothing the zoom)
                Zoom(difference * 0.01f);
            }
            else
            {
                //get touch with index 0 (first touch)
                Touch touch = Input.GetTouch(0);
                
                switch (touch.phase)
                {
                    //touch begun
                    case TouchPhase.Began:
                        //check if the touch overlaps UI
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            moveAllowed = false;
                        }
                        else
                        {
                            moveAllowed = true;
                        }

                        //save touch position
                        touchPos = cam.ScreenToWorldPoint(touch.position);
                        break;
                    //touch moved
                    case TouchPhase.Moved:
                        if (moveAllowed)
                        {
                            //calculate direction to move the camera
                            Vector3 direction = touchPos - cam.ScreenToWorldPoint(touch.position);
                            //move the camera in that direction
                            cam.transform.position += direction;

                            //clamp the move position between limits
                            transform.position = new Vector3
                                (
                                Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                                Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
                                transform.position.z
                                );
                        }
                        break;
                }
            }
        }
    }
    
    private void Zoom(float increment)
    {
        //zoom by changing the size of the camera
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, zoomMin, zoomMax);
    }

    //make the camera follow an object
    public void FollowObject(Transform objToFollow)
    {
        //initialize fields
        objectToFollow = objToFollow;
        objectBounds = objectToFollow.GetComponent<PolygonCollider2D>().bounds;
        
        //save previous position of the camera to avoid jumpy movement
        prevPos = cam.ScreenToWorldPoint(Vector3.zero);
    }

    //stop the camera from following an object
    public void UnfollowObject()
    {
        objectToFollow = null;
    }

    //move the camera towards some position
    public void Focus(Vector3 position)
    {
        //initialize new position
        Vector3 newPos = new Vector3(position.x, position.y, transform.position.z);
        //move the camera towards this position
        LeanTween.move(gameObject, newPos, 0.2f);
        
        //clamp the camera position
        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
            Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
            transform.position.z
        );
        
        touchPos = transform.position;
    }
    
    private void OnDrawGizmos()
    {
        //display a cube to indicate area to scroll
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3
        ((rightLimit - Mathf.Abs(leftLimit) / 2.0f),
            (upperLimit - Mathf.Abs(bottomLimit) / 2.0f)),
            new Vector3(rightLimit - leftLimit, upperLimit - bottomLimit));
    }
}
