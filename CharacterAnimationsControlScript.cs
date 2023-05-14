using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterAnimationsControlScript : MonoBehaviour
{
    // Start is called before the first frame update

    Animator _Animator;
    Animator _TargetAnimator;
    public AudioManagerScript audioManager; // Audio Manager.
    public AudioClip EffectSound; // Fire sound.
    public float EffectSoundVolume = 1f; // Fire sound volume.
    public AudioSource SoundEffectSourceFar;
    public ParticleSystem WeaponParticleEffect;

    bool _CarryStaff;
    
    bool _CarryWeapon;

    GameObject _Staff;
    GameObject PickedObject;

    bool aiming;
    GameObject _player;
    GameObject _ObjectCarryModule;
    GameObject _ObjectReadyModule;

    public float _RaycastDistance;
    [SerializeField]
    public float _Weapon_X_Rotation;
    public float _Weapon_Y_Rotation;
    public float _Weapon_Z_Rotation;
    public float _Weapon_X_Position;
    public float _Weapon_Y_Position;
    public float _Weapon_Z_Position;

    void Start()
    {
        _Animator = GetComponent<Animator>();
        _RaycastDistance = 10;
        _player = GameObject.FindGameObjectWithTag("Player");
        //_ObjectCarryModule = GameObject.FindGameObjectWithTag("OCM");
        //_ObjectReadyModule = GameObject.FindGameObjectWithTag("ORM");
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            //InteractWithObject();
            if (GetComponent<Animator>().GetBool("DoorOpen") == false)
            {
                GetComponent<Animator>().Play("Open Door State");
            }
            else
            {
                GetComponent<Animator>().Play("Close Door State");
            }
            
        }
        */
        /*
        if (_CarryStaff == true)
        {
            _Weapon_X_Rotation = 0.66f;
            _Weapon_Y_Rotation = -6.6f;
            _Weapon_Z_Rotation = -14.5f;
            _Weapon_X_Position = 1;
            _Weapon_Y_Position = 0.997f;
            _Weapon_Z_Position = 1f;
        }
        */
        /*
        if (_CarryWeapon)
        {
            if (Input.GetKeyDown(KeyCode.R) && _Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle State"))
            {
                _Animator.SetBool("DrawWeaponBool", true);
            }

            if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Draw Weapon State"))
            {
                //Debug.Log("DRAW WEAPON STATE DETECETED");
                _Animator.SetBool("DrawWeaponBool", false);
                _Animator.SetBool("WeaponReadyBool", true);

            }
            if (Input.GetKeyDown(KeyCode.R) && _Animator.GetCurrentAnimatorStateInfo(0).IsName("Weapon Ready State"))
            {
                //Debug.Log("WEAPON READY STATE DETECETED");
                _Animator.SetBool("SheatheWeaponBool", true);
                _Animator.SetBool("WeaponReadyBool", false);
            }
            if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle State"))
            {
                _Animator.SetBool("SheatheWeaponBool", false);
            }
            if (Input.GetKeyDown(KeyCode.Mouse0) && _Animator.GetCurrentAnimatorStateInfo(0).IsName("Weapon Ready State"))
            {
                _Animator.Play("Weapon Attack State");
            }
        }*/
        if (PickedObject != null)
        {
            SetDrawWeaponStateLocations();
        }


        /*
        if (Input.GetKeyDown(KeyCode.Mouse1) && _Animator.GetBool("AimingLoopBool")==false)
        {
            
            _Animator.SetBool("AimingLoopBool", true);
            _Animator.SetBool("SetAimingBool", true);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && _Animator.GetBool("AimingLoopBool") == true)
        {
            _Animator.SetBool("AimingLoopBool", false);
            _Animator.SetBool("SetAimingBool", false);
            
            
        }
        */

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {

            _Animator.SetBool("SpellCasting Ready",true);
            _Animator.SetBool("SpellCharge", true);
        }
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            _Animator.SetBool("SpellCasting Ready", false);
            _Animator.SetBool("SpellCharge", false);
        }
    }
    public void InteractWithObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.transform.tag);
                if (hit.collider.transform.tag == "Door")
                {
                    _Animator.Play("Open Door State");
                    _TargetAnimator = hit.collider.GetComponent<Animator>();
                    if (_TargetAnimator.GetBool("InteractBool") == true)
                    {
                        _TargetAnimator.SetBool("InteractBool", false);
                    }
                    else
                    {
                        _TargetAnimator.SetBool("InteractBool", true);
                    }
                }
                else if (hit.collider.transform.tag == "Weapon")
                {
                    _TargetAnimator = GetComponent<Animator>();
                    _TargetAnimator.Play("Collect State");
                    //StartCoroutine(WaitCoorutine());
                    PickedObject = hit.collider.gameObject;
                    Destroy(hit.collider.gameObject);

                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.drag = 0;
                    rb.angularDrag = 0;
                    Collider col = hit.collider;
                    col.enabled = false;
                    PickedObject = hit.transform.gameObject;
                    PickedObject = Instantiate(PickedObject, transform.position, Quaternion.identity);
                    _CarryStaff = true;
                    Debug.Log(PickedObject.name);
                }
            }
        }
        _CarryWeapon = true;
    }

    public void WeaponAnimations()
    {
        if (_CarryWeapon == true && _Animator.GetCurrentAnimatorStateInfo(0).IsName("Draw Weapon State"))
        {
            Debug.Log("DONE2");
            //Debug.Log(_Animator.GetCurrentAnimatorStateInfo(0).ToString());
            _Animator.SetBool("WeaponReadyBool", true);
            _Animator.Play("Weapon Ready State");
        }
        else
        {

            //_Animator.Play("Draw Weapon State");
        }


        if (Input.GetKeyDown(KeyCode.R) && _Animator.GetBool("WeaponReadyBool") == true && _Animator.GetBool("DrawWeaponBool") == true && _Animator.GetCurrentAnimatorStateInfo(0).IsName("Weapon Ready State"))
        {

            _Animator.SetBool("WeaponReadyBool", false);
            _Animator.SetBool("DrawWeaponBool", false);
            _Animator.SetBool("SheatheWeaponBool", true); ;
            if (_Animator.GetBool("SheatheWeaponBool") == true && _Animator.GetCurrentAnimatorStateInfo(0).IsName("Weapon Ready State"))
            {
                _Animator.Play("Sheathe Weapon State");
                //_Animator.SetBool("SheatheWeaponBool", false);
            }



        }

        if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle State"))
        {
            _Animator.SetBool("WeaponReadyBool", false);
            _Animator.SetBool("DrawWeaponBool", false);
            _Animator.SetBool("SheatheWeaponBool", false);
        }
    }

    public void SetDrawWeaponStateLocations()
    {
        if(_Animator.GetCurrentAnimatorStateInfo(0).IsName("Weapon Ready State"))
        {
            //PickedObject.transform.position = new Vector3(_ObjectReadyModule.transform.position.x, _ObjectReadyModule.transform.position.y, _ObjectReadyModule.transform.position.z);
            _Weapon_X_Rotation = 0.376f;
            _Weapon_Y_Rotation = -1.501f;
            _Weapon_Z_Rotation = -71.794f;
            PickedObject.transform.position = _ObjectReadyModule.transform.position;
            PickedObject.transform.rotation = _ObjectReadyModule.transform.rotation;
            //_ObjectCarryModule.transform.position = new Vector3(_ObjectCarryModule.transform.position.x * 0.148f,
            //_ObjectCarryModule.transform.position.y * 0.156f, _ObjectCarryModule.transform.position.z * 0.337f);

        }
        else
        {
            PickedObject.transform.position = _ObjectCarryModule.transform.position;
            PickedObject.transform.rotation = _ObjectCarryModule.transform.rotation;
            //PickedObject.transform.Rotate(new Vector3(_Weapon_X_Rotation, _Weapon_Y_Rotation, _Weapon_Z_Rotation));
        }
    }

    public void PlaySoundEffect(AudioClip clip, float volume)
    {
        SoundEffectSourceFar.PlayOneShot(clip, volume); // Plays shot sound, and scales the AudioSource volume by volume.
    }

    IEnumerator WaitCoorutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);

    }
}