using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenuMasterScript : MonoBehaviour, IMyDataManager
{
    Animator _MainMenuCameraAnimator;
    AudioSource _MainMenuAudioSource;
    AudioClip _AudioClip;
    Renderer _GameTitle;
    Canvas _MainMenuCanvas;
    GameObject _UICamera;
    
    public bool _ClickSound;

    bool _Subtitles;
    public GameObject _SubtitlesGlowYes;
    public GameObject _SubtitlesGlowNo;

    public Scrollbar _MouseSpeed;

    bool _FullScreen;
    public GameObject _FullScreenGlowYes;
    public GameObject _FullScreenGlowNo;
    public TMP_Dropdown dropdown;
    int _ResolutionX;
    int _ResolutionY;
    float _Gamma;
    float _MasterVolume;
    float _MusicVolume;
    float _SpeachVolume;

    void Start()
    {
        _MainMenuCameraAnimator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        _MainMenuAudioSource = transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();
        _MainMenuCanvas = transform.GetChild(1).GetComponent<Canvas>();
        _AudioClip = Resources.Load("MagicButtonClick") as AudioClip;
        _UICamera = transform.GetChild(0).GetChild(1).gameObject;
        _ClickSound = false;
            
    }

    void Update()
    {
        #region CameraRotation for Animations
        if (OwlCallScript.OwlCall)
        {
            _MainMenuCameraAnimator.SetBool("OwlCall", true);
        }
        else
        {
            _MainMenuCameraAnimator.SetBool("OwlCall", false);
        }

        if (GrassNoiseScript.GrassNoise)
        {
            _MainMenuCameraAnimator.SetBool("GrassNoise", true);
        }
        else
        {
            _MainMenuCameraAnimator.SetBool("GrassNoise", false);
        }

        if (ThunderstormScript._LightningFlashBool)
        {
            _MainMenuCameraAnimator.SetBool("LightningFlash", true);
        }
        else
        {
            _MainMenuCameraAnimator.SetBool("LightningFlash", false);
        }
        #endregion
 
    }
    public void PlayClickSound()
    {
        _ClickSound = true;
        if (_ClickSound)
        {
            if(Resources.Load("UI/MagicButtonClick"))
            {
                _AudioClip = Resources.Load<AudioClip>("UI/MagicButtonClick");
                _MainMenuAudioSource.PlayOneShot(_AudioClip);
            }
            MainMenuButtonScript._ClickSound = false;
        }
    }
    public void onConfirmStartClick()
    {
        PlayClickSound();
        //gameObject.transform.parent.gameObject.SetActive(false);
        SceneManager.LoadScene(2);
    }
    public void OnExitClick()
    {
        Application.Quit();
    }
    public void OnInstagramButtonClick()
    {
        Application.OpenURL("https://www.instagram.com/smartdevsgames/");
    }
    public void OnYouTubeButtonClick()
    {
        Application.OpenURL("https://www.youtube.com/channel/UC5lzIbomC99UlgW53SUMSLA?Sub_Confirmation=1");
    }
    public void OnSubtitlesClick(bool value)
    {
        _Subtitles = value;
    }
    public void OnFullScreenClick(bool value)
    {
        _FullScreen = value;
    }
    public void OnChooseResolution()
    {
        if(dropdown.value == 0)
        {
            _ResolutionX = 1024;
            _ResolutionY = 768;
        }
        if (dropdown.value == 1)
        {
            _ResolutionX = 1280;
            _ResolutionY = 720;
        }
        if (dropdown.value == 2)
        {
            _ResolutionX = 1600;
            _ResolutionY = 900;
        }
        if (dropdown.value == 3)
        {
            _ResolutionX = 1920;
            _ResolutionY = 1080;
        }
        if (dropdown.value == 4)
        {
            _ResolutionX = 2560;
            _ResolutionY = 1440;
        }
        if (dropdown.value == 5)
        {
            _ResolutionX = 3840;
            _ResolutionY = 2160;
        }
        if (dropdown.value == 6)
        {
            _ResolutionX = 5120;
            _ResolutionY = 2880;
        }
        Screen.SetResolution(_ResolutionX, _ResolutionY, true, Screen.currentResolution.refreshRate);
    }
    public void LoadData(GameData data)
    {
        _Subtitles = data.Subtitles;
        _FullScreen = data.Fullscreen;
        _ResolutionX = data.ResolutionX;
        _ResolutionY = data.ResolutionY;
        _Gamma = data.Gamma;
        _MasterVolume = data.MasterVolume;
        _MusicVolume = data.MusicVolume;
        _SpeachVolume = data.SpeachVolume;
        UpdateMainMenu();

    }
    public void SaveData(ref GameData data)
    {
        data.Subtitles = _Subtitles;
        data.Fullscreen = _FullScreen;
        data.ResolutionX = _ResolutionX;
        data.ResolutionY = _ResolutionY;
        data.Gamma = _Gamma;
        data.MasterVolume = _MasterVolume;
        data.MusicVolume = _MusicVolume;
        data.SpeachVolume = _SpeachVolume;
        UpdateMainMenu();
    }
    public void UpdateMainMenu()
    {
        if (_Subtitles == true)
        {
            _SubtitlesGlowYes.SetActive(true);
            _SubtitlesGlowNo.SetActive(false);

        }
        else
        {
            _SubtitlesGlowYes.SetActive(false);
            _SubtitlesGlowNo.SetActive(true);


        }

        if (_FullScreen == true)
        {
            _FullScreenGlowYes.SetActive(true);
            _FullScreenGlowNo.SetActive(false);

            Screen.fullScreen = true;

        }
        else
        {
            _FullScreenGlowYes.SetActive(false);
            _FullScreenGlowNo.SetActive(true);

            Screen.fullScreen = false;
        }
    }
    
}
