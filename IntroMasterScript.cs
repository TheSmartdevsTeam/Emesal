using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;


public class IntroMasterScript : MonoBehaviour
{
    #region StartVariables
    public GameObject _IntroSystemsManager;
    GameObject _PostProcessingObject;
    Volume _PostProcessingVolume;
    AudioSource _MusicManager;
    AudioClip _MusicAudioClip;
    AudioSource _AmbienceManager;
    AudioClip _AmbienceAudioClip;

    CarScript _CarScript;
    public GameObject _CarObject;
    Animator _CarAnimator;
    AudioSource _CarAudioSource;
    AudioClip _CarAudioClip;
    GameObject _CarCameraObject;
    GameObject _CarSpotLightsObject;
    Animator _CarCameraFadeAnimator;

    public GameObject _CHARACTERS;
    GameObject _CharacterMasterObject;
    Animator _CharacterINTROMasterAnimator;
    AudioSource _CharacterAudioSource;
    AudioClip _CharacterAudioClip;
    GameObject _CharacterBody;
    Animator _CharacterBodyAnimator;
    AudioSource _CharacterBodyAudioSource;
    AudioClip _CharacterBodyAudioClip;
    GameObject _CharacterCameraParentObject;
    Animator _CharacterFadeCameraAnimator;
    Camera _CharacterCamera;
    Animator _CharacterCameraAnimator;
    Vector3 _LastPosition;
    AudioSource _CharacterCameraAudioSource;
    AudioClip _CharacterCameraAudioClip;

    public GameObject _PLAYERUIOBJECT;
    GameObject _DialogueObject;
    GameObject _DialogueTextObject;

    public GameObject _SPECIALOBJECTS;
    GameObject _FadeInObject;
    GameObject _FadeOutObject;
    GameObject _ObjectsToDisableAfterDrive;
    GameObject _Planks;
    GameObject _StaticCarObject;

    private string _DialogueText;

    bool _PlayText;
    bool _TextDelayDone;
    bool _Text1;
    bool _Text2;
    bool _Text3;
    bool _Text4;
    bool _Text5;

    bool _CarCrashed;
    bool _PlayerMovingOutOfCar;
    bool _PlayCrashAudioPlayed;

    bool _PlayHeartBeat;
    bool _PlayHeartBeatPlayed;

    bool _PlayLeatherMove;
    bool _PlayLeatherMovePlayed;

    bool _PlayOpenDoor;
    bool _PlayOpenDoorPlayed;

    bool _PlayLookPhone;
    bool _PlayLookPhonePlayed;

    bool _TransitionBool;

