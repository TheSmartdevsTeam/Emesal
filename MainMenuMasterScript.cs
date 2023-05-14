using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMasterScript : MonoBehaviour
{
    Animator _MainMenuCameraAnimator;
    AudioSource _MainMenuAudioSource;
    AudioClip _AudioClip;
    Renderer _GameTitle;
    Canvas _MainMenuCanvas;
    SpriteRenderer _SpriteRenderer;
    Sprite _NormalCursorSprite;
    Texture2D _CursorNormalTexture;
    GameObject _UICamera;
    Vector3 _MousePosition;
    public bool _ClickSound;

    void Start()
    {
        _MainMenuCameraAnimator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        _MainMenuAudioSource = transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();
        _MainMenuCanvas = transform.GetChild(1).GetComponent<Canvas>();
        _AudioClip = Resources.Load("MagicButtonClick") as AudioClip;
        _UICamera = transform.GetChild(0).GetChild(1).gameObject;
        _ClickSound = false;
        
        #region Cursor
        //_SpriteRenderer = GetComponent<SpriteRenderer>();
        //_NormalCursorSprite = _SpriteRenderer.sprite;
        //_CursorNormalTexture.width = (int)1f;
        //_CursorNormalTexture.height = (int)1f;




        #endregion
        //_CursorImage = transform.GetChild(1).GetChild(4).gameObject;
        //_GameTitle = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Renderer>();
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

        _MousePosition.x = Input.mousePosition.x;
        _MousePosition.y = Input.mousePosition.y;
        _MousePosition.z = Camera.main.nearClipPlane;

        #region Cursor
        //Vector2 _CursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Cursor.SetCursor(_CursorNormalTexture, _CursorPosition, CursorMode.ForceSoftware);
        //Cursor.SetCursor(_CursorNormalTexture, _CursorPosition, CursorMode.Auto);

        #endregion
    }

    public void PlayClickSound()
    {
        _ClickSound = true;
        if (_ClickSound)
        {
            if(Resources.Load("MagicButtonClick"))
            {
                _AudioClip = Resources.Load("MagicButtonClick") as AudioClip;
                _MainMenuAudioSource.PlayOneShot(_AudioClip);
            }
            else
            {
                Debug.Log("NO SOURCE");
            }
            MainMenuButtonScript._ClickSound = false;
        }
    }

    public void onConfirmStartClick()
    {
        PlayClickSound();
        //gameObject.transform.parent.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
