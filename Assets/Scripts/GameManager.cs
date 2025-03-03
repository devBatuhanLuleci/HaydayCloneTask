using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public GameObject canvas;

    private void Awake()
    {
        current = this;
    }

    public void GetXP(int amount)
    {
        XPAddedGameEvent info = new XPAddedGameEvent(amount);
        EventManager.Instance.QueueEvent(info);
    }

    public void GetCoins(int amount)
    {
        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(amount, CurrencyType.Coins);
        
        EventManager.Instance.QueueEvent(info);
    }
}
