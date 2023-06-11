using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlCallScript : MonoBehaviour
{
    AudioSource _AudioSource;
    AudioClip _AudioClip; 
    float _OwlTimer;  
    public static bool OwlCall;

    void Start()
    {
        if (GetComponent<AudioSource>() != null)
        {
            _AudioSource = GetComponent<AudioSource>();
            _AudioClip = Resources.Load<AudioClip>("Animals/Owl");
        }
        _OwlTimer = 10;
        OwlCall = false;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (_OwlTimer > 0)
        {
            _OwlTimer -= Time.deltaTime;
        }
        else
        {
            _OwlTimer = 0;
        }

        if (_OwlTimer == 0)
        {
            _AudioSource.Stop();
            StartCoroutine(playMainMenuOwlSound());
            OwlCall = true;
            _OwlTimer = Random.Range(20, 50);
            return;
        }
        OwlCall = false;
    }

    IEnumerator playMainMenuOwlSound()
    {
        _AudioSource.loop = true;
        _AudioSource.Play();
        _AudioSource.volume = Random.Range(0.4f, 1f);
        _AudioSource.pitch = Random.Range(0.7f, 0.95f);
        yield return new WaitForSeconds(_AudioClip.length+Random.Range(0.1f,1));
        _AudioSource.loop = false;
    }

    
}
