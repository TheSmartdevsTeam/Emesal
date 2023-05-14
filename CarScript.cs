using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public bool _FadeOut;
    public bool _StopingCar;
    void Start()
    {
        _FadeOut = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name == "FadeOutObject")
        {
            _FadeOut = true;
        }

        if (collider.gameObject.name == "StopingCarObject")
        {
            _StopingCar = true;

        }
    }

    public bool _GetFadeOut()
    {
        return _FadeOut;
    }

    public bool _GetStopingCar()
    {
        return _StopingCar;
    }

    public void _SetStopingCar(bool value)
    {
        _StopingCar = value;
    }

}
