using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public int coinCount;

    private void Awake()
    {
        Instance = this;
        coinCount = 0;
    }

    public void AddCoin()
    {
        coinCount++;
    }

    public void RemoveCoin()
    {
        coinCount--;
    }
}
