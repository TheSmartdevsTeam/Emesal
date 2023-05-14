using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerStatusController : MonoBehaviour
{
    //LIFE
    [HideInInspector]
    public float _CurrentHealthPoints;
    public float _MaxHealthPoints;
    public float _HPRegenerationSpeed;
    public bool _HPRegenerate;

    public float delayToRegenerate; // Delay to start regenerating.
    private float nextRegenerationTime;

    //MANA
    public float _CurrentManaPoints;
    public float _MaxManaPoints;
    public float _MPRegenerationSpeed;
    public bool _MPRegenerate;
    

    //STAMINA
    public float _CurrentStaminaPoints;
    public float _MaxStaminaPoints;

    //MOVESPEED
    public float _CurrentMoveSpeed;
    public float _MaxMoveSpeed;

    //CHARACTER DAMAGE
    public float _CurrentMeleeDamage;
    public float _CurrentSpellDamage;
    
    //STATS
    public float _Strength;
    public float _Constitution;
    public float _Agility;
    public float _Dexterity;
    public float _Inteligence;

    public float _Weigth;
    public float _MaxWeigth;



    public bool _BackpackEquiped;
    public bool _SkillBookEquiped;


    //NEGATIVE STATUS
    public float _NegativeStatusModifier;




    private float _HigherHeight = 0.0f;  // Distance to the ground of the last fall.

    public GUIStyle _HealthLayer;
    public Image _HealthImage;
    public GameObject _HPIMage;

    [Space()]
    public AudioClip _heartBeat;

    [Header("Fall Damage")]
    public float _HeightThreshold;  // The max height that player can fall without hurting yourself.
    public float _FallDamageMultiplier;  // The damage multiplier increases the damage in the fall, to make it more realistic.

    public AudioManagerScript _AudioManager; // The audio manager.
    public PlayerUIScript _UI; // The player UI.
    public CameraAnimationsScript _CameraAnimations;
    public CameraEffectsScript _CameraEffects;
    public Vector3 v;

    private PlayerControllerScript _MoveController;
    private CapsuleCollider _PlayerCapsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        /*
        _MaxWeigth = 200;
        _MaxHealthPoints = 100 + (_Strength + _Constitution)/2;
        _CurrentHealthPoints = _MaxHealthPoints;
        _HPRegenerationSpeed = _Constitution / _Strength / 10;
        
        _MaxManaPoints = 100 + _Inteligence;
        _MPRegenerationSpeed = _Inteligence / 10;

        _MaxStaminaPoints = 100 + (_Constitution + _Strength + _Dexterity) / 3;

        _Weigth = 85+(_Strength-_Constitution)/2;

        _MaxMoveSpeed = 2+(_Dexterity/_Weigth);
        */
        _MoveController = GetComponent<PlayerControllerScript>();
        _PlayerCapsuleCollider = GetComponent<CapsuleCollider>();
        
        //_HPIMage = GameObject.FindGameObjectWithTag("_MaxHPImageTag");

        //v.y = 0.08f;
        //v.x = 3;
        //v.z = 0;


    }



    // Update is called once per frame
    void Update()
    {

        //_HPIMage.transform.localScale = v;

        CheckFalling(); // Checks if player is falling.

        _CurrentHealthPoints = Mathf.Clamp(_CurrentHealthPoints, 0, _MaxHealthPoints); // Prevents the amount of hp from being less than 0 or greater than the maximum of HP.
        //_CurrentStaminaPoints= Mathf.Clamp(_CurrentStaminaPoints, 0, _MaxStaminaPoints);
        //_CurrentManaPoints = Mathf.Clamp(_CurrentManaPoints, 0, _CurrentManaPoints);


        // Regenerates Hp                              
        if (_CurrentHealthPoints < _MaxHealthPoints && _CurrentHealthPoints > (_MaxHealthPoints-(_MaxHealthPoints/3)))
        {
            _CurrentHealthPoints += Time.deltaTime * _HPRegenerationSpeed;
        }
        else if (_CurrentHealthPoints < _MaxHealthPoints && _CurrentHealthPoints < (_MaxHealthPoints - (_MaxHealthPoints / 3)) && _CurrentHealthPoints > _MaxHealthPoints/2)
        {
            _CurrentHealthPoints += Time.deltaTime * (_HPRegenerationSpeed - _NegativeStatusModifier);
        }
        else if(_CurrentHealthPoints < _MaxHealthPoints && _CurrentHealthPoints < _MaxHealthPoints/2 && _CurrentHealthPoints > _MaxHealthPoints/3)
        {
            _CurrentHealthPoints += Time.deltaTime * (_HPRegenerationSpeed - _NegativeStatusModifier*2);
            
        }
        else if((_CurrentHealthPoints < _MaxHealthPoints && _CurrentHealthPoints < _MaxHealthPoints / 3 && _CurrentHealthPoints > 0)){
            _CurrentHealthPoints += Time.deltaTime * (_HPRegenerationSpeed - _NegativeStatusModifier * 3);
            _AudioManager.isDying = _CurrentHealthPoints < _MaxHealthPoints * 0.35f;
            _AudioManager.PlayHeartbeatSound(_heartBeat, _CurrentHealthPoints, _MaxHealthPoints * 0.3f);
        }



    }


    private void CheckFalling()
    {
        
        // If the player is touching the ground.
        if (_MoveController.Grounded && _HigherHeight > 0.0f)
        {
            // If the height of the drop is greater than the limit, Applies damage to the player.
            if (_HigherHeight > _HeightThreshold)
            {
                FallDamage(Mathf.Round(_FallDamageMultiplier * -Physics.gravity.y * (_HigherHeight - _HeightThreshold)));
                _CameraAnimations.FallShake();
            }
            else if (_HigherHeight >= _MoveController._JumpSpeed / 6.2f)
            {
                // Shakes the camera when hit the ground.
                if (_CameraAnimations != null)
                    _CameraAnimations.FallShake();
            }

            // Reset the value to be able to calculate a new fall.
            _HigherHeight = 0.0f;
        }
        else if (!_MoveController.Grounded)
        {
            // Calculates the distance to the ground and takes the longest distance (the highest point that you have fallen).
            if (CheckDistanceBelow() > _HigherHeight)
                _HigherHeight = CheckDistanceBelow();
        }
    }

    public void FallDamage(float damage)
    {
        ApplyDamage(damage);
        _UI.ShowCircularIndicator();
    }


    /// <summary>
    /// Calculates the distance from the current position to a surface near the bottom of the player.
    /// </summary>
    public float CheckDistanceBelow()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.SphereCast(transform.position, _PlayerCapsuleCollider.radius, Vector3.down, out hit, _HeightThreshold * 10))
        {
            return hit.distance;
        }
        else
        {
            // If the distance is too big, returns a big value that when the player hit the ground will die instantly.
            return _HeightThreshold * 10;
        }
    }

    

    /// <summary>
    /// Applies damage to the player.
    /// Parameters: The amount of Hp the player will lose.
    /// </summary>
    private void ApplyDamage(float damage)
    {
        if (_CurrentHealthPoints > 0 && damage > 0)
        {
            _CurrentHealthPoints -= damage;
            _UI.ShowHitScreen();
        }
        nextRegenerationTime = Time.time + delayToRegenerate;
    }
}
