using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameLogoIntroScript : MonoBehaviour
{
    public GameObject GameLogoObject;
    public VideoPlayer _VideoPlayer;
    bool _startFade;
    public CanvasGroup _CanvasToFade;
    LoadWorldScene _LoadScene;

    // Start is called before the first frame update
    void Start()
    {
        GameLogoObject = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;
        _VideoPlayer = transform.GetChild(1).gameObject.GetComponent<VideoPlayer>();
        _CanvasToFade = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<CanvasGroup>();
        StartCoroutine(PlayDelayedVideoPlayer());
        StartCoroutine(PlayDelayedLogo());
        _LoadScene = new LoadWorldScene();
    }

    // Update is called once per frame
    void Update()
    {
        FadeIN();
        
        
    }

    private void FixedUpdate()
    {

    }
    private void LateUpdate()
    {
        StartCoroutine(FadeScreen());
        StartCoroutine(LoadNextScene());
    }
    void FadeIN()
    {
        if (_startFade == true)
        {
            if (_CanvasToFade.alpha < 1)
            {
                _CanvasToFade.alpha += Time.deltaTime;
                if (_CanvasToFade.alpha >= 1)
                {
                    _startFade = false;
                }
            }
        }
    }
    void FadeOUT()
    {
        if (_CanvasToFade.alpha < 0.1f)
        {
            _CanvasToFade.alpha += Time.deltaTime;
        }
        
    }
    IEnumerator PlayDelayedVideoPlayer()
    {
        yield return new WaitForSeconds(3f);
        _VideoPlayer.gameObject.SetActive(true);
        _startFade = true; 
    }
    IEnumerator PlayDelayedLogo()
    {
        yield return new WaitForSeconds(3.1f);
        GameLogoObject.SetActive(true);
    }
    IEnumerator FadeScreen()
    {
        
        yield return new WaitForSeconds(12);
        FadeOUT();
        GameLogoObject.SetActive(false);
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(16);
        _LoadScene.LoadNextScene(1);
    }
}
