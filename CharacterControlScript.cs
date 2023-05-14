using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class CharacterControlScript : MonoBehaviour
{
    #region VARIABLES
    private ScanTerrainTextureUnderCharacter SCANT;
    
    CharacterController _CharacterController;
    AudioSource _CharacterAudioSource;
    [SerializeField] private AudioClip _CharacterJumpSound;
    [SerializeField] private AudioClip _CharacterBreathClip;
    [SerializeField] private AudioClip _CharacterLandSound;
    [SerializeField] private AudioClip _CharacterRunningClip;

    public List<AudioClip> _CharacterFoostepsSounds;

    Animator _CharacterAnimator;
    private int _CharacterCurrentAnimatorState;
    private int _CharacterNextAnimatorState;

    private CollisionFlags _CollisionFlags;

    private Camera _CharacterCamera;
    CameraDetectionScript CDS;
    private Transform _CharacterTransform;
    [SerializeField] private _MouseLook _Cursor;
    Vector2 _CursorLastLocation;
    private Vector3 _OriginalCharacterCameraPosition;
    private Animator _CharacterCameraAnimator;

    //MOVEMENT
    private bool _CharacterWasGrounded;
    private bool _CharacterGrounded;
    private bool _CharacterCrouching;
    private float _CharacterCrouchSpeed;
    private bool _CharacterWalking;
    private float _CharacterWalkSpeed;
    private bool _CharacterRunning;
    private float _CharacterRunSpeed;
    private bool _CharacterJumped;
    private float _CharacterJumpSpeed;
    private bool _CharacterInAir;
    private bool _LandingProcedureDone;
    private Vector3 _CharacterMoveDirection;
    private float _CharacterCurrentMoveSpeed;
    private float _CharacterMinimumMoveSpeed;
    private float _CharacterMoveMultiplier;
    public bool _CharacterUnderwater { get; private set; }
    private float _CharacterShallowWaterMoveSpeed;
    private float _CharacterUnderWaterMoveSpeed;
    private bool _CharacterSwimming;
    private float _CharacterSwimmingSpeed;
    private float _CharacterSwimingWaterDepth;
    private float _CharacterWaterSurfaceHeight;
    private float _CharacterCurrentWaterDepth;
    private float _CharacterNextStep;
    public float _CharacterStepCycle;
    public float _CharacterWalkStep;
    public float _CharacterRuningStep;

    public bool isSliding { get; private set; }
    //ACTION
    private bool _CharacterAiming;
    private bool _CharacterAttacking;
    private bool _CharacterChanneling;
    private bool _CharacterCasting;
    //ATTRIBUTES
    CharacterAttributesScript _CharacterAttributesScript;
    private bool _CharacterStaminaDrained;
    private float _CharacterHeight;
    private float _CharacterWeigth;
    private float _CharacterMaxHealth;
    private float _CharacterCurrentHealth;
    private float _CharacterMaxStamina;
    private float _CharacterCurrentStamina;
    private float _CharacterMaxMana;
    private float _CharacterCurrentMana;
    private Transform _CharacterBodyObject;

    [SerializeField] private float _GravityForce;
    private Vector2 _MovementInput;

    public GameObject _CharacterUIMaster;
    private GameObject _Inventory;
    private GameObject _Skills;
    private GameObject _CharacterSheets;
    

    
    [SerializeField] GameObject _ChargeToUse;
    [SerializeField] GameObject _ChargeSocket;

    [SerializeField] GameObject _ProjectileToUse;
    [SerializeField] GameObject _ProjectileSocket;
    ProjectileScript projectileScript;
    public float _ProjectileSpeed = 0.1f;
    float _ProjectileDamage = 5;
    float _ProjectileLifeTime = 2;
    float _ProjectileCurrentLifeTime = 2;

    GameObject _ChargingSpell;
    GameObject _CastedSpell;
   
    float _Timer;
    public Transform _ProjectileSpawnPoint;

    bool _CastFlag;
    float _ChargeCastTime = 3;
    float _SpellCastTime = 0;
    float _CurrentCastTime;
    float _CastCooldown = 5;
    float _CastCurrentCooldown = 5;

    public AudioSource _SpellAudioSource;
    AudioClip _SpellSpellAudio;
    private float _CharacterAngle;
    private RaycastHit slopeHit;

    public float moveSpeed;
    private Vector3 input;
    private Vector3 GravityForceVector;
    public float velocity;
    public float gravityScale;
    #endregion
    void Start()
    {
        
        SCANT = GetComponent<ScanTerrainTextureUnderCharacter>();
        SCANT.CheckLayers();
        
        _CharacterCamera = Camera.main;

        _OriginalCharacterCameraPosition = _CharacterCamera.transform.localPosition;
        _CharacterBodyObject = transform.GetChild(0);
        _CharacterAnimator = _CharacterBodyObject.GetComponent<Animator>();
        
        _CharacterAudioSource = GetComponent<AudioSource>();
        _CharacterAudioSource.spatialBlend = 1;
        _CharacterAudioSource.rolloffMode = AudioRolloffMode.Linear;
        
        _GetRigidBody.mass = 75;
        _GetRigidBody.drag = 0;
        _GetRigidBody.angularDrag = 0.05f;
        _GetRigidBody.useGravity = true;
        _GetRigidBody.isKinematic = true;
        _GetRigidBody.interpolation = RigidbodyInterpolation.None;
        _GetRigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        _GetRigidBody.constraints = RigidbodyConstraints.FreezeRotationY;
        
        _GravityForce = 9f;
        _CharacterController = GetComponent<CharacterController>();
        _CharacterController.slopeLimit = 60;
        _CharacterController.skinWidth = 0.001f;
        _CharacterController.minMoveDistance = 0;
        _CharacterController.center = new Vector3(0, -0.23f, 0.0f);
        _CharacterController.radius = 0.22f;
        _CharacterController.height = 1.83f;
        _CharacterController.stepOffset = _CharacterController.height;

        _CharacterStepCycle = 1;
        _CharacterNextStep = _CharacterNextStep / 2;
        _CharacterWalkStep = 3.4f;
        _CharacterRuningStep = 3.4f;

        _CharacterAttributesScript = GetComponent<CharacterAttributesScript>();
        
        _CharacterWasGrounded = false;
        _CharacterJumped = false;
        _CharacterJumpSpeed = 0.5f;
        
        _CharacterRunning = false;
        _CharacterAiming = false;
        _CharacterAttacking = false;
        _CharacterStaminaDrained = false;
        _CharacterWalking = false;
        _CharacterChanneling = false;
        _CharacterCasting = false;

        _CharacterCurrentMoveSpeed = 0;
        _CharacterMinimumMoveSpeed = 0.1f;
        _CharacterCrouching = false;
        _CharacterCrouchSpeed = 2;
        _CharacterWalkSpeed = 2f;
        _CharacterRunSpeed = 4f;
        _CharacterMoveMultiplier = 2;
        _CharacterShallowWaterMoveSpeed = 0.5f;
        _CharacterSwimingWaterDepth = _CharacterController.height-0.05f;


        _CharacterSwimmingSpeed = 0.9f;

        _CharacterUnderWaterMoveSpeed = 0.1f;

        #region CHARACTER_STANCE
        _CharacterGrounded = false;
        _CharacterInAir = false;
        _CharacterSwimming = false;
        _CharacterUnderwater = false;
        #endregion CHARACTER_STANCE

        _CharacterCamera = Camera.main;
        _CharacterTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _Skills = _CharacterUIMaster.transform.GetChild(0).GetChild(2).gameObject;
        _Inventory = _CharacterUIMaster.transform.GetChild(0).GetChild(0).gameObject;
        _CharacterSheets = _CharacterUIMaster.transform.GetChild(0).GetChild(1).gameObject;
        
        //_CharacterCameraAnimator = GetComponent<Animator>();

        _Cursor = gameObject.AddComponent<_MouseLook>();

        _Cursor.XSensitivity = 2;
        _Cursor.YSensitivity = 2;
        _Cursor.MinimumX = -70;
        _Cursor.MaximumX = 70;
        _Cursor.clampVerticalRotation = true;
        _Cursor.smooth = false;
        _Cursor.smoothTime = 5;
        //_CursorLastLocation = Input.mousePosition;
        //_Cursor.lockCursor = true;
        //_Cursor.SetCursorLock(true);


        CDS = transform.GetChild(4).GetComponent<CameraDetectionScript>();
        
    }
    private void Update()
    {

    }
    private void LateUpdate()
    {
        if (_Inventory.activeSelf == false && _Skills.activeSelf == false && _CharacterSheets.activeSelf == false)
        {
            RotateView();
        }
    }
    private void FixedUpdate()
    {
        if (CheckInventoryActive() == false)
        {
            MOVEMENT();

            #region COMBAT
            CheckAttack();
            //Interact();
            if (Input.GetKeyDown(KeyCode.F) && _CharacterAnimator.GetBool("OpenDoor") == false)
            {
                _CharacterAnimator.SetBool("OpenDoor", true);
            }
            else
            {
                _CharacterAnimator.SetBool("OpenDoor", false);
            }

            CheckSpellCast();
            #endregion
        }
    }
    #region MOVEMENT
    private void MOVEMENT()
    {
        PlayerStateDetection();
        GravityForce();
        CheckGroundMovement();
        CheckWaterMovement();
        CheckAirMovement();
    }
    private Rigidbody _GetRigidBody
    {
        get
        {
            return GetComponent<Rigidbody>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            _CharacterWaterSurfaceHeight = other.transform.position.y;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            _CharacterCurrentWaterDepth = 0;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Water")
        {
            _CharacterCurrentWaterDepth = (_CharacterWaterSurfaceHeight - (_CharacterCamera.transform.position.y - _CharacterController.height + _CharacterController.skinWidth));
        }
    }
    private bool CheckInventoryActive()
    {
        if (_Inventory.activeSelf == true || _Skills.activeSelf == true || _CharacterSheets.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void RotateView()
    {
        _Cursor.Init(_CharacterTransform, _CharacterCamera.transform);

        _Cursor.LookRotation(_CharacterTransform, _CharacterCamera.transform);
    }
    private void PlayerStateDetection()
    {
        if (_CharacterController.isGrounded && CDS._GetCharacterSwimming() == false && CDS._GetCharacterUnderwater() == false)
        {
            _CharacterGrounded = true;
            _CharacterInAir = false;
        }
        if (_CharacterController.isGrounded == false && CDS._GetCharacterSwimming() == false && CDS._GetCharacterUnderwater() == false)
        {
            _CharacterInAir = true;
        }
        
        if (CDS._GetCharacterSwimming())
        {
            _CharacterInAir = false;
            _CharacterGrounded = false;
        }
        if (CDS._GetCharacterUnderwater())
        {
            _CharacterInAir = false;
            _CharacterGrounded = false;
        }
    }
    private void GravityForce()
    {
        float tempGravity = _GravityForce;

        if (CDS._GetCharacterUnderwater())
        {
            _GravityForce = -0.1f;
            velocity = -0.1f;
        }
        if (CDS._GetCharacterSwimming())
        {
            _GravityForce = 0f;
            velocity = 0f;
        }

        if (_CharacterGrounded || _CharacterInAir)
        {
            _GravityForce = tempGravity;
            velocity += _GravityForce * gravityScale * Time.deltaTime;

            if (velocity == 0)
            {
                velocity = -_GravityForce;
            }
            else if (velocity < -10)
            {
                velocity = -10;
            }
        }
        
    }
    public Vector2 GetInputFromAxis()
    {
        Vector2 input = new Vector2
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };
        return input;
    }
    private void GetInput(out float MoveSpeed)
    {
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");
        _MovementInput = new Vector2(horizontal, vertical);

        if (_MovementInput.x == 0 && _MovementInput.y == 0)
        {
            MoveSpeed = 0;
        }
        else if (_CharacterCrouching)
        {
            MoveSpeed = _CharacterCrouchSpeed;
        }
        else if (_CharacterRunning)
        {
            MoveSpeed = _CharacterRunSpeed;
        }
        else if (_CharacterSwimming || _CharacterUnderwater)
        {
            MoveSpeed = _CharacterSwimmingSpeed;
        }
        else
        {
            MoveSpeed = _CharacterWalkSpeed;
        }
        if (_MovementInput.sqrMagnitude > 1)
        {
            _MovementInput.Normalize();
        }
    }
    private void MovePlayer()
    {
        _CharacterController.Move(new Vector3(_CharacterMoveDirection.x, velocity, _CharacterMoveDirection.z) * Time.deltaTime);
    }
    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space) && Input.anyKey)
        {
            velocity = 0;
            gravityScale = 0;
            DoJump();
            MovePlayer();
        }
    }
    private void DoJump()
    {
        gravityScale += 2f;
        velocity = Mathf.Sqrt(_CharacterJumpSpeed * -1f * (-_GravityForce * gravityScale));
        PlayJumpSound();
    }
    private void PlayJumpSound()
    {
        _CharacterAudioSource.clip = _CharacterJumpSound;
        _CharacterAudioSource.Play();
        _LandingProcedureDone = true;
        _CharacterWasGrounded = true;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_LandingProcedureDone && _CharacterWasGrounded)
        {
            PlayLandingSound();
            _LandingProcedureDone = false;
            _CharacterWasGrounded = false;
        }
    }
    private void PlayLandingSound()
    {
        _CharacterAudioSource.clip = _CharacterLandSound;
        _CharacterAudioSource.PlayOneShot(_CharacterLandSound);
        _CharacterInAir = false;
        _CharacterNextStep += 0.5f;
    }
    private void CheckGroundMovement()
    {
        if (_CharacterGrounded)
        {
            GroundMovement();
            MovePlayer();
        }
    }
    private void GroundMovement()
    {
        AdjustMovementToTerrain();
        CheckJump();
        CheckRunning();
        GroundMoveAnimations();
    }
    private void SetUpDesiredMove(Vector3 desiredMove, int i)
    {
        desiredMove = (transform.forward * _MovementInput.y + transform.right * _MovementInput.x) / i;
        TerrainBasedMovementToStepCycle(desiredMove, i);
    }
    private void AdjustMovementToTerrain()
    {
        int i;
        Vector3 desiredMove = new();
        if (_CharacterAngle < 60)
        {
            if (SCANT._GetCurrentLayer != null && SCANT._GetCurrentLayer.StartsWith("0"))
            {
                i = 0;
                SetUpDesiredMove(desiredMove, i);
            }
            else if (SCANT._GetCurrentLayer != null && SCANT._GetCurrentLayer.StartsWith("1"))
            {
                i = 1;
                SetUpDesiredMove(desiredMove, i);
            }
            else if (SCANT._GetCurrentLayer != null && SCANT._GetCurrentLayer.StartsWith("2"))
            {
                i = 2;
                SetUpDesiredMove(desiredMove, i);
            }
            else
            {
                i = 3;
                SetUpDesiredMove(desiredMove, i);
            }
        }
        else
        {
            TerrainBasedMovementToStepCycle(Vector3.down, 4);
        }
    }
    private void TerrainBasedMovementToStepCycle(Vector3 desiredMove, int i)
    {
        GetInput(out float speed);

        if (_CharacterAngle > 60 && speed > 0)
        {
            _CharacterWalkSpeed = i / 6;
        }
        if (_CharacterAngle > 30 && speed > 0)
        {
            _CharacterWalkSpeed = i / 3;
        }

        _ = new RaycastHit();
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, _CharacterController.radius, Vector3.down, out hitInfo,
                           _CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
        _CharacterMoveDirection.x = desiredMove.x * speed;
        _CharacterMoveDirection.z = desiredMove.z * speed;

        ProgressStepCycle(speed);
    }
    private void ProgressStepCycle(float speed)
    {
        float temp = new float();
        if (_CharacterController.velocity.sqrMagnitude > 0 && (_MovementInput.x != 0 || _MovementInput.y != 0))
        {
            if (_CharacterWalking)
            {
                _CharacterStepCycle += ((speed * _CharacterWalkStep) * Time.fixedDeltaTime);
            }
            if (_CharacterRunning)
            {
                _CharacterStepCycle += ((speed + _CharacterRuningStep) * Time.fixedDeltaTime);
            }
        }

        if (!(_CharacterStepCycle > _CharacterNextStep))
        {
            return;
        }
        if (_CharacterWalking)
        {
            temp = _CharacterWalkStep;
        }
        if (_CharacterRunning)
        {
            temp = _CharacterRuningStep;
        }
        PlayFootStepAudio(temp);
        _CharacterNextStep += temp;

    }
    public void SwapFootsteps(FootstepsCollectionScript collection)
    {
        int n = 3;
        for (int i = 0; i < n; i++)
        {
            if (_CharacterFoostepsSounds.Count < n)
            {
                _CharacterFoostepsSounds.Add(collection.foostepSounds[i]);
            }
            else
            {
                _CharacterFoostepsSounds[i] = collection.foostepSounds[i];
            }
        }
        _CharacterJumpSound = collection.jumpSound;
        _CharacterLandSound = collection.landSound;
    }
    private void PlayFootStepAudio(float _StepCycle)
    {
        if (!_CharacterController.isGrounded)
        {
            return;
        }
        else
        {
            if (_CharacterFoostepsSounds.Count > 0)
            {
                int n = Random.Range(1, _CharacterFoostepsSounds.Count);
                _CharacterAudioSource.clip = _CharacterFoostepsSounds[n];
                for (int i = 0; i < 6; i++)
                {
                    if (_StepCycle != 0 && i == 3)
                    {
                        _CharacterAudioSource.PlayOneShot(_CharacterAudioSource.clip);
                    }
                }
                _CharacterFoostepsSounds[n] = _CharacterFoostepsSounds[0];
                _CharacterFoostepsSounds[0] = _CharacterAudioSource.clip;
            }
        }
    }
    public bool CheckRunning()
    {
        Vector2 input = GetInputFromAxis();

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            _CharacterCurrentMoveSpeed = _CharacterRunSpeed;
            _CharacterCurrentStamina = _CharacterAttributesScript._GetCharacterCurrentStamina;
            _CharacterCurrentStamina -= 0.2f;
            _CharacterAttributesScript._SetCharacterCurrentStamina(_CharacterCurrentStamina);
            _CharacterRunning = true;
            _CharacterWalking = false;
            _CharacterAnimator.SetBool("Running", true);
            return true;
        }
        else
        {
            _CharacterRunning = false;
            _CharacterAnimator.SetBool("Running", false);
            return false;
        }
    }
    public void GroundMoveAnimations()
    {
        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D)
            )
        {
            _CharacterAnimator.SetBool("Walking", true);
            _CharacterWalking = true;
            _CharacterRunning = false;
        }
        else
        {
            _CharacterAnimator.SetBool("Walking", false);
            _CharacterWalking = false;
        }
    }
    private void CheckWaterMovement()
    {
        if (CDS._GetCharacterSwimming() || CDS._GetCharacterUnderwater())
        {
            AdjustMovementAccordingToWater();
            MovePlayer();

            if (CDS._GetCharacterSwimming())
            {
                SwimmingVerticalMovement();
            }

            if (CDS._GetCharacterUnderwater())
            {
                UnderWaterVerticalMovement();
            }
        }
    }
    private void AdjustMovementAccordingToWater()
    {
        Vector3 desiredMove = transform.forward * _MovementInput.y + transform.right * _MovementInput.x;
        GetInput(out float speed);
        _CharacterMoveDirection.x = desiredMove.x * speed;
        _CharacterMoveDirection.z = desiredMove.z * speed;
        ProgressStepCycle(speed);
    }
    private void SwimmingVerticalMovement()
    {
        if (Input.GetKey(KeyCode.C))
        {
            velocity = -0.8f;
            MovePlayer();
        }
    }
    private void UnderWaterVerticalMovement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            velocity = 0.7f;
            MovePlayer();
        }
        if (Input.GetKey(KeyCode.C))
        {
            velocity = -0.8f;
            MovePlayer();
        }
    }
    private void CheckAirMovement()
    {
        if (_CharacterController.isGrounded == false && CDS._GetCharacterSwimming() == false && CDS._GetCharacterUnderwater() == false)
        {
            AirMovement();
        }
    }
    private void AirMovement()
    {
        ApplyGravityOnCharacterInAir();
        MovePlayer();
    }
    private void ApplyGravityOnCharacterInAir()
    {
        if (gravityScale < 0)
        {
            gravityScale = 0;
        }
        else
        {
            gravityScale -= 2f;
            _GravityForce = 9.81f;
            velocity += _GravityForce * gravityScale * Time.deltaTime;
        }
    }
    #endregion


    /*public void CheckJumpingOnSolidGround()
    {
        if (_CharacterJumped == false && Input.GetKeyDown(KeyCode.Space) && !_CharacterCrouching && !_CharacterUnderwater && !_CharacterSwimming)
        {
            _CharacterCurrentStamina = _CharacterAttributesScript._GetCharacterCurrentStamina;
            if (_CharacterCurrentStamina > 0)
            {
                
                _CharacterJumped = CrossPlatformInputManager.GetButtonDown("Jump");
                PlayJumpSound();
                //_CharacterAttributesScript._SetCharacterCurrentStamina(_CharacterCurrentStamina);
                //_CharacterInAir = true;
                Debug.Log("TEST");
            }
            else
            {
                Debug.Log("Too tired to jump!");
            }        
        }
    }*/


    #region REST
    public void CheckAttack()
    {
        if (/*(Input.GetButtonDown("Attack") ||*/ Input.GetKeyDown(KeyCode.Mouse0))/*Input.GetKeyDown(KeyCode.Mouse0)*/
        {
            //_CharacterAnimator.SetBool("Attack", true);
        }
        else
        {
            // _CharacterAnimator.SetBool("Attack", false);
        }

    }
    void SpawnChargingProjectile()
    {
        _ChargingSpell = Instantiate(_ChargeToUse, _ChargeSocket.transform.position /*_ProjectileSocket.transform.position, Quaternion.identity*/, _CharacterCamera.transform.rotation) as GameObject;
        //newProjectile.GetComponent<ProjectileScript>().damageCaused = _ProjectileDamage;
        Vector3 unitVectorToPlayer = (_CharacterCamera.transform.position + _ChargeSocket.transform.position).normalized;
        projectileScript = _ChargingSpell.GetComponent<ProjectileScript>();
        _ProjectileSpeed = projectileScript.speed;
        //_ChargingSpell.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        Debug.Log("SPAWNED CHARGED SPELL");
        Destroy(_ChargingSpell, 2);
    }
    void SpawnCharge()
    {
        if(_ChargingSpell == null)
        {
            _ChargingSpell = Instantiate(_ChargeToUse, _ChargeSocket.transform.position /*_ProjectileSocket.transform.position, Quaternion.identity*/, _CharacterCamera.transform.rotation) as GameObject;
        }
        
        //newProjectile.GetComponent<ProjectileScript>().damageCaused = _ProjectileDamage;
        Vector3 unitVectorToPlayer = (_CharacterCamera.transform.position + _ChargeSocket.transform.position).normalized;
        projectileScript = _ChargingSpell.GetComponent<ProjectileScript>();
        _ProjectileSpeed = projectileScript.speed;
        _ProjectileSpeed = 0.12f;
        _ChargingSpell.GetComponent<Rigidbody>().velocity = /*unitVectorToPlayer  * _ProjectileSpeed*/_ProjectileSpawnPoint.transform.up * _ProjectileSpeed;
        //Debug.Log("SPAWNED CHARGE");
        Destroy(_ChargingSpell, 3);
    }
    void SpawnProjectile()
    {
        if(_CastedSpell == null)
        {
            _CastedSpell = Instantiate(_ProjectileToUse, _ProjectileSpawnPoint.transform.position /*_ProjectileSocket.transform.position, Quaternion.identity*/, _CharacterCamera.transform.rotation) as GameObject;
        }
        
        //newProjectile.GetComponent<ProjectileScript>().damageCaused = _ProjectileDamage;
        Vector3 unitVectorToPlayer = (_CharacterCamera.transform.position + _ProjectileSocket.transform.position).normalized;
        projectileScript = _CastedSpell.GetComponent<ProjectileScript>();
        _ProjectileSpeed = projectileScript.speed;
        _ProjectileSpeed = 1 * projectileScript.speed;
        _CastedSpell.GetComponent<Rigidbody>().velocity = /*unitVectorToPlayer  * _ProjectileSpeed*/_CastedSpell.transform.forward * _ProjectileSpeed;
        Debug.Log("SPAWNED SPELL");
        Destroy(_CastedSpell, 4);
    }
    public void CheckSpellCast()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            SpawnCharge();
            StartCoroutine(DelayInSeconds(1));
        }
        else
        {
            _CharacterAnimator.SetBool("SpellCharge", false);
            _CharacterAnimator.SetBool("SpellCharged", false);
            _CharacterAnimator.SetBool("SpellCasted", false);
            StopAllCoroutines();
            return;
        }
    }
    private IEnumerator DelayInSeconds(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        if (Input.GetKey(KeyCode.Mouse1))
        {
            StartCoroutine(DelayInSecondsAnimator(2));
            StartCoroutine(DelayInSecondsSpellCast(3));

        }
    }
    private IEnumerator DelayInSecondsAnimator(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        _CharacterAnimator.SetBool("SpellCasted", true);
    }
    private IEnumerator DelayInSecondsSpellCast(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        SpawnProjectile();
    }
    public void CheckInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //InteractWithObject();
        }
    }
    private void IteractingStuff()
    {
        /*
            if (_InteractableObject != null && _InteractedObject)
            {
                if (_InteractedObject.tag == "Destructible")
                {
                    //interactedObject.transform.position = ObjectCarrier.position;
                    _InteractedObject.transform.position = new Vector3(_ObjectCarryModule.transform.position.x, _ObjectCarryModule.transform.position.y, _ObjectCarryModule.transform.position.z);
                    _InteractedObject.transform.rotation = _ObjectCarryModule.transform.rotation;
                    _InteractedObject.transform.Rotate(new Vector3(_ObjectCarryModule.transform.localRotation.x, _ObjectCarryModule.transform.localRotation.y, _ObjectCarryModule.transform.localRotation.z));

                }
                else if (_InteractedObject.tag == "Melee Weapon")
                {
                    _InteractedObject.transform.position = _ObjectCarryModule.transform.position;
                    _InteractedObject.transform.position = new Vector3(_ObjectCarryModule.transform.position.x, _ObjectCarryModule.transform.position.y - 0.1f, _ObjectCarryModule.transform.position.z + 0.4f);
                    _InteractedObject.transform.rotation = _ObjectCarryModule.transform.rotation;
                    //interactedObject.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y - 0.1f, m_Camera.transform.position.z + 0.4f);

                    _InteractedObject.transform.Rotate(new Vector3(_ObjectCarryModule.transform.localRotation.x, _ObjectCarryModule.transform.localRotation.y, _ObjectCarryModule.transform.localRotation.z));

                }

                //interactedObject.transform.Rotate(new Vector3(ObjectCarrier.localRotation.x, 50, ObjectCarrier.localRotation.z + 30));

            }
            else
            {
                _InteractedObject = null;
            }*/
    }
    public virtual void Interact()
    {
        RaycastHit _CharacterInteractionRayHit;
        if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(transform.position, _CharacterCamera.transform.forward, out _CharacterInteractionRayHit, 1))
        {
            Item _Item = _CharacterInteractionRayHit.transform.GetComponent<PickupScript>()._Item;
            //Debug.Log(_CharacterInteractionRayHit.transform.GetComponent<PickupScript>()._Item.name);
        }
    }
    private void SetFocusOnInteractedObject(CharacterInteractionScript newObjectFocus)
    {
        //_InteractableObject = newObjectFocus;
    }
    private GameObject ObjectCarryModuleComponent
    {
        get
        {
            return transform.GetChild(0).gameObject;
        }
    }
    private void RemoveFocusFromInteractedObject()
    {
        //_InteractableObject = null;
    }
    public bool _GetCharacterRunning()
    {
        return _CharacterRunning;
    }
    public bool _GetCharacterJumping()
    {
        return _CharacterInAir;
    }
    public bool _GetCharacterAttacking()
    {
        return _CharacterAttacking;
    }
    public bool _GetCharacterChanneling()
    {
        return _CharacterChanneling;
    }
    public bool _GetCharacterCasting()
    {
        return _CharacterCasting;
    }
    #endregion
}

