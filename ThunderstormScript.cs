using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderstormScript : MonoBehaviour
{
    Light _LightningFlash;
    float _LightningFLashTimer;
    public static bool _LightningFlashBool;
    void Start()
    {
        if(GetComponent<Light>() != null)
        {
            _LightningFlash = GetComponent<Light>();
        }
        _LightningFLashTimer = 20;
        _LightningFlashBool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_LightningFLashTimer > 0)
        {
            _LightningFLashTimer -= Time.deltaTime;
        }
        else
        {
            _LightningFLashTimer = 0;
        }

        if (_LightningFLashTimer == 0)
        {
            _LightningFlash.intensity = Random.Range(1000, 1500);
            StartCoroutine(playMainMenuLightningFlash());
            _LightningFlashBool = true;
            _LightningFLashTimer = Random.Range(30, 60);
            return;
        }
        _LightningFlashBool = false;
    }

    IEnumerator playMainMenuLightningFlash()
    {
        _LightningFlash.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _LightningFlash.enabled = false;
        yield return new WaitForSeconds(0.1f);
        _LightningFlash.enabled = true;
        _LightningFlash.enabled = false;
    }
}
