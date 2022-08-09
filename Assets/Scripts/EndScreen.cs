using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    float endScreenDelay;

    [SerializeField]
    Button replayButton;

    [SerializeField]
    Button homeButton;

    [SerializeField]
    Animator endScreenAnimator;

    [SerializeField]
    GameObject loadScreenPage;

    // Start is called before the first frame update
    void Start()
    {

        gameObject.SetActive(false);
        CharacterHandler.characterDiedEvent += EndScreenIsReady;
        replayButton.onClick.AddListener(ReplayButtonClicked);
        homeButton.onClick.AddListener(HomeButtonClicked);
    }



    void EndScreenIsReady() 
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowEndScreen());
        CharacterHandler.characterDiedEvent -= EndScreenIsReady;
    }

    IEnumerator ShowEndScreen()
    {
        yield return new WaitForSeconds(endScreenDelay);


        endScreenAnimator.SetBool("EnableEndScreen", true);
    }

    void ReplayButtonClicked()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        replayButton.onClick.RemoveAllListeners();

    }

    void HomeButtonClicked()
    {
        Vector2 CameraPos = Camera.main.transform.position;
        Instantiate<GameObject>(loadScreenPage, new Vector3(CameraPos.x, CameraPos.y, 0f), Camera.main.transform.rotation);
        gameObject.SetActive(false);
        replayButton.onClick.RemoveAllListeners();
        homeButton.onClick.RemoveAllListeners();
        //StartCoroutine(TEST());
        SceneManager.LoadScene(0);
    }


    IEnumerator TEST()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
        gameObject.SetActive(false);

    }

}
