using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CharacterCameraAnimationsScript : MonoBehaviour
{
    Animator _CharacterAnimator;
    public GameObject _Camera1;
    public GameObject _Player;
    // Start is called before the first frame update
    void Start()
    {
        _CharacterAnimator = _Camera1.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void LateUpdate()
    {
        if (Input.anyKey)
        {
            _Camera1.GetComponent<Animator>().SetBool("AnyKey", true);
            
            if (_Camera1.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("End"))
            {
                _Player.SetActive(true);
                _Camera1.SetActive(false);
                //_Camera1.GetComponent<Animator>().enabled = false;
                //_Camera1.gameObject.transform.parent.rotation = 
                //_Camera1.SetActive(false);
                //_Camera2.SetActive(true);
                //_CharacterAnimator = _Camera2.GetComponent<Animator>();
            }

;       }
        
        if (Input.GetKey(KeyCode.W))
        {
            _CharacterAnimator.SetBool("Moving",true);
            
        }
        else
        {
            _CharacterAnimator.SetBool("Moving", false);

        }
        /*
        if (Input.GetKeyDown(KeyCode.Mouse1) && _CharacterAnimator.GetBool("AimingLoopBool") == false)
        {
            _CharacterAnimator.SetBool("SetAimingBool", true);
            //_CharacterAnimator.SetBool("AimingLoopBool", true);

        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && _CharacterAnimator.GetBool("AimingLoopBool") == true)
        {
            _CharacterAnimator.SetBool("SetAimingBool", false);
            //_CharacterAnimator.SetBool("AimingLoopBool", false);

        }
        */
        /*

        if (Input.GetKeyDown(KeyCode.Mouse0) && _CharacterAnimator.GetBool("FireBool") == false && _CharacterAnimator.GetBool("AimingLoopBool") == true)
        {
            _CharacterAnimator.SetBool("FireBool", true);
        }


        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _CharacterAnimator.SetBool("FireBool", false);
        }

        if(_CharacterAnimator.GetBool("AimingLoopBool") ==true) {
            
        }
        */
    }


}