    bool _HurtHead;
    bool _HurtHeadDone;
    bool _FadeInAsterStonesDone;
    bool _NextState;
    bool _CharacterFadeOutAfterStones;
    bool _CharacterMoving;
    public CharacterIntroScript CIS;
    #endregion
    void Start()
    {
        SetUpStartParameters();
        PlayCarDrive();
    }
    void Update()
    {
        DoIntro();
        if(CIS.GetComponent<CharacterIntroScript>()._PlanksBreak)
        {
            StartCoroutine(EndIntroFadeOut());
        }
        
    }
    private void LateUpdate()
    {
        SetUpPostProcessingEffects();
    }
    void SetUpStartParameters()
    {
        _IntroSystemsManager = GameObject.FindGameObjectWithTag("GameSystemsManager");
        _PostProcessingObject = _IntroSystemsManager.transform.GetChild(0).gameObject;
        _PostProcessingVolume = _PostProcessingObject.GetComponent<Volume>();
        _MusicManager = _IntroSystemsManager.transform.GetChild(1).GetComponent<AudioSource>();
        _AmbienceManager = _IntroSystemsManager.transform.GetChild(2).GetComponent<AudioSource>();

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
        _CHARACTERS = GameObject.FindGameObjectWithTag("CHARACTERS");

        _PLAYERUIOBJECT = GameObject.FindGameObjectWithTag("PlayerUIObject");
        _DialogueObject = _PLAYERUIOBJECT.transform.GetChild(0).GetChild(0).gameObject;
        _DialogueTextObject = _DialogueObject.transform.GetChild(0).transform.GetChild(0).GetChild(2).gameObject;

        _SPECIALOBJECTS = GameObject.FindGameObjectWithTag("SpecialObjects");
        _FadeInObject = _SPECIALOBJECTS.transform.GetChild(0).gameObject;
        _FadeOutObject = _SPECIALOBJECTS.transform.GetChild(1).gameObject;
        _ObjectsToDisableAfterDrive = _SPECIALOBJECTS.transform.GetChild(2).gameObject;
        _StaticCarObject = _SPECIALOBJECTS.transform.GetChild(3).gameObject;
        _Planks = _SPECIALOBJECTS.transform.GetChild(4).gameObject;

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
        _PlayText = false;
        _DialogueObject.SetActive(false);
    }
    void SetUpPlayerParameters()
    {
        if (_CharacterMasterObject = _CHARACTERS.transform.GetChild(0).gameObject)
        {
            _CharacterMasterObject.SetActive(true);
            _CharacterINTROMasterAnimator = _CHARACTERS.transform.GetChild(0).GetComponent<Animator>();
            _CharacterAudioSource = _CharacterMasterObject.GetComponent<AudioSource>();
            _CharacterBody = _CharacterMasterObject.transform.GetChild(0).gameObject;
            _CharacterBodyAnimator = _CharacterBody.GetComponent<Animator>();
            _CharacterBodyAudioSource = _CharacterBody.GetComponent<AudioSource>();
            _CharacterCameraParentObject = _CHARACTERS.transform.GetChild(0).GetChild(1).gameObject;
            _CharacterCamera = _CharacterCameraParentObject.transform.GetChild(1).GetComponent<Camera>();
            _CharacterCameraAnimator = _CharacterCameraParentObject.GetComponent<Animator>();
            _CharacterFadeCameraAnimator = _CharacterCameraParentObject.transform.GetChild(0).GetComponent<Animator>();
            _CharacterCameraAudioSource = _CHARACTERS.transform.GetChild(0).GetChild(1).GetComponent<AudioSource>();
            _CharacterCameraAudioClip = Resources.Load<AudioClip>("CharacterSounds/ScaredBreathFADE_IN_OUT");
        }
    }
    void PlayCarDrive()
    {
        if (_MusicManager != null)
        {
            _MusicManager.volume = 0.4f;
            _MusicManager.PlayOneShot(Resources.Load<AudioClip>("Music/Beginning"));
        }
        _CarCameraFadeAnimator.Play("Fade In Animation");
        _CarCameraFadeAnimator.SetBool("BlackScreen", false);
        _CarCameraFadeAnimator.SetBool("FadeIn", false);
        if (GetComponent<AudioSource>() != null)
        {
            _CarAudioSource = GetComponent<AudioSource>();
            _CarAudioSource.volume = 0.7f;
            _CarAudioClip = Resources.Load<AudioClip>("Car/CarDrivingInterior");
            _CarAudioSource.clip = _CarAudioClip;
            _CarAudioSource.Play();
        }
    }
    void DoIntro()
    {
        DoIntroSubtitles();
        CarStopingAnimation();
        FadeOutAfterDrive();
        CharacterExitsCar();
    }
    void DoIntroSubtitles()
    {
        if (_PlayText == false)
        {
            if (_TextDelayDone == false)
            {
                _ = StartCoroutine(DelaySubtitlesForSeconds(2));
                _TextDelayDone = true;
            }

            if (_Text1)
            {
                _ = StartCoroutine(MyNameIsText(5));
            }
            if (_Text2)
            {
                _ = StartCoroutine(IDecidedText(7));

            }
            if (_Text3)
            {
                _ = StartCoroutine(CantSeeShText(12));

            }
            if (_Text4)
            {
                _ = StartCoroutine(WhatTheHText(17));
                _ = StartCoroutine(HideDialogue(18));
                _PlayText = true;
            }
        }
    }
    void SetText(string Text)
    {
        _DialogueText = Text;
        _DialogueTextObject.GetComponent<TextMeshProUGUI>().text = _DialogueText;
    }
    IEnumerator DelaySubtitlesForSeconds(int i)
    {
        yield return new WaitForSeconds(i);
        _Text1 = true;
    }
    IEnumerator MyNameIsText(float i)
    {
        yield return new WaitForSeconds(i);
        _DialogueObject.SetActive(true);
        SetText("My name is Aurora..");
        _Text1 = false;
        _Text2 = true;
    }
    IEnumerator IDecidedText(int i)
    {

        yield return new WaitForSeconds(i);
        SetText("I decided to change my life completely.. \nI don't know where am I but i don't care! I just want to forget everything that has happened!");
        _Text2 = false;
        _Text3 = true;
    }
    IEnumerator CantSeeShText(int i)
    {

        yield return new WaitForSeconds(i);
        SetText("I am really tired and there is fog on the road.\nI need to stop at next motel to rest.");
        _Text3 = false;
        _Text4 = true;
    }
    IEnumerator WhatTheHText(int i)
    {

        yield return new WaitForSeconds(i);
        SetText("WHAT THE...!!???");
        _Text4 = false;
        _Text5 = true;
    }
    IEnumerator HideDialogue(int i)
    {
        yield return new WaitForSeconds(i);
        _DialogueObject.SetActive(false);
    }
    IEnumerator AghMyHeadText()
    {

        yield return new WaitForSeconds(4);
        _DialogueObject.SetActive(true);
        SetText("What the heck??? Why are rocks on the road? AGHHH!!! My Head!");
    }
    IEnumerator ShowDelayedText7()
    {
        yield return new WaitForSeconds(14);
        _DialogueObject.SetActive(true);
        SetText("O crap screen is broken... ! Ugh soo dizzy.. \nIs that rest sign?? \nI could check that out maybe there is a phone or something...");
    }
    IEnumerator HideDelayedText7()
    {
        yield return new WaitForSeconds(20);
        _DialogueObject.SetActive(false);
    }
    private void EnableAndAdjustPPOForCharacter()
    {
        _PostProcessingObject.SetActive(true);
        for (int i = 0; i < _PostProcessingVolume.sharedProfile.components.Count; i++)
        {
            if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "LensDistortion")
            {
                UnityEngine.Rendering.Universal.LensDistortion _LensDistortion = (UnityEngine.Rendering.Universal.LensDistortion)_PostProcessingVolume.sharedProfile.components[i];
                _LensDistortion.active = true;
                _LensDistortion.SetAllOverridesTo(true);
                _LensDistortion.intensity.value = 0.6f;
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
                _DepthOfField.active = false;
                _DepthOfField.SetAllOverridesTo(true);
                _DepthOfField.mode.value = DepthOfFieldMode.Bokeh;
                _DepthOfField.focusDistance.value = 0f;
                _DepthOfField.focalLength.value = Random.Range(5f, 10f);
                _DepthOfField.aperture.value = 1;
                _DepthOfField.bladeCount.value = 3;
                _DepthOfField.bladeCurvature.value = 1;
                _DepthOfField.bladeRotation.value = 0;
            }
        }
    }
    private void DisabledAndAdjustPPOForCar()
    {
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
    private void SetUpPostProcessingEffects()
    {
        if (_CarCrashed == true)
        {
            EnableAndAdjustPPOForCharacter();
        }
        if (_CarCrashed == false)
        {
            DisabledAndAdjustPPOForCar();
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
        _CarAudioClip = Resources.Load<AudioClip>("Car/CarCrash");
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
    private void CharacterBlackScreen()
    {
        _CharacterFadeCameraAnimator.SetBool("BlackScreen", true);
        _CharacterFadeCameraAnimator.Play("BlackScreen");
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
    private IEnumerator DelayedCharacterFadeIn(int i)
    {
        yield return new WaitForSecondsRealtime(i);
        _CharacterFadeCameraAnimator.SetBool("FadeIn", false);

    }
    private void SwapCameras()
    {
        _CharacterCameraParentObject.SetActive(true);
        _CharacterCamera = _CharacterCameraParentObject.transform.GetChild(1).GetComponent<Camera>();
        _CarCameraObject.SetActive(false);
    }
    IEnumerator CharacterState()
    {
        SwapAndActivatePrefabs();
        CharacterBlackScreen();
        PlayHeartBeatAudio();
        PlayForestAmbience();
        PlayMusic();

        
        yield return new WaitForSeconds(6);
    }
    private void SwapAndActivatePrefabs()
    {
        SetUpPlayerParameters();
        _CarObject.SetActive(false);
        _StaticCarObject.SetActive(true);
    }
    private void PlayHeartBeatAudio()
    {
        _CharacterAudioClip = Resources.Load<AudioClip>("CharacterSounds/HeartbeatFastDizzyLoop2");
        _CharacterAudioSource.clip = _CharacterAudioClip;
        _CharacterAudioSource.loop = true;
        _CharacterAudioSource.pitch = 0.9f;
        _CharacterAudioSource.reverbZoneMix = 0.3f;
        _CharacterAudioSource.volume = 0.4f;
        _CharacterAudioSource.Play();
    }
    private void PlayForestAmbience()
    {
        _AmbienceAudioClip = Resources.Load<AudioClip>("Ambience/WindAmbience1");
        _AmbienceManager.clip = _AmbienceAudioClip;
        _AmbienceManager.loop = true;
        _AmbienceManager.reverbZoneMix = 0.3f;
        _AmbienceManager.volume = 0.7f;
        _AmbienceManager.Play();
    }
    private void PlayMusic()
    {
        _MusicAudioClip = Resources.Load<AudioClip>("Music/HollowDroneAmbience");
        _MusicManager.clip = _MusicAudioClip;
        _MusicManager.loop = true;
        _MusicManager.reverbZoneMix = 0.3f;
        _MusicManager.volume = 0.7f;
        _MusicManager.Play();
    }
    IEnumerator PlayLeatherMove()
    {
        _CarAudioClip = Resources.Load<AudioClip>("CharacterInteractionSounds/LeatherMoveSound");
        _CarAudioSource.clip = _CarAudioClip;
        _CarAudioSource.PlayOneShot(_CarAudioClip, 1);
        yield return new WaitForSeconds(4);
        _PlayLeatherMove = false;
        _PlayLeatherMovePlayed = true;
    }
    IEnumerator PlayBreathMove()
    {
        _CharacterCameraAudioSource.volume = 0.1f;
        _CharacterCameraAudioSource.pitch = 1f;
        _CharacterCameraAudioSource.Play();
        yield return new WaitForSeconds(1);
        _CharacterCameraAudioSource.Stop();
    }
    private void PlayOpenDoorAudio()
    {
        _CarAudioClip = Resources.Load<AudioClip>("Car/CarOpenDoor");
        _CarAudioSource.clip = _CarAudioClip;
        _CarAudioSource.PlayOneShot(_CarAudioClip, 1);
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
                    StartCoroutine(PlayBreathMove());
                    _PlayLeatherMovePlayed = true;
                    if (_PlayLeatherMovePlayed && _PlayOpenDoorPlayed == false)
                    {
                        PlayOpenDoorAudio();
                        _CharacterFadeCameraAnimator.SetBool("BlackScreen", false);
                        _CharacterFadeCameraAnimator.transform.gameObject.SetActive(true);
                        StartCoroutine(DelayedCharacterFadeIn(3));
                        _PlayOpenDoorPlayed = true;
                        StopAllCoroutines();
                        SwapCameras();
                        StartCoroutine(AghMyHeadText());
                        StartCoroutine(DelayLookPhone());
                        StartCoroutine(CharacterMovingCoroutine());
                        StartCoroutine(ShowDelayedText7());
                        StartCoroutine(HideDelayedText7());
                    }

                }
            }

        }
    }
    private IEnumerator DelayLookPhone()
    {
        yield return new WaitForSecondsRealtime(8);

        _CharacterBodyAnimator.SetBool("UsePhone", true);
        _CharacterCameraAnimator.SetBool("LookPhone", true);
    }
    IEnumerator CharacterMovingCoroutine()
    {
        _CharacterCameraAnimator.SetBool("LookPhone", false);
        _CharacterINTROMasterAnimator.SetBool("IntroWalk", true);
        _CharacterCameraAnimator.SetBool("Moving", true);
        yield return new WaitForSeconds(1);
        if (_CharacterMasterObject.GetComponent<Rigidbody>().velocity.magnitude > 0.5f)
        {
            Debug.Log("WALKING");
            PlayFootstepsAudio();
        }
    }
    private void PlayFootstepsAudio()
    {
        _CharacterBodyAudioClip = Resources.Load<AudioClip>("FootstepsSounds/WoodWalk");
        _CharacterBodyAudioSource.clip = _CharacterBodyAudioClip;
        _CharacterBodyAudioSource.loop = true;
        _CharacterBodyAudioSource.pitch = 0.6f;
        _CharacterBodyAudioSource.volume = 1f;
    }
    IEnumerator EndIntroFadeOut()
    {
        yield return new WaitForSeconds(1);
        _CharacterFadeCameraAnimator.SetBool("FadeOut", true);
    }
}
