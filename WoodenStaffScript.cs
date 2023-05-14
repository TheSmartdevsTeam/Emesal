using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenStaffScript : MonoBehaviour
{
    Animator _Animator;
    AudioManagerScript audioManager; // Audio Manager.
    public AudioSource _EffectSound; // Fire sound.
    float EffectSoundVolume = 1f; // Fire sound volume.
    public bool _AnimatorBoolState;
    public GameObject _WoodenStaff;

    Camera _CharacterCamera;
    public GameObject _WoodenStaff_EQUI_SLOT;
    public GameObject _WoodenStaff_IDLE_SLOT;
    public GameObject _WoodenStaff_HOLS_SLOT;
    [SerializeField] private AudioClip _StaffAttackSound;

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponent<Animator>();
        _CharacterCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        //NORMAL ATTACK
        if (Input.GetKeyDown(KeyCode.Mouse0) && _Animator.GetCurrentAnimatorStateInfo(0).IsName("Aim Idle Animation") == true)
        {
            _Animator.Play("Fire Animation");
            _Animator.SetBool("FireBool", true);
            _EffectSound.clip = _StaffAttackSound;
            _EffectSound.Play();

            //GameObject _FireEffectClone = (GameObject)Instantiate(_FireEffectOriginal, transform.parent);
            //Destroy(_FireEffectClone, 0.3f);
            _Animator.SetBool("FireBool", false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && _Animator.GetBool("ReadyWeapon") == false)
        {
            _Animator.SetBool("ReadyWeapon", true);

        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && _Animator.GetBool("ReadyWeapon") == true)
        {
            _Animator.SetBool("ReadyWeapon", false);
        }

    }
}