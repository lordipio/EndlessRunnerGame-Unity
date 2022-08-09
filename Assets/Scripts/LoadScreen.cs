using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScreen : MonoBehaviour
{
    public static LoadScreen loadscreen;

    [SerializeField]
    GameObject loadScreen;
    [SerializeField]
    Slider loadingBar;

    private void Awake()
    {
        loadScreen.SetActive(false);
        if (loadscreen && loadscreen != this)
        {
            Destroy(gameObject);
            return;
        }
        loadscreen = this;
    }

    public void LoadScene(int sceneID)
    {
        loadScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneID));
    }


    IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progressValue;
            yield return null;
        }
    }
}
