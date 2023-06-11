using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadIntroScene : MonoBehaviour
{
    public GameObject _LoadingScreen;
    public Image _LoadingBarFill;
    public void LoadScene()
    {
        _ = StartCoroutine(LoadSceneAsync(1));
    }

    IEnumerator LoadSceneAsync(int i)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            _LoadingBarFill.fillAmount = progressValue;
            yield return null;

        }
    }
}
