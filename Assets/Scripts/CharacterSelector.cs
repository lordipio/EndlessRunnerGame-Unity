using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{

    [SerializeField]
    Button leftArrowButton;

    [SerializeField]
    Button rightArrowButton;

    [SerializeField]
    Button purchaseButton;

    [SerializeField]
    Sprite purchasedSprite;

    [SerializeField]
    Sprite lockSprite; 

    [SerializeField]
    TMP_Text priceText;

    [SerializeField]
    bool[] unlockedCharacters;

    [SerializeField]
    Button backButton;

    [SerializeField]
    TMP_Text coinCounter;

    [SerializeField]
    GameObject mainMenu;

    [SerializeField]
    CharacterScriptableObject[] characters;

    [SerializeField]
    Image characterImage;

    [SerializeField]
    TMP_Text characterText;

    int currentCharacter = 1;

    int remainingCoins;

    int selectedCharacter = 1; //starts from 1

    SelectedCharacterData selectedCharacterDataFile;
    TotalCoinsData totalCoinsData;

    string selectedName = "Selected";
    string unlockedName = "Unlocked";

    void OnEnable()
    {
        //characters[0].isUnlocked = true;
        //characters[1].isUnlocked = false;
        //StartCoroutine(TryGetLoadedDataFile());
        //SaveAndLoadSystem.InitialSave();
        //if (SaveAndLoadSystem.Load() == null)
        //    SaveAndLoadSystem.InitialSave();
        //SaveAndLoadSystem.SaveCharacter(selectedCharacter);
        //SaveAndLoadSystem.SaveTotalCoins(2000);
        //dataFile = SaveAndLoadSystem.Load();
        SaveAndLoadHandler();

        selectedCharacter = selectedCharacterDataFile.SelectedCharacter;
        if (characters[0].isUnlocked && selectedCharacter - 1 != 0)
        {
            Unlock(1);
        }
        if (!characters[0].isUnlocked)
            characterText.text = characters[0].price.ToString();
        if (selectedCharacter - 1 == 0)
            SelectCharacter();


        coinCounter.text = totalCoinsData.TotalCoins.ToString();
        //if (SaveAndLoadSystem.Load() == null)
        //    print("DataFile that is loaded is null");
        //dataFile = SaveAndLoadSystem.Load();
        //SaveAndLoadSystem.SaveCharacter(selectedCharacter);
        //SaveAndLoadSystem.SaveTotalCoins(2000);
        //if (dataFile == null)
        //    print("DataFile is null");
        //selectedCharacter = dataFile.SelectedCharacter;
        //if (characters[0].isUnlocked && selectedCharacter - 1 != 0)
        //{
        //    Unlock(1);
        //}
        //if (!characters[0].isUnlocked)
        //    characterText.text = characters[0].price.ToString();
        //if (selectedCharacter - 1 == 0)
        //    SelectCharacter();


        //coinCounter.text = dataFile.TotalCoins.ToString();

        purchaseButton.onClick.AddListener(CheckPurchased);
        leftArrowButton.onClick.AddListener(PreviouseCharacter);
        rightArrowButton.onClick.AddListener(NextCharacter);
        backButton.onClick.AddListener(BackButton);
    }

    void SaveAndLoadHandler()
    {
        totalCoinsData = SaveAndLoadSystem.LoadTotalCoinsData();
        selectedCharacterDataFile = SaveAndLoadSystem.LoadSelectedCharacter();
    }

    //IEnumerator TryGetLoadedDataFile()
    //{
    //    while (dataFile == null)
    //    {
    //        yield return null;
    //        SaveAndLoadSystem.SaveSelectedCharacter(selectedCharacter);
    //        SaveAndLoadSystem.SaveTotalCoins(2000);
    //        dataFile = SaveAndLoadSystem.Load();
    //        print((dataFile == null));
    //    }
    //    selectedCharacter = dataFile.SelectedCharacter;
    //    if (characters[0].isUnlocked && selectedCharacter - 1 != 0)
    //    {
    //        Unlock(1);
    //    }
    //    if (!characters[0].isUnlocked)
    //        characterText.text = characters[0].price.ToString();
    //    if (selectedCharacter - 1 == 0)
    //        SelectCharacter();


    //    coinCounter.text = dataFile.TotalCoins.ToString();

    //}

    void CheckPurchased()
    {
        if (characters[currentCharacter - 1].isUnlocked)
        {
            SelectCharacter();
        }


        if (totalCoinsData.TotalCoins >= characters[currentCharacter - 1].price && !characters[currentCharacter - 1].isUnlocked)
        {
            Unlock(currentCharacter);
        }


    }

    void Unlock(int characterNum)
    {
        purchaseButton.GetComponent<Image>().sprite = purchasedSprite;
        purchaseButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        purchaseButton.GetComponentInChildren<TMP_Text>().text = unlockedName;

        remainingCoins = totalCoinsData.TotalCoins - characters[characterNum - 1].price;
        coinCounter.text = remainingCoins.ToString();
        characters[currentCharacter - 1].isUnlocked = true;
        SaveAndLoadSystem.SaveTotalCoins(remainingCoins);
    }

    void PreviouseCharacter()
    {
        
        if (currentCharacter == 1)
            currentCharacter = characters.Length;
        
        else
            currentCharacter--;

        UpdateCharacterAndButton();


    }

    void NextCharacter()
    {
        if (currentCharacter == characters.Length)
            currentCharacter = 1;
        else
            currentCharacter++;

        UpdateCharacterAndButton();

    }

    private void SelectCharacter()
    {
        selectedCharacter = currentCharacter;
        characterText.text = selectedName;
        //Sprite sp = purchaseButton.GetComponent<Image>().sprite;
        purchaseButton.GetComponent<Image>().sprite = purchasedSprite;
        purchaseButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        SaveAndLoadSystem.SaveSelectedCharacter(selectedCharacter);

    }

    void BackButton()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
        RemoveAllListenerFromAllButtons();
    }

    void UpdateCharacterAndButton() // when player press next or previouse buttons
    {
        characterImage.sprite = characters[currentCharacter - 1].CharacterIdlePose;

        if (characters[currentCharacter - 1].isUnlocked && currentCharacter != selectedCharacter)
        {
            characterText.text = unlockedName;
            purchaseButton.GetComponent<Image>().sprite = purchasedSprite;
        }

        if (!characters[currentCharacter - 1].isUnlocked)
        {
            print("Not Unlocked");
            characterText.text = characters[currentCharacter - 1].price.ToString();
            purchaseButton.GetComponent<Image>().sprite = lockSprite;
        }

        if (selectedCharacter == currentCharacter)
            SelectCharacter();
    }

    void RemoveAllListenerFromAllButtons()
    {
        purchaseButton.onClick.RemoveAllListeners();
        leftArrowButton.onClick.RemoveAllListeners();
        rightArrowButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }
}
