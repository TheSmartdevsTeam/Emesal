using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class BackpackScript : MonoBehaviour
{
    public bool _BackpackActive;
    Animator _BackpackAnimator;
    
    void Start()
    {
        _BackpackAnimator = GetComponent<Animator>();
        _BackpackActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && _BackpackActive == false)
        {
            _BackpackAnimator.SetBool("Active", true);
        }

        if (Input.GetKeyDown(KeyCode.Tab) && _BackpackActive == true)
        {
            _BackpackAnimator.SetBool("Active", false);
        }
    }
}
