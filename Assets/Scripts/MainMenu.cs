using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    #region Buttons
    [Header("Buttons:")]
    [Space]
    [Space]
    [SerializeField]
    Button playButton;

    [SerializeField]
    Button exitButton;



    [SerializeField]
    Button selectCharacterButton;
    #endregion

    #region Pages
    [Header("Pages:")]
    [Space]
    [Space]
    [SerializeField]
    GameObject workInProgressPage;

    [SerializeField]
    GameObject loadScreenPagePrefab;
    GameObject loadScreenPage;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        AssignInitialValues();
    }

    private void Awake()
    {

    }

    void AssignInitialValues()
    {
        if (!SaveAndLoadSystem.DoesTotalCoinsDataExist())
            SaveAndLoadSystem.SaveTotalCoins(0);
        if (!SaveAndLoadSystem.DoesSelectedCharacterDataExist())
            SaveAndLoadSystem.SaveSelectedCharacter(1);

        loadScreenPage = Instantiate<GameObject>(loadScreenPagePrefab);
        loadScreenPage.SetActive(false);
        workInProgressPage.gameObject.SetActive(false);
        playButton.onClick.AddListener(PlayButtonClicked);
        selectCharacterButton.onClick.AddListener(SelectCharacterButtonClicked);
        exitButton.onClick.AddListener(ExitButton);
    }

    void PlayButtonClicked()
    {
        RemoveAllListenerForAllButtons();
        loadScreenPage.SetActive(true);
        gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }


    void SelectCharacterButtonClicked()
    {
        //print("Checked");
        workInProgressPage.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    void ExitButton()
    {
        RemoveAllListenerForAllButtons();
        Application.Quit();
    }

    void BackButton()
    {
        gameObject.SetActive(true);
        workInProgressPage.gameObject.SetActive(false);
    }

    void RemoveAllListenerForAllButtons()
    {
        playButton.onClick.RemoveAllListeners();
        selectCharacterButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }
}
