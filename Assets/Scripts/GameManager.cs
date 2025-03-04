using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singletone pattern
    public static GameManager current;

    //save the canvas
    public GameObject canvas;

    private void Awake()
    {
        #if !UNITY_EDITOR
                Application.targetFrameRate = 60;
                QualitySettings.vSyncCount = 0;
        #endif
        //initialize fields
        current = this;

        //initialize
        ShopItemDrag.canvas = canvas.GetComponent<Canvas>();
        UIDrag.canvas = canvas.GetComponent<Canvas>();
    }
}
