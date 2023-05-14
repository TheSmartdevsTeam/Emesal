using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class GameLogoIntroScript : MonoBehaviour
{
    public GameObject _LogoSceneMasterObject;
    public GameObject GameLogoObject;
    public VideoPlayer LogoVideoPlayer;
    bool _startFade;
    public CanvasGroup _CanvasToFade;

    // Start is called before the first frame update
    void Start()
    {
        _LogoSceneMasterObject = GameObject.FindGameObjectWithTag("LogoSceneMasterObject");
        GameLogoObject = _LogoSceneMasterObject.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).gameObject;
        LogoVideoPlayer = _LogoSceneMasterObject.transform.GetChild(2).gameObject.GetComponent<VideoPlayer>();
        _CanvasToFade = _LogoSceneMasterObject.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<CanvasGroup>();
        StartCoroutine(PlayDelayedVideoPlayer());
        StartCoroutine(PlayDelayedLogo());
    }

    // Update is called once per frame
    void Update()
    {
        if(_startFade == true)
        {
            if(_CanvasToFade.alpha < 1)
            {
                _CanvasToFade.alpha += Time.deltaTime;
                if(_CanvasToFade.alpha >=1)
                {
                    _startFade = false;
                }
            }
        }
    }

    IEnumerator PlayDelayedVideoPlayer()
    {
        yield return new WaitForSeconds(2.2f);
        LogoVideoPlayer.gameObject.SetActive(true);
        
        _startFade = true;
        
    }
    IEnumerator PlayDelayedLogo()
    {
        yield return new WaitForSeconds(2.4f);
        GameLogoObject.SetActive(true);

    }
}
