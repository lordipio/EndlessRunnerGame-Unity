using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoinAndGemHandler : MonoBehaviour
{
    #region Events And Delegates
    public delegate void GrabedCoins();
    public static GrabedCoins grabedCoinsEvent;
    #endregion

    #region Tags
    string gemTag = "Gem";
    string silverCoinTag = "SilverCoin";
    string goldenCoin = "GoldenCoin";
    #endregion

    #region TotalCoinCounter
    [Header("CointCounter:")]
    [Space]
    [Space]
    //[SerializeField]
    //TotalCoinCounterScriptableObject totalCoinCounter;
    //[SerializeField]
    //TotalCoinCounterScriptableObject currentCoinCounter;
    [SerializeField]
    GameObject coinCounterUIPrefab;
    GameObject coinCounterUI;
    [SerializeField]
    TMP_Text coinCounterTextUI;
    int currentCoinCounter = 0;
    #endregion

    #region CoinEffect
    [Header("CointEffect:")]
    [Space]
    [Space]
    [SerializeField]
    GameObject coinEffectPrefab;
    GameObject coinEffect;
    List<GameObject> coinEffectList = new List<GameObject>();
    #endregion

    #region MapGenerator
    MapGenerator mapGenerator;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //coinCounterUI = Instantiate<GameObject>(coinCounterUIPrefab);
        int zero = 0;
        coinCounterTextUI.text = zero.ToString();
        if (!MapGenerator.Instance)
        {
            throw new System.Exception("MapGenerator is not found!");
        }
        mapGenerator = MapGenerator.Instance;

        MapGenerator.DestroyEvent += CoinEffectDestroyer;
        GameManager.characterDiedEvent += CharacterDied;
        coinEffect = Instantiate<GameObject>(coinEffectPrefab);
        coinEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollidedWith(Collider2D collider)
    {
        collider.gameObject.SetActive(false);
        if (collider.tag.CompareTo(gemTag) == 0)
        {
            currentCoinCounter += 10;
        }

        if (collider.tag.CompareTo(silverCoinTag) == 0)
        {
            currentCoinCounter += 1;
        }

        if (collider.tag.CompareTo(goldenCoin) == 0)
        {
            currentCoinCounter += 5;
        }

        coinCounterTextUI.text = currentCoinCounter.ToString();
        coinEffect.transform.position = CharacterSpawner.Instance.Character.transform.position;
        coinEffect.SetActive(true);
        coinEffect.GetComponent<Animator>().SetBool("IsAnimationOver", false);
        grabedCoinsEvent?.Invoke();
    }


    void CoinEffectDestroyer()
    {
        if (coinEffectList.Count != 0)
            mapGenerator.DestroyOutofBoundsGameObject(coinEffectList);
    }

    void CharacterDied()
    {
        int totalCoinCounter = SaveAndLoadSystem.LoadTotalCoinsData().TotalCoins;
        coinCounterUIPrefab.SetActive(false);
        DontDestroyOnLoad(coinCounterUIPrefab);

        SaveAndLoadSystem.SaveTotalCoins(totalCoinCounter + currentCoinCounter);
        currentCoinCounter = 0;
        print(SaveAndLoadSystem.LoadTotalCoinsData().TotalCoins);
        MapGenerator.DestroyEvent -= CoinEffectDestroyer;
    }
}
