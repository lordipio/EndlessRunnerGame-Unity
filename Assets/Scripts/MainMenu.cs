using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    #region Buttons
    [SerializeField]
    Button playButton;

    [SerializeField]
    Button selectCharacterButton;

    [SerializeField]
    Button exitButton;

    [SerializeField]
    Button backButton;

    [SerializeField]
    GameObject workInProgressPage;

    [SerializeField]
    GameObject loadScreenPage;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        workInProgressPage.gameObject.SetActive(false);
        playButton.onClick.AddListener(PlayButtonClicked);
        selectCharacterButton.onClick.AddListener(SelectCharacterButtonClicked);
        backButton.onClick.AddListener(BackButton);
        exitButton.onClick.AddListener(ExitButton);
    }


    void PlayButtonClicked()
    {
        
        Instantiate<GameObject>(loadScreenPage);
        //gameObject.SetActive(false);
        playButton.onClick.RemoveAllListeners();
        selectCharacterButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
        SceneManager.LoadScene(1);
        //gameObject.SetActive(false);
        //LoadScreen.loadscreen.LoadScene(1);
        //StartCoroutine(TEST());
    }

    IEnumerator TEST()
    {
        yield return new WaitForSeconds(1f);

    }

    void SelectCharacterButtonClicked()
    {
        workInProgressPage.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    void ExitButton()
    {
        playButton.onClick.RemoveAllListeners();
        selectCharacterButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
        Application.Quit();
    }

    void BackButton()
    {
        gameObject.SetActive(true);
        workInProgressPage.gameObject.SetActive(false);
    }
}
