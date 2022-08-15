using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinCounter")]
public class TotalCoinCounterScriptableObject : ScriptableObject
{
    [HideInInspector]
    public int coins = 0;
}
