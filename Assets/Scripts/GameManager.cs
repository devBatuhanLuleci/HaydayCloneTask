using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singletone pattern
    public static GameManager current;

    //save the canvas
    public GameObject canvas;

    private void Awake()
    {
        //initialize fields
        current = this;
        
        //initialize
        ShopItemDrag.canvas = canvas.GetComponent<Canvas>();
    }
}
