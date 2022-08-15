using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    #region Buttons
    [Header("Buttons: ")]
    [Space]
    [Space]
    
    [SerializeField]
    Button replayButton;

    [SerializeField]
    Button homeButton;
    #endregion

    #region EndScreen
    [Header("EndScreen: ")]
    [Space]
    [Space]

    [SerializeField]
    Animator endScreenAnimator;
    #endregion

    #region LoadScreen
    [Header("LoadScreen: ")]
    [Space]
    [Space]
    [SerializeField]
    GameObject loadScreenPagePrefab;
    GameObject loadScreenPage;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        AssignInitialValues();
    }
    
    void AssignInitialValues()
    {
        GameManager.characterDiedEvent += ReadyUpEndScreen;
        replayButton.onClick.AddListener(ReplayButtonClicked);
        homeButton.onClick.AddListener(HomeButtonClicked);
    }

    void ReadyUpEndScreen() 
    {
        Vector2 CameraPos = Camera.main.transform.position;
        loadScreenPage = Instantiate<GameObject>(loadScreenPagePrefab, new Vector3(CameraPos.x, CameraPos.y, 0f), Camera.main.transform.rotation);
        loadScreenPage.SetActive(false);
        gameObject.SetActive(true);
        GameManager.characterDiedEvent -= ReadyUpEndScreen;
    }



    void ReplayButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        RemoveAllListenerForAllButtons();
    }

    void HomeButtonClicked()
    {
        loadScreenPage.SetActive(true);
        gameObject.SetActive(false);
        RemoveAllListenerForAllButtons();
        SceneManager.LoadScene(0);
    }
    
    void RemoveAllListenerForAllButtons()
    {
        replayButton.onClick.RemoveAllListeners();
        homeButton.onClick.RemoveAllListeners();
    }
}
