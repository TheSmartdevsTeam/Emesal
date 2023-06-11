using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CharacterIntroScript : MonoBehaviour
{
    AudioSource _CharacterCameraAudioSource;
    AudioClip _CharacterCameraAudioClip;
    public GameObject _CharacterCameraObject;
    public GameObject _CharacterFadeCamera;
    GameObject _IntroSystemsManager;
    GameObject _RockSlideObject;
    GameObject _OwlObject;
    bool _RockSlidePlayed;
    bool _ScaredBreathPlayed;
    bool _BreathNormal;
    bool _OwlPlayed;
    GameObject _Planks;
    AudioClip _PlanksAudioClip;
    bool _PlanksSquik;
    public bool _PlanksBreak;
    GameObject _FeetSoundObject;
    AudioClip _FeetAudioClip;
    public bool _EndIntro;
    public LoadWorldScene load;
    public GameObject _LoadingObject;

    public GameObject _PostProcessingObject;
    Volume _PostProcessingVolume;

    Animator _CharacterBodyAnimator;

    public AudioSource _CharacterMasterAudioSource;
    public AudioSource _CharacterBodyAudioSource;
    public AudioSource _GameAmbientAudioSource;
    public AudioSource _GameMusicAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        _CharacterCameraAudioSource = transform.GetChild(1).GetComponent<AudioSource>();
        _IntroSystemsManager = GameObject.FindGameObjectWithTag("GameSystemsManager");
        _RockSlideObject = _IntroSystemsManager.transform.GetChild(3).gameObject;
        _OwlObject = _IntroSystemsManager.transform.GetChild(4).gameObject;
        _RockSlidePlayed = false;
        _ScaredBreathPlayed = false;
        _OwlPlayed = false;
        _FeetSoundObject = transform.GetChild(11).gameObject;
        //_PostProcessingObject = _IntroSystemsManager.transform.GetChild(0).gameObject;
        _PostProcessingVolume = _PostProcessingObject.GetComponent<Volume>();
        _CharacterBodyAnimator = transform.GetChild(0).GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        DecreaseBreathVolume();
    }
    private void DecreaseBreathVolume()
    {
        if (_BreathNormal)
        {
            if (_CharacterCameraAudioSource.volume > 0.03f)
            {
                _CharacterCameraAudioSource.volume -= 0.001f;
            }
            else
            {
                _CharacterCameraAudioSource.volume = 0.03f;
            }
            if (_CharacterCameraAudioSource.pitch > 1f)
            {
                _CharacterCameraAudioSource.pitch -= 0.001f;
            }
            else
            {
                _CharacterCameraAudioSource.pitch = 1f;
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RockSlideObject")
        {
            if (_RockSlidePlayed == false)
            {
                _RockSlideObject.GetComponent<AudioSource>().Play();
                _RockSlidePlayed = true;
                _FeetSoundObject.SetActive(true);
            }

        }
        if (collision.gameObject.name == "BreathObject")
        {
            _CharacterCameraAudioSource.volume = 0.3f;
            _CharacterCameraAudioSource.pitch = 1.3f;

        }
        if (collision.gameObject.name == "BreathNormal")
        {
            _BreathNormal = true;
        }


        if (collision.gameObject.name == "OwlObject")
        {
            if (_OwlPlayed == false)
            {
                _OwlObject.GetComponent<AudioSource>().Play();
                _OwlPlayed = true;

            }
        }
        
        if (collision.gameObject.name == "GrassNoise")
        {
            _OwlObject.GetComponent<AudioSource>().enabled = true;
        }
        if (collision.gameObject.name == "Walk" || collision.gameObject.name == "Walk2")
        {
            _FeetSoundObject.SetActive(true);
        }
        if (collision.gameObject.name == "StopWalk" || collision.gameObject.name == "StopWalk2")
        {
            _FeetSoundObject.SetActive(false);
        }
        if (collision.gameObject.name == "WoodWalk")
        {
            _FeetSoundObject.SetActive(true);
            _FeetAudioClip = Resources.Load<AudioClip>("FootstepsSounds/WoodWalk");
            _FeetSoundObject.GetComponent<AudioSource>().clip = _FeetAudioClip;
            _FeetSoundObject.GetComponent<AudioSource>().Play();
        }

        if (collision.gameObject.name == "Planks")
        {
            _FeetSoundObject.GetComponent<AudioSource>().volume = 1;
            _FeetSoundObject.GetComponent<AudioSource>().pitch = 1;

            _Planks = collision.gameObject;
            _FeetSoundObject.SetActive(false);
            
            _CharacterBodyAnimator.SetBool("UsePhone", false);
            if (_PlanksSquik == false)
            {
                _PlanksAudioClip = Resources.Load<AudioClip>("PropsSounds/PlanksSquik");
                _Planks.GetComponent<AudioSource>().PlayOneShot(_PlanksAudioClip);
                _PlanksSquik = true;

            }
            if (_PlanksBreak == false)
            {
                _Planks.GetComponent<AudioSource>().Stop();
                _PlanksAudioClip = Resources.Load<AudioClip>("PropsSounds/PlanksBreaking");
                _Planks.GetComponent<AudioSource>().PlayOneShot(_PlanksAudioClip);
                _PlanksBreak = true;
            }
        }

        if (collision.gameObject.name == "EndIntro")
        {
            StartCoroutine(ShowLoadingScreen());  
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Planks")
        {
            _FeetSoundObject.GetComponent<AudioSource>().volume = Random.Range(0.6f, 1f);
            _FeetSoundObject.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }

        if (collision.gameObject.name == "StopWalk" || collision.gameObject.name == "StopWalk2")
        {
            DisabledAndAdjustPPO();
            _CharacterCameraObject.GetComponent<Animator>().SetBool("Moving", false);
        }
        else
        {
            _CharacterCameraObject.GetComponent<Animator>().SetBool("Moving", true);
        }
        
    }
    private void DisabledAndAdjustPPO()
    {
        _PostProcessingObject.SetActive(true);
        for (int i = 0; i < _PostProcessingVolume.sharedProfile.components.Count; i++)
        {
            if (_PostProcessingVolume.sharedProfile.components[i].name.ToString() == "LensDistortion")
            {
                UnityEngine.Rendering.Universal.LensDistortion _LensDistortion = (UnityEngine.Rendering.Universal.LensDistortion)_PostProcessingVolume.sharedProfile.components[i];
                _LensDistortion.active = true;
                _LensDistortion.SetAllOverridesTo(true);
                _LensDistortion.intensity.value = 1f;
                _LensDistortion.xMultiplier.value = Random.Range(0.1f, 0.1f);
                _LensDistortion.yMultiplier.value = Random.Range(0.1f, 0.1f);
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
                _DepthOfField.focalLength.value = 0;
                _DepthOfField.aperture.value = 1;
                _DepthOfField.bladeCount.value = 3;
                _DepthOfField.bladeCurvature.value = 1;
                _DepthOfField.bladeRotation.value = 0;
            }
        }
    }
    IEnumerator ShowLoadingScreen()
    {
        _CharacterFadeCamera.SetActive(false);
        _LoadingObject.SetActive(true);
        _CharacterCameraAudioSource.Stop();
        _CharacterMasterAudioSource.Stop();
        _CharacterBodyAudioSource.Stop();
        _GameAmbientAudioSource.Stop();
        _GameMusicAudioSource.Stop();
        yield return new WaitForSeconds(5);
        EndIntro();
    }
    public void EndIntro()
    {
        _EndIntro = true;    
        load.LoadNextSceneWithLoadingBar(3);
    }
}
