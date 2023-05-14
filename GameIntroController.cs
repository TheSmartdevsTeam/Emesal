using Mono.Cecil;
using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class GameIntroController : MonoBehaviour
{
    #region StartVariables
    public CarScript _CarScript;

    public GameObject _GAMESYSTEMSMANAGER;
    public GameObject _PostProcessingObject;
    public Volume _PostProcessingVolume;
    public AudioSource _MusicManager;
    public AudioSource _AmbienceAudioSource;
    AudioClip _AmbienceAudioClip;

    public GameObject _SPECIALOBJECTS;
    public GameObject _FadeInObject;
    public GameObject _FadeOutObject;
    public GameObject _ObjectsToDisableAfterDrive;
    public GameObject _Planks;
    public GameObject _Carpet;
    public GameObject _BrokenPlanks;

    public GameObject _CHARACTERS;
    public GameObject _PlayerObject;
    public Animator _PlayerObjectAnimator;
    public AudioSource _PlayerAudioSource;
    AudioClip _PlayerAudioClip;
    public GameObject _PlayerBody;
    public Animator _PlayerBodyAnimator;
    public AudioSource _PlayerBodyAudioSource;
    AudioClip _PlayerBodyAudioClip;
    public GameObject _PlayerCameraParentObject;
    public Animator _PlayerFadeCameraAnimator;
    public Camera _PlayerCamera;
    public Animator _PlayerCameraAnimator;

    public GameObject _PLAYERUIOBJECT;
    public GameObject _DialogueObject;
    public GameObject _DialogueTextObject;
    public GameObject _StaticCarObject;

    public GameObject _CarObject;
    public Animator _CarAnimator;
    public AudioSource _CarAudioSource;
    AudioClip _CarAudioClip;
    public GameObject _CarCameraObject;
    public GameObject _CarSpotLightsObject;
    public Animator _CarCameraFadeAnimator;



    private string _DialogueText;

    public bool _DelayedPlayed;
    public bool _StartText;
    public bool _StartTextPlayed;
    public bool _Text1;
    public bool _Text2;
    public bool _Text3;
    public bool _Text4;

    public bool _CarCrashed;
    public bool _PlayerMovingOutOfCar;
    public bool _PlayCrashAudioPlayed;

    public bool _PlayHeartBeat;
    public bool _PlayHeartBeatPlayed;

    public bool _PlayLeatherMove;
    public bool _PlayLeatherMovePlayed;

    public bool _PlayOpenDoor;
    public bool _PlayOpenDoorPlayed;

    public bool _PlayLookPhone;
    public bool _PlayLookPhonePlayed;

    public bool _TransitionBool;

    public bool _HurtHead;
    public bool _HurtHeadDone;
    public bool _FadeInAsterStonesDone;
    public bool _NextState;
    public bool _CharacterFadeOutAfterStones;
    public bool _CharacterMoving;
    #endregion

    void Start()
    {
        SetParametersForStart();
        OnStartFadeIn();
        CarDriveAnimation();


    }
    private void SetParametersForStart()
    {
        _GAMESYSTEMSMANAGER = GameObject.FindGameObjectWithTag("GameSystemsManager");
        _PostProcessingObject = _GAMESYSTEMSMANAGER.transform.GetChild(0).gameObject;
        _PostProcessingVolume = _PostProcessingObject.GetComponent<Volume>();
        _MusicManager = _GAMESYSTEMSMANAGER.transform.GetChild(1).GetComponent<AudioSource>();
        _AmbienceAudioSource = _GAMESYSTEMSMANAGER.transform.GetChild(2).GetComponent<AudioSource>();

        _SPECIALOBJECTS = GameObject.FindGameObjectWithTag("SpecialObjects");
        _FadeInObject = _SPECIALOBJECTS.transform.GetChild(0).gameObject;
        _FadeOutObject = _SPECIALOBJECTS.transform.GetChild(1).gameObject;
        _ObjectsToDisableAfterDrive = _SPECIALOBJECTS.transform.GetChild(2).gameObject;
        _StaticCarObject = _SPECIALOBJECTS.transform.GetChild(3).gameObject;
        _Planks = _SPECIALOBJECTS.transform.GetChild(4).gameObject;
        _Carpet = _SPECIALOBJECTS.transform.GetChild(5).gameObject;
        _BrokenPlanks = _SPECIALOBJECTS.transform.GetChild(6).gameObject;

        _CHARACTERS = GameObject.FindGameObjectWithTag("CHARACTERS");

        _PLAYERUIOBJECT = GameObject.FindGameObjectWithTag("PlayerUIObject");
        _DialogueObject = _PLAYERUIOBJECT.transform.GetChild(2).gameObject;
        _DialogueTextObject = _DialogueObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject;

        if (_CarObject = GameObject.FindGameObjectWithTag("CarObject"))
        {
            _CarObject.SetActive(true);
            _CarScript = _CarObject.GetComponent<CarScript>();
            _CarAnimator = _CarObject.GetComponent<Animator>();
            _CarAudioSource = _CarObject.GetComponent<AudioSource>();
            _CarCameraObject = _CarObject.transform.GetChild(0).gameObject;
            _CarCameraFadeAnimator = _CarCameraObject.transform.GetChild(0).GetComponent<Animator>();
            _CarSpotLightsObject = GameObject.FindGameObjectWithTag("CarSpotLightsObject");
        }

        _CarCrashed = false;
        _PlayerMovingOutOfCar = false;
        _PlayLookPhone = false;
        _PlayLookPhonePlayed = false;
        _HurtHead = false;
        _HurtHeadDone = false;
        _FadeInAsterStonesDone = false;
        _NextState = false;
        _CharacterMoving = false;
        _PlayOpenDoorPlayed = false;
        _StartTextPlayed = false;
        _DelayedPlayed = false;
        _DialogueObject.SetActive(false);
    }

    void _SetUpPlayerParameters()
    {
        if (_PlayerObject = _CHARACTERS.transform.GetChild(0).gameObject)
        {
            _PlayerObject.SetActive(true);
            _PlayerObjectAnimator = _PlayerObject.GetComponent<Animator>();
            _PlayerAudioSource = _PlayerObject.GetComponent<AudioSource>();
            _PlayerBody = _PlayerObject.transform.GetChild(0).gameObject;
            _PlayerBodyAnimator = _PlayerBody.GetComponent<Animator>();
            _PlayerBodyAudioSource = _PlayerBody.GetComponent<AudioSource>();
            _PlayerCameraParentObject = _PlayerObject.transform.GetChild(1).gameObject;
            _PlayerCamera = _PlayerCameraParentObject.transform.GetChild(1).GetComponent<Camera>();
            _PlayerCameraAnimator = _PlayerCameraParentObject.GetComponent<Animator>();
            _PlayerFadeCameraAnimator = _PlayerCameraParentObject.transform.GetChild(0).GetComponent<Animator>();
        }
    }
    void OnStartFadeIn()
    {
        if (_MusicManager != null)
        {
            //_MusicManager = GetComponent<AudioSource>();
            _MusicManager.volume = 0.4f;
            _MusicManager.PlayOneShot(Resources.Load("Beginning") as AudioClip);
            //_CarAudioClip = Resources.Load("CarDrivingInterior") as AudioClip;
            //_CarAudioSource.clip = _CarAudioClip;
            //_CarAudioSource.Play();
        }
        _CarCameraFadeAnimator.Play("Fade In Animation");
        _CarCameraFadeAnimator.SetBool("BlackScreen", false);
        _CarCameraFadeAnimator.SetBool("FadeIn", false);


    }
    private void CarDriveAnimation()
    {
        PlayAudioCarInterior();
    }
    private void PlayAudioCarInterior()
    {
        if (GetComponent<AudioSource>() != null)
        {
            _CarAudioSource = GetComponent<AudioSource>();
            _CarAudioSource.volume = 0.7f;
            _CarAudioClip = Resources.Load("CarDrivingInterior") as AudioClip;
            _CarAudioSource.clip = _CarAudioClip;
            _CarAudioSource.Play();
        }
    }
    private void Update()
    {
        if (_StartTextPlayed == false)
        {
            if (_DelayedPlayed == false)
            {
                StartCoroutine(DelaySomething());
                _DelayedPlayed = true;
            }

            if (_StartText == true)
            {
                _StartText = false;
                StartCoroutine(ShowDelayedText1());

            }
            if (_Text1 == true)
            {
                _Text1 = false;
                StartCoroutine(ShowDelayedText2());

            }
            if (_Text2 == true)
            {
                _Text2 = false;
                StartCoroutine(ShowDelayedText3());


            }
            if (_Text3 == true)
            {
                StartCoroutine(ShowDelayedText4());
                StartCoroutine(DelayHideTextBox());
                _StartTextPlayed = true;
            }

        }
        
        CarStopingAnimation();
        FadeOutAfterDrive();
        CharacterExitsCar();

    }
    private void LateUpdate()
    {
        if (_CarCrashed == true)
        {
            //PostProcessingEffects
            _PostProcessingObject.SetActive(true);
            for (int i = 0; i < _PostProcessingVolume.sharedProfile.components.Count; i++)
            {
                if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "LensDistortion")
                {
                    UnityEngine.Rendering.Universal.LensDistortion _LensDistortion = (UnityEngine.Rendering.Universal.LensDistortion)_PostProcessingVolume.sharedProfile.components[i];
                    _LensDistortion.active = true;
                    _LensDistortion.SetAllOverridesTo(true);
                    _LensDistortion.intensity.value = 1f;
                    _LensDistortion.xMultiplier.value = Random.Range(0.1f, 0.2f);
                    _LensDistortion.yMultiplier.value = Random.Range(0.1f, 0.2f);
                }

                if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "Vignette")
                {
                    UnityEngine.Rendering.Universal.Vignette _Vignette = (UnityEngine.Rendering.Universal.Vignette)_PostProcessingVolume.sharedProfile.components[i];
                    _Vignette.active = true;
                    _Vignette.SetAllOverridesTo(true);
                    _Vignette.color.value = Color.gray;
                    _Vignette.intensity.value = 0.5f;
                    _Vignette.smoothness.value = 0.2f;
                    _Vignette.rounded.value = true;
                }

                if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "LiftGammaGain")
                {
                    UnityEngine.Rendering.Universal.LiftGammaGain _LiftGammaGain = (UnityEngine.Rendering.Universal.LiftGammaGain)_PostProcessingVolume.sharedProfile.components[i];
                    _LiftGammaGain.active = true;
                    _LiftGammaGain.SetAllOverridesTo(true);
                    //_LiftGammaGain.gain.value = new Vector4(1, 1, 1,1.4f);

                }

                if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "Bloom")
                {
                    UnityEngine.Rendering.Universal.Bloom _Bloom = (UnityEngine.Rendering.Universal.Bloom)_PostProcessingVolume.sharedProfile.components[i];
                    _Bloom.active = true;
                    _Bloom.SetAllOverridesTo(true);
                    _Bloom.threshold.value = 0.5f;
                    _Bloom.intensity.value = 5;
                    _Bloom.scatter.value = 0.5f;
                    _Bloom.tint.value = Color.white;
                    _Bloom.skipIterations.value = 6;

                }

                if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "DepthOfField")
                {
                    UnityEngine.Rendering.Universal.DepthOfField _DepthOfField = (UnityEngine.Rendering.Universal.DepthOfField)_PostProcessingVolume.sharedProfile.components[i];
                    _DepthOfField.active = true;
                    _DepthOfField.SetAllOverridesTo(true);
                    _DepthOfField.mode.value = DepthOfFieldMode.Bokeh;
                    _DepthOfField.focusDistance.value = 0.1f;
                    _DepthOfField.focalLength.value = Random.Range(100f, 300f);
                    _DepthOfField.aperture.value = 1;
                    _DepthOfField.bladeCount.value = 3;
                    _DepthOfField.bladeCurvature.value = 1;
                    _DepthOfField.bladeRotation.value = 0;

                }
            }
        }
        if (_CarCrashed == false)
        {
            //PostProcessingEffects
            _PostProcessingObject.SetActive(true);
            for (int i = 0; i < _PostProcessingVolume.sharedProfile.components.Count; i++)
            {
                if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "LensDistortion")
                {
                    UnityEngine.Rendering.Universal.LensDistortion _LensDistortion = (UnityEngine.Rendering.Universal.LensDistortion)_PostProcessingVolume.sharedProfile.components[i];
                    _LensDistortion.active = false;

                }

                if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "Vignette")
                {
                    UnityEngine.Rendering.Universal.Vignette _Vignette = (UnityEngine.Rendering.Universal.Vignette)_PostProcessingVolume.sharedProfile.components[i];
                    _Vignette.active = false;
                }

                if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "LiftGammaGain")
                {
                    UnityEngine.Rendering.Universal.LiftGammaGain _LiftGammaGain = (UnityEngine.Rendering.Universal.LiftGammaGain)_PostProcessingVolume.sharedProfile.components[i];
                    _LiftGammaGain.active = true;
                    _LiftGammaGain.SetAllOverridesTo(true);
                    //_LiftGammaGain.gain.value = new Vector4(1, 1, 1,1.4f);

                }

                if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "Bloom")
                {
                    UnityEngine.Rendering.Universal.Bloom _Bloom = (UnityEngine.Rendering.Universal.Bloom)_PostProcessingVolume.sharedProfile.components[i];
                    _Bloom.active = true;
                    _Bloom.SetAllOverridesTo(true);
                    _Bloom.threshold.value = 0.5f;
                    _Bloom.intensity.value = 5;
                    _Bloom.scatter.value = 0.5f;
                    _Bloom.tint.value = Color.white;
                    _Bloom.skipIterations.value = 6;

                }

                if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "DepthOfField")
                {
                    UnityEngine.Rendering.Universal.DepthOfField _DepthOfField = (UnityEngine.Rendering.Universal.DepthOfField)_PostProcessingVolume.sharedProfile.components[i];
                    _DepthOfField.active = false;

                }

            }
        }

    }
    private void CarStopingAnimation()
    {
        if (_CarScript._GetStopingCar())
        {
            if (_PlayCrashAudioPlayed == false)
            {
                StartCoroutine(PlayCarCrashAudio());
                _PlayCrashAudioPlayed = true;
                StopCoroutine(PlayCarCrashAudio());
                StartCoroutine(StopCarLights());
                StartCoroutine(SetCarCrashed());
            }
        }
    }
    private IEnumerator PlayCarCrashAudio()
    {
        _CarAudioClip = Resources.Load("CarCrash") as AudioClip;
        _CarAudioSource.volume = 1f;
        _CarAudioSource.clip = _CarAudioClip;
        _CarAudioSource.PlayOneShot(_CarAudioClip, 1f);
        yield return new WaitForSeconds(1);
    }
    IEnumerator StopCarLights()
    {
        yield return new WaitForSeconds(0.65f);
        _CarSpotLightsObject.SetActive(false);
    }
    IEnumerator SetCarCrashed()
    {
        yield return new WaitForSeconds(17);
        _CarCrashed = true;

    }
    void FadeOutAfterDrive()
    {
        if (_CarScript._GetFadeOut() == true)
        {
            _CarCameraFadeAnimator.speed = 0.1f;
            FadeOut();
        }
    }
    void FadeOut()
    {
        _CarCameraFadeAnimator.SetBool("FadeOut", true);
        _CarCameraFadeAnimator.SetBool("BlackScreen", true);
        _CarCameraFadeAnimator.SetBool("FadeOut", false);
    }
    private void CharacterExitsCar()
    {
        if (_CarCrashed)
        {
            if (_PlayHeartBeatPlayed == false)
            {
                StartCoroutine(CharacterState());
                _PlayHeartBeatPlayed = true;
                if (_PlayHeartBeatPlayed == true && _PlayLeatherMovePlayed == false)
                {
                    StartCoroutine(PlayLeatherMove());
                    _PlayLeatherMovePlayed = true;
                    if (_PlayLeatherMovePlayed && _PlayOpenDoorPlayed == false)
                    {
                        PlayOpenDoorAudio();
                        CharacterFadeIn();
                        _PlayOpenDoorPlayed = true;
                        StopAllCoroutines();
                        SwapCameras();
                        StartCoroutine(ShowDelayedText5());

                        StartCoroutine(DelayLookPhone());
                        StartCoroutine(ShowDelayedText6());
                        StartCoroutine(ShowDelayedText7());
                        StartCoroutine(HideDelayedText7());
                    }

                }
            }

        }
    }
    IEnumerator CharacterState()
    {
        SwapAndActivatePrefabs();
        CharacterBlackScreen();
        PlayHeartBeatAudio();
        PlayForestAmbience();
        yield return new WaitForSeconds(6);

    }
    private void SwapAndActivatePrefabs()
    {
        _SetUpPlayerParameters();
        _CarObject.SetActive(false);
        _StaticCarObject.SetActive(true);
    }
    private void PlayHeartBeatAudio()
    {
        _PlayerAudioClip = Resources.Load("HeartbeatFastDizzyLoop2") as AudioClip;
        _PlayerAudioSource.clip = _PlayerAudioClip;
        _PlayerAudioSource.loop = true;
        _PlayerAudioSource.pitch = 0.9f;
        _PlayerAudioSource.reverbZoneMix = 0.3f;
        _PlayerAudioSource.volume = 0.8f;
        _PlayerAudioSource.Play();
    }
    private void PlayForestAmbience()
    {
        _AmbienceAudioClip = Resources.Load("Drone_Atmosphere_Mystery_06") as AudioClip;
        _AmbienceAudioSource.clip = _AmbienceAudioClip;
        _AmbienceAudioSource.loop = true;
        _AmbienceAudioSource.reverbZoneMix = 0.3f;
        _AmbienceAudioSource.volume = 0.7f;
        _AmbienceAudioSource.Play();
    }
    private void CharacterBlackScreen()
    {
        _PlayerFadeCameraAnimator.SetBool("BlackScreen", true);
        _PlayerFadeCameraAnimator.Play("BlackScreen");
    }
    IEnumerator PlayLeatherMove()
    {
        PlayLeatherMoveAudio();
        yield return new WaitForSeconds(6);
        _PlayLeatherMove = false;
        _PlayLeatherMovePlayed = true;
    }
    private void PlayLeatherMoveAudio()
    {
        _CarAudioClip = Resources.Load("LeatherMoveSound") as AudioClip;
        _CarAudioSource.clip = _CarAudioClip;
        _CarAudioSource.PlayOneShot(_CarAudioClip, 1);
    }
    private void PlayOpenDoorAudio()
    {
        _CarAudioClip = Resources.Load("CarOpenDoor") as AudioClip;
        _CarAudioSource.clip = _CarAudioClip;
        _CarAudioSource.PlayOneShot(_CarAudioClip, 1);
    }
    void CharacterFadeIn()
    {
        _PlayerFadeCameraAnimator.Play("Fade In Animation");
        _PlayerFadeCameraAnimator.SetBool("BlackScreen", false);
        _PlayerFadeCameraAnimator.SetBool("FadeIn", false);
    }
    private void SwapCameras()
    {
        _PlayerCameraParentObject.SetActive(true);
        _PlayerCamera = _PlayerCameraParentObject.transform.GetChild(1).GetComponent<Camera>();
        _CarCameraObject.SetActive(false);

    }
    private IEnumerator DelayLookPhone()
    {
        yield return new WaitForSecondsRealtime(10);
        _PlayLookPhone = true;
        if (_PlayOpenDoorPlayed == true && _PlayLookPhone == true && _PlayLookPhonePlayed == false)
        {
            CharacterChecksMobilePhone();
            _PlayLookPhonePlayed = true;

        }
    }
    private void CharacterChecksMobilePhone()
    {
        StartCoroutine(LookPhone());
    }
    IEnumerator LookPhone()
    {
        Debug.Log("1 LOOKING PHONE");
        _PlayerCameraAnimator.SetBool("LookPhone", true);
        _PlayerBodyAnimator.SetBool("UseMPhone", true);

        yield return new WaitForSeconds(6);
        _PlayerCameraAnimator.SetBool("LookPhone", false);
        _PlayLookPhone = false;
        _PlayLookPhonePlayed = true;
        if (_PlayLookPhone == false && _PlayLookPhonePlayed == true)
        {
            CharacterMoving();

        }

    }
    public void CharacterMoving()
    {
        StartCoroutine(CharacterMovingCoroutine());
        _CharacterFadeOutAfterStones = true;
    }
    IEnumerator CharacterMovingCoroutine()
    {
        _PlayerBodyAnimator.SetBool("Walking", true);
        //_CharacterAnimator.Play("Walking");
        //_CharacterAnimator.SetBool("Walking", true);
        Debug.Log("2 WALKING");
        yield return new WaitForSeconds(9);
        _CharacterMoving = true;
        if (_CharacterMoving)
        {
            PlayFootstepsAudio();
            //CharacterFadeOut();
            StartCoroutine(DelayedFadeOut1());
            StartCoroutine(DelayedFadeIn1());
            StartCoroutine(ShowDelayedText8());
            StartCoroutine(DelayHideText8());
            StartCoroutine(DelayedFadeOut2());
            StartCoroutine(ShowDelayedText9());
            StartCoroutine(DelayHideText9());
            StartCoroutine(DelayedFadeIn2());
            StartCoroutine(ShowDelayedText10());
            StartCoroutine(DelayHideText10());
            StartCoroutine(ShowDelayedText11());
            StartCoroutine(DelayHideText11());
            StartCoroutine(DelayedFadeOut3());

            //_CharacterAnimator.StopPlayback();
            /*
            _CharacterCameraFadeAnimator.SetBool("FadeOut", true);
            _CharacterCameraFadeAnimator.SetBool("BlackScreen", true);
            _CharacterAnimator.SetBool("Walking", false);
            Debug.Log("BLACK SCREEN");*/
        }
    }

    private void PlayFootstepsAudio()
    {
        _PlayerBodyAudioClip = Resources.Load("GrassWalk_01") as AudioClip;
        _PlayerBodyAudioSource.clip = _PlayerBodyAudioClip;
        _PlayerBodyAudioSource.loop = true;
        _PlayerBodyAudioSource.pitch = 0.7f;
        _PlayerBodyAudioSource.volume = 0.5f;
        _PlayerBodyAudioSource.Play();
    }

    void CharacterFadeOut()
    {
        if (_CharacterFadeOutAfterStones == true)
        {
            Debug.Log("FADE OUT");
            _PlayerFadeCameraAnimator.SetBool("FadeOut", true);
            _PlayerFadeCameraAnimator.SetBool("BlackScreen", true);
            _CharacterFadeOutAfterStones = false;
        }

    }
    void SetText(string Text)
    {
        _DialogueText = Text;
        _DialogueTextObject.GetComponent<TextMeshProUGUI>().text = _DialogueText;
    }
    IEnumerator DelaySomething()
    {
        yield return new WaitForSeconds(4);
        _StartText = true;
    }
    IEnumerator ShowDelayedText1()
    {
        yield return new WaitForSeconds(7);
        _DialogueObject.SetActive(true);
        SetText("My name is Aurora..");
        _Text1 = true;
    }
    IEnumerator ShowDelayedText2()
    {
        _Text1 = false;
        yield return new WaitForSeconds(8);
        SetText("I decided to change my life completely.. \nI decided to move away from everything that binds me to my old life...");
        _Text2 = true;
    }
    IEnumerator ShowDelayedText3()
    {
        _Text2 = false;
        yield return new WaitForSeconds(11);
        SetText("A fresh start!\nA new beginning...");
        _Text3 = true;
    }
    IEnumerator ShowDelayedText4()
    {
        _Text3 = false;
        yield return new WaitForSeconds(12);
        SetText("WHAT THE...!!???");
        _Text4 = true;
    }
    IEnumerator DelayHideTextBox()
    {
        yield return new WaitForSeconds(14);
        _DialogueObject.SetActive(false);
    }
    IEnumerator ShowDelayedText5()
    {

        yield return new WaitForSeconds(4);
        _DialogueObject.SetActive(true);
        SetText("AGHHH!!! My Head!");
    }
    IEnumerator ShowDelayedText6()
    {
        yield return new WaitForSeconds(8);
        SetText("I need to call ambulance!");
    }
    IEnumerator DelayedFadeOut1()
    {
        yield return new WaitForSeconds(10);
        _PlayerFadeCameraAnimator.speed = 2f;
        _CharacterFadeOutAfterStones = true;
        CharacterFadeOut();
        //_CharacterAnimator.StopPlayback();
        Debug.Log("FADE OUT 1(11sec)");
    }
    IEnumerator DelayedFadeIn1()
    {
        yield return new WaitForSeconds(18);
        _PlayerFadeCameraAnimator.speed = 1f;
        _PlayerFadeCameraAnimator.Play("Fade In Animation");
        _PlayerFadeCameraAnimator.SetBool("BlackScreen", false);
        _PlayerFadeCameraAnimator.SetBool("FadeIn", false);
        Debug.Log("FADE IN 1(18sec)");
    }
    IEnumerator ShowDelayedText7()
    {
        yield return new WaitForSeconds(14);
        _DialogueObject.SetActive(true);
        SetText("WELL WHY AM I NOT SUPRISED!\nI should try to find someone to help me..");
    }
    IEnumerator HideDelayedText7()
    {
        yield return new WaitForSeconds(20);
        _DialogueObject.SetActive(false);
    }
    IEnumerator ShowDelayedText8()
    {
        yield return new WaitForSeconds(9);
        _DialogueObject.SetActive(true);
        SetText("WHY ARE ROCKS ON A ROAD?? WHAT THE HELL IS GOING ON?\nMY HEADDD AGHH!");
    }
    IEnumerator DelayHideText8()
    {
        yield return new WaitForSeconds(15);
        _DialogueObject.SetActive(false);
    }
    IEnumerator ShowDelayedText9()
    {
        yield return new WaitForSeconds(18);
        _PlayerBodyAudioSource.Stop();
        _DialogueObject.SetActive(true);
        SetText("Is that a house? Maybe someone can help me, i should check it out..");
    }
    IEnumerator DelayHideText9()
    {
        yield return new WaitForSeconds(26);
        _DialogueObject.SetActive(false);
    }
    IEnumerator DelayedFadeOut2()
    {
        yield return new WaitForSeconds(20);
        _PlayerFadeCameraAnimator.speed = 3f;
        CharacterFadeOut();
        _PlayerBodyAnimator.StopPlayback();
        Debug.Log("FADE OUT 2(19sec)");
    }
    IEnumerator DelayedFadeIn2()
    {
        yield return new WaitForSeconds(32f);
        _PlayerFadeCameraAnimator.speed = 1f;
        _PlayerFadeCameraAnimator.Play("Fade In Animation");
        _PlayerFadeCameraAnimator.SetBool("BlackScreen", false);
        _PlayerFadeCameraAnimator.SetBool("FadeIn", false);
        _PlayerFadeCameraAnimator.SetBool("FadeOut", false);
        Debug.Log("FADE IN 2(32sec)");
    }
    IEnumerator ShowDelayedText10()
    {
        yield return new WaitForSeconds(36);
        SetText("Hellou!? Is anybody home?\n... ...i should check the place");
    }
    IEnumerator DelayHideText10()
    {
        yield return new WaitForSeconds(44);
        _PlayerBodyAudioSource.Play();
        _DialogueObject.SetActive(false);
    }
    IEnumerator DelayedAudioPlay()
    {
        yield return new WaitForSeconds(48);
        _PlayerBodyAudioClip = Resources.Load("WoodWalk_01") as AudioClip;
        _PlayerBodyAudioSource.clip = _PlayerBodyAudioClip;
        _PlayerBodyAudioSource.loop = true;
        _PlayerBodyAudioSource.pitch = 0.7f;
        _PlayerBodyAudioSource.volume = 0.5f;
        _PlayerBodyAudioSource.Play();
    }
    IEnumerator ShowDelayedText11()
    {
        yield return new WaitForSeconds(81);
        _PlayerBodyAudioSource.Stop();
        _DialogueObject.SetActive(true);
        SetText("..O NOO!");
        _Planks.SetActive(false);
        _Carpet.SetActive(false);
        _BrokenPlanks.SetActive(true);
        PlayPlanksSquikSound();
        PlayPlanksBreakSound();
    }
    private void PlayPlanksSquikSound()
    {
        _AmbienceAudioClip = Resources.Load("PlanksSquik") as AudioClip;
        _AmbienceAudioSource.clip = _AmbienceAudioClip;
        _AmbienceAudioSource.PlayOneShot(_AmbienceAudioClip, 1);
    }
    private void PlayPlanksBreakSound()
    {
        _AmbienceAudioClip = Resources.Load("PlanksBreaking") as AudioClip;
        _AmbienceAudioSource.clip = _AmbienceAudioClip;
        _AmbienceAudioSource.PlayOneShot(_AmbienceAudioClip, 1);
    }
    IEnumerator DelayHideText11()
    {
        yield return new WaitForSeconds(84);
        _DialogueObject.SetActive(false);
    }
    IEnumerator DelayedFadeOut3()
    {
        yield return new WaitForSeconds(75f);
        _PlayerFadeCameraAnimator.speed = 1f;
        _PlayerFadeCameraAnimator.SetBool("FadeOut", true);
        _PlayerFadeCameraAnimator.SetBool("BlackScreen", true);
        Debug.Log("FADE OUT 3(75sec)");
    }
}
