using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadWorldScene : MonoBehaviour
{
    public GameObject _LoadingScreen;
    public Image _LoadingBarFill;

    public void LoadNextSceneWithLoadingBar(int i)
    {
        StartCoroutine(LoadSceneAsync(i));
    }
    public void LoadNextScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    IEnumerator LoadSceneAsync(int i)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(i);
        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            _LoadingBarFill.fillAmount = progressValue;
            yield return null;
            
        }
    }

}
