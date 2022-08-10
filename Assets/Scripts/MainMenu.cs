using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    Button backButton;

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
    GameObject loadScreenPage;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        AssignInitialValues();
    }

    void PlayButtonClicked()
    {
        loadScreenPage = Instantiate<GameObject>(loadScreenPage);
        loadScreenPage.SetActive(true);
        RemoveAllListenerForAllButtons();
        gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    void AssignInitialValues()
    {
        workInProgressPage.gameObject.SetActive(false);
        playButton.onClick.AddListener(PlayButtonClicked);
        selectCharacterButton.onClick.AddListener(SelectCharacterButtonClicked);
        backButton.onClick.AddListener(BackButton);
        exitButton.onClick.AddListener(ExitButton);
    }

    void SelectCharacterButtonClicked()
    {
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
        backButton.onClick.RemoveAllListeners();
    }
}
