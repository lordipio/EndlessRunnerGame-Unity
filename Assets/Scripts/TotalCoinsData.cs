using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TotalCoinsData
{
    [HideInInspector]
    public int TotalCoins
    {
        get { return totalCoins; }
        set { totalCoins = value; }
    }

    int totalCoins = 0;

    public void SetTotalCoin(int totalCoins)
    {
        this.totalCoins = totalCoins;
    }

}
