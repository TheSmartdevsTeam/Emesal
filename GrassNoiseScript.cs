using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassNoiseScript : MonoBehaviour
{

    AudioSource _AudioSource;
    AudioClip _AudioClip;

    float _GrassNoiseTimer;
    public static bool GrassNoise;

    // Start is called before the first frame update
    void Start()
    {

        if (GetComponent<AudioSource>() != null)
        {
            _AudioSource = GetComponent<AudioSource>();
            _AudioClip = Resources.Load("Grass Noise") as AudioClip;
        }

        _GrassNoiseTimer = 15;
        GrassNoise = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_GrassNoiseTimer > 0)
        {
            _GrassNoiseTimer -= Time.deltaTime;
        }
        else
        {
            _GrassNoiseTimer = 0;
        }

        if (_GrassNoiseTimer == 0)
        {
            _AudioSource.Stop();
            StartCoroutine(playMainMenuGrassNoise());
            GrassNoise = true;
            _GrassNoiseTimer = Random.Range(30, 60);
            //Debug.Log(_GrassNoiseTimer);
            return;
        }
        GrassNoise = false;
    }

    IEnumerator playMainMenuGrassNoise()
    {
        _AudioSource.loop = true;
        _AudioSource.Play();
        _AudioSource.volume = Random.Range(0.7f, 1f);
        _AudioSource.pitch = Random.Range(0.6f, 0.95f);
        yield return new WaitForSeconds(_AudioClip.length);
        _AudioSource.loop = false;
    }
}
