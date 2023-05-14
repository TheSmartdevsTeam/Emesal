using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    Animator _Animator;
    public AudioClip _OpenDoorClip;
    public AudioClip _CloseDoorClip;
    AudioSource _AudioSource;
    Collider col;
    // Start is called before the first frame update
    void Start()
    {

        _Animator = GetComponent<Animator>();
        _Animator.SetBool("OpenDoor", false);
        _AudioSource = GetComponent<AudioSource>();
        col = GetComponent<Collider>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && GetComponent<Animator>().GetBool("OpenDoor") == false)
        {
            _Animator.SetBool("OpenDoor", true);
            //_CharacterAnimator.SetBool("DoorOpen", true);
            _AudioSource.PlayOneShot(_OpenDoorClip);

        }
        else if(Input.GetKeyDown(KeyCode.F) && GetComponent<Animator>().GetBool("OpenDoor") == true)
        {
            _Animator.SetBool("OpenDoor", false);
            //_CharacterAnimator.SetBool("DoorOpen", false);
            _AudioSource.PlayOneShot(_CloseDoorClip);

        }

        
    }

}
