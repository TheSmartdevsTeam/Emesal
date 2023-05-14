using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class PostProcessingController : MonoBehaviour
{
    public GameObject PPO;
    public PostProcessProfile PPP;


    private AmbientOcclusion _AmbientOcclusion;
    private ColorGrading _ColorGrading;
    private Vignette _Vignette;

    public PostProcessingController(Vignette vignette)
    {
        _Vignette = vignette;
    }


    private void Start()
    {


       
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            PPO.active = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            PPO.active = true;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            PPO.GetComponent<PostProcessProfile>().AddSettings<PostProcessEffectSettings>();
            PPP = PPO.GetComponent<PostProcessProfile>();

            PostProcessEffectSettings postProcessEffectSettings = PPP.AddSettings(_Vignette);
            _Vignette.active = true;
            
        }

    }

    public void AmbientOcclusionOnOff(bool value)
    {
        //_AmbientOcclusion.active = true;
        //_ColorGrading.active = true;
       // _Vignette.active = true;
        
    }
}


