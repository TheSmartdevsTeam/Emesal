using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;

public enum MoveState
{
    Idle,
    Crouched,
    Walking,
    Running,
    Flying
}
#pragma warning disable 618, 649
namespace UnityStandardAssets.Characters.FirstPerson
{




    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class PlayerControllerScript : MonoBehaviour
    {

        //MOVEMENT
        [SerializeField] private bool _IsWalking;
        [SerializeField] public float _WalkSpeed;
        [SerializeField] private float _RunSpeed;
        [Range(1, 3)]
        public float _RunMultiplier = 2; // Speed when sprinting = walkingSpeed * runMultiplier
        [SerializeField] [Range(0f, 1f)] private float _RunstepLenghten;
        [SerializeField] public float _JumpSpeed = 8;
        public float _CrouchSpeed = 2;
        [SerializeField] private MouseLook _MouseLook;
        [SerializeField] private bool _UseHeadBob;
        [SerializeField] private bool _UseFovKick;
        [SerializeField] private FOVKick _FovKick = new FOVKick();
        [SerializeField] private CurveControlledBob _HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob _JumpBob = new LerpControlledBob();
        [SerializeField] private float _StepInterval;

        private bool _Jump;
        private bool _Inventory;
        private Vector2 _Input;
        private Vector3 _MoveDir = Vector3.zero;
        private CharacterController _CharacterController;
        private CollisionFlags _CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 _OriginalCameraPosition;
        private float _StepCycle;
        private float _NextStep;
        private bool _Jumping;


        public CharacterInteractionScript focus;
        public GameObject ObjectCarrier;
        public GameObject interactedObject;
        public Vector3 Offset;
        CharacterInteractionScript interactable;


        [HideInInspector]
        public bool isClimbing;

        [HideInInspector]
        public bool canVault;

        public MoveState moveState = MoveState.Idle;


        [Space()]
        //public float jumpForce = 10;
        public bool airControll = false;

        [Tooltip("Limits the collider to only climb slopes that are less steep (in degrees) than the indicated value.")]
        public float _SlopeLimit = 60;
        public float _GravityForce = 9;

        private float currentSpeed;


        private bool jump;
        private bool jumping;
        private bool crouched, running, aiming;


        //AUDIO
        [SerializeField] private AudioClip[] m_FootstepSounds;
        [SerializeField] private AudioClip m_JumpSound;
        [SerializeField] private AudioClip m_LandSound;

        private AudioSource m_AudioSource;
        public AudioManagerScript _AudioManager;
        public AudioClip breathClip;

        public CastSpellScript css;


        //GRAVITY
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;


        //CAMERA
        public Camera m_Camera;
        public CameraAnimationsScript cameraAnim;


        //PLAYER STATS
        [Space()]
        public bool stamina = true;

        [Tooltip("Decrement and increment the value of stamina per second")]
        public float _StaminaDrainRate = 5.0f;

        public Texture2D InventoryImage;


        PlayerStatusController psc;
        Animator anim;

        public LayerMask movementMask;
        public float _UIScale = 1.0f;
        private void Start()
        {

            //anim = gameObject.GetComponent<Animator>();

            _CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            _OriginalCameraPosition = m_Camera.transform.localPosition;
            _FovKick.Setup(m_Camera);
            _HeadBob.Setup(m_Camera, _StepInterval);
            _StepCycle = 0f;
            _NextStep = _StepCycle / 2f;
            _Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            _MouseLook.Init(transform, m_Camera.transform);


            psc = GetComponent<PlayerStatusController>();


            psc._Strength = 10;
            psc._Constitution = 10;
            psc._Agility = 10;
            psc._Dexterity = 10;
            psc._Inteligence = 10;
            psc._Weigth = 10 * psc._Strength - psc._Constitution;

            psc._MaxHealthPoints = 100 + psc._Strength + (psc._Constitution / 5);
            psc._CurrentHealthPoints = psc._MaxHealthPoints;
            psc._HPRegenerationSpeed = psc._Constitution / psc._Strength;


            psc._MaxStaminaPoints = 100 + psc._Constitution;
            psc._CurrentStaminaPoints = psc._MaxStaminaPoints;
            _StaminaDrainRate = psc._Constitution * psc._Agility / psc._Weigth;

            _WalkSpeed = 2 + psc._Agility / psc._Weigth;
            _RunSpeed = 6 + psc._Agility / psc._Weigth;
            _RunstepLenghten = 0.7f;
            _JumpSpeed = (5 * psc._Agility * psc._Strength) / psc._Weigth;
            _StepInterval = _WalkSpeed + _RunstepLenghten;
            _SlopeLimit = 60 + 10 * psc._Agility / psc._Weigth;
            psc._HeightThreshold = 5 - (psc._Weigth / 100);
            psc._FallDamageMultiplier = psc._Weigth / 10;

            //anim = GameObject.FindGameObjectWithTag("Backpack").GetComponent<Animator>();

            do
            {
                ObjectCarrier = GameObject.FindGameObjectWithTag("MainCamera");
                //Offset = interactable.transform.position - ObjectCarrier.transform.position;

            } while (focus != null && interactedObject != null);


            css = GetComponentInChildren<CastSpellScript>();

        }




        // Update is called once per frame
        private void Update()
        {

            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!_Jump)
            {
                _Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!_Inventory)
            {
                _Inventory = CrossPlatformInputManager.GetButtonDown("Inventory");
            }

            if (!m_PreviouslyGrounded && _CharacterController.isGrounded)
            {
                StartCoroutine(_JumpBob.DoBobCycle());
                PlayLandingSound();
                _MoveDir.y = 0f;
                _Jumping = false;
            }
            if (!_CharacterController.isGrounded && !_Jumping && m_PreviouslyGrounded)
            {
                _MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = _CharacterController.isGrounded; 


            currentSpeed = UpdateCurrentSpeed();
            CheckCrouchedState();
            SetPlayerState();

            if (stamina)
            {
                UpdateStaminaAmount();
            }

            int state1 = anim.GetCurrentAnimatorStateInfo(0).tagHash;
            int state2 = anim.GetNextAnimatorStateInfo(0).tagHash;
            if (Input.GetKeyDown(KeyCode.R))
            {

                if (state1 != state2)
                {
                    anim.SetBool("IfKeyIsPressed", true);
                }



            }
            if (Input.GetKeyUp(KeyCode.R) && state1 != state2)
            {
                anim.SetBool("IfKeyIsPressed", false);
                state1 = 0;
            }



            TryToInteractWithObject();
            css.GetUserInput();



        }


        void TryToInteractWithObject()
        {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetKeyDown(KeyCode.F) && interactedObject == null)
            {


                if (Physics.Raycast(ray, out hit, 100))
                {
                    interactable = hit.collider.GetComponent<CharacterInteractionScript>();
                    if (interactable != null)
                    {
                        SetFocus(interactable);
                        Debug.Log("TRIGGER!");
                        interactedObject = hit.transform.gameObject;
                        Debug.Log(interactedObject.ToString());
                        interactedObject.transform.position = ObjectCarrier.transform.position;

                    }
                }
            }


            if (Input.GetKeyDown(KeyCode.X))
            {


                if (Physics.Raycast(ray, out hit, 100) || focus != null)
                {
                    //interactable = hit.collider.GetComponent<Interactable>();
                    interactable = focus;
                    if (interactable != null)
                    {
                        RemoveFocus();
                        interactedObject = null;
                        Debug.Log("END TRIGGER!");
                    }
                }
            }
        }

        private void LateUpdate()
        {
            if (focus != null && interactedObject)
            {

                if (interactedObject.tag == "Destructible")
                {
                    //interactedObject.transform.position = ObjectCarrier.position;
                    interactedObject.transform.position = new Vector3(ObjectCarrier.transform.position.x, ObjectCarrier.transform.position.y, ObjectCarrier.transform.position.z);
                    interactedObject.transform.rotation = ObjectCarrier.transform.rotation;
                    interactedObject.transform.Rotate(new Vector3(ObjectCarrier.transform.localRotation.x, ObjectCarrier.transform.localRotation.y, ObjectCarrier.transform.localRotation.z));

                }
                else if (interactedObject.tag == "Melee Weapon")
                {
                    interactedObject.transform.position = ObjectCarrier.transform.position;
                    interactedObject.transform.position = new Vector3(ObjectCarrier.transform.position.x, ObjectCarrier.transform.position.y - 0.1f, ObjectCarrier.transform.position.z + 0.4f);
                    interactedObject.transform.rotation = ObjectCarrier.transform.rotation;
                    //interactedObject.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y - 0.1f, m_Camera.transform.position.z + 0.4f);

                    interactedObject.transform.Rotate(new Vector3(ObjectCarrier.transform.localRotation.x, ObjectCarrier.transform.localRotation.y, ObjectCarrier.transform.localRotation.z));

                }

                //interactedObject.transform.Rotate(new Vector3(ObjectCarrier.localRotation.x, 50, ObjectCarrier.localRotation.z + 30));

            }
            else
            {
                interactedObject = null;
            }


        }

        void SetFocus(CharacterInteractionScript newFocus)
        {
            focus = newFocus;
        }

        void RemoveFocus()
        {
            focus = null;

        }



        private void OnGUI()
        {
            float offset = Mathf.Clamp((Screen.height / 7.2f) / (2 - _UIScale), 50, 150);
            if (_Inventory)
            {
                Debug.Log("I");
                GUI.BeginGroup(new Rect(Screen.width * 1f, Screen.height - 1f, psc._CurrentHealthPoints * (psc._MaxHealthPoints) * 1f, 1f));
                GUI.DrawTexture(new Rect(Screen.width - offset * 1.6f, Screen.height - offset * 1.25f, offset * 0.18f, offset * 0.18f), InventoryImage);

                GUI.EndGroup();

            }
            _Inventory = false;
        }
        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * _Input.y + transform.right * _Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, _CharacterController.radius, Vector3.down, out hitInfo,
                               _CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            _MoveDir.x = desiredMove.x * speed;
            _MoveDir.z = desiredMove.z * speed;



            if (_CharacterController.isGrounded)
            {
                _MoveDir.y = -m_StickToGroundForce;

                if (_Jump)
                {
                    _MoveDir.y = _JumpSpeed;
                    PlayJumpSound();
                    _Jump = false;
                    _Jumping = true;
                }
            }
            else
            {
                _MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            _CollisionFlags = _CharacterController.Move(_MoveDir * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            _MouseLook.UpdateCursorLock();
        }


        //MOVEMENT

        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }
        private void ProgressStepCycle(float speed)
        {
            if (_CharacterController.velocity.sqrMagnitude > 0 && (_Input.x != 0 || _Input.y != 0))
            {
                _StepCycle += (_CharacterController.velocity.magnitude + (speed * (_IsWalking ? 1f : _RunstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(_StepCycle > _NextStep))
            {
                return;
            }

            _NextStep = _StepCycle + _StepInterval;

            PlayFootStepAudio();
        }

        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool _Waswalking = _IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            _IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = _IsWalking ? _WalkSpeed : _RunSpeed;
            _Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (_Input.sqrMagnitude > 1)
            {
                _Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (_IsWalking != _Waswalking && _UseFovKick && _CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!_IsWalking ? _FovKick.FOVKickUp() : _FovKick.FOVKickDown());
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


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
            if (hit.gameObject == GameObject.FindGameObjectWithTag("Enemy"))
            {
                gameObject.SetActive(false);
            }
        }





        private void SetPlayerState()
        {
            //PlayerStatusController _PSC = GetComponent<PlayerStatusController>();
            //_PSC._CurrentMoveSpeed = GetComponent<Rigidbody>().velocity.magnitude;

            float currentSpeed = RigidBody.velocity.magnitude;
            float minMoveSpeed = RealWalkingSpeed() * 0.85f;
            bool idle = GetInputFromAxis() == Vector2.zero;

            if (m_PreviouslyGrounded && !jumping)
            {
                if (currentSpeed < minMoveSpeed * _RunMultiplier && currentSpeed > _CrouchSpeed && !CheckRunning() && !idle)
                {
                    // Walking
                    moveState = MoveState.Walking;
                }
                else if (currentSpeed > minMoveSpeed * RunMultiplierWithStamina() && !idle)
                {
                    // Running
                    moveState = MoveState.Running;

                }
                else if (currentSpeed < minMoveSpeed && currentSpeed > _CrouchSpeed * 0.85f && crouched && !idle)
                {
                    // Crouched
                    moveState = MoveState.Crouched;
                }
                else if (currentSpeed < _CrouchSpeed * 0.5f)
                {
                    // Idle
                    moveState = MoveState.Idle;
                }
            }
            else
            {
                if (_StatsController.CheckDistanceBelow() > _JumpSpeed / 10f)
                {
                    moveState = MoveState.Flying;
                }
            }
        }
        public float RealWalkingSpeed()
        {
            switch (psc._Weigth)
            {
                case 0: return _WalkSpeed;
                case 1: return _WalkSpeed * 0.9f;
                case 2: return _WalkSpeed * 0.85f;
                case 3: return _WalkSpeed * 0.8f;
                case 4: return _WalkSpeed * 0.75f;
                case 5: return _WalkSpeed * 0.7f;
                default: return _WalkSpeed;
            }
        }
        public bool isCrouched
        {
            get
            {
                return crouched;
            }
        }
        public bool isJumping
        {
            get
            {
                return jumping;
            }
        }
        private Rigidbody RigidBody
        {
            get
            {
                return GetComponent<Rigidbody>();
            }
        }
        private CapsuleCollider capsuleCol
        {
            get
            {
                return GetComponent<CapsuleCollider>();
            }
        }
        public bool Grounded
        {
            get
            {
                return m_PreviouslyGrounded;
            }
        }

        private void CheckCrouchedState()
        {
            float crouchingSpeed = 8.0f;

            if (crouched)
            {
                capsuleCol.height = 1.2f;
                capsuleCol.center = new Vector3(0, -0.4f, 0);

                m_Camera.transform.localPosition = Vector3.Lerp(m_Camera.transform.localPosition, new Vector3(0, 0.05f, 0), Time.deltaTime * crouchingSpeed);
            }
            else
            {
                capsuleCol.height = 2;
                capsuleCol.center = Vector3.zero;

                m_Camera.transform.localPosition = Vector3.Lerp(m_Camera.transform.localPosition, new Vector3(0, 0.8f, 0), Time.deltaTime * crouchingSpeed);
            }
        }
        public bool CheckRunning()
        {
            Vector2 input = GetInputFromAxis();

            if (Input.GetKey(KeyCode.LeftShift))      //Input.GetButtonDown("Run"))
            {
                running = true;
            }

            if (Input.GetKey(KeyCode.LeftShift) && running && input.y > 0 && !aiming)  //Input.GetButton("Run") && running && input.y > 0 && !aiming) 
            {
                return true;
            }
            else
            {
                running = false;
                return false;
            }
        }
        public float RunMultiplierWithStamina()
        {
            return stamina ? 1 + (psc._CurrentStaminaPoints * (_RunMultiplier - 1)) / 100 : _RunMultiplier;
        }



        //CAMERA

        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!_UseHeadBob)
            {
                return;
            }
            if (_CharacterController.velocity.magnitude > 0 && _CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    _HeadBob.DoHeadBob(_CharacterController.velocity.magnitude +
                                      (speed * (_IsWalking ? 1f : _RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - _JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = _OriginalCameraPosition.y - _JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }
        private void RotateView()
        {
            _MouseLook.LookRotation(transform, m_Camera.transform);
        }



        public bool isAiming
        {
            set
            {
                aiming = value;
            }

            get
            {
                return aiming;
            }
        }

        //AUDIO

        private void PlayFootStepAudio()
        {
            if (!_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }
        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            _NextStep = _StepCycle + .5f;
        }


        //STATS

        private PlayerStatusController _StatsController
        {
            get
            {
                return GetComponent<PlayerStatusController>();
            }
        }
        private void UpdateStaminaAmount()
        {
            psc._CurrentStaminaPoints = Mathf.Clamp(psc._CurrentStaminaPoints, 0, psc._MaxStaminaPoints);

            if (psc._CurrentStaminaPoints <= 50)
                _AudioManager.PlayBreathingSound(breathClip, psc._CurrentStaminaPoints, 50);

            if (CheckRunning())
            {
                psc._CurrentStaminaPoints -= _StaminaDrainRate * Time.deltaTime;
            }
            else
            {
                psc._CurrentStaminaPoints += _StaminaDrainRate * Time.deltaTime;
            }
        }

        private float UpdateCurrentSpeed()
        {
            if (crouched)
            {
                return _CrouchSpeed;
            }
            else
            {
                if (CheckRunning())
                {
                    return RealWalkingSpeed() * (stamina ? RunMultiplierWithStamina() : _RunMultiplier);
                }
                else
                {
                    return aiming ? RealWalkingSpeed() * 0.7f : RealWalkingSpeed();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                //WaterScript water = other.gameObject.GetComponent<WaterScript>();
                //
            }
        }


        private void FireRay()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // If we press left mouse
            if (Input.GetMouseButtonDown(0))
            {
                // We create a ray
                Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // If the ray hits
                if (Physics.Raycast(ray, out hit, 100))
                {

                }
            }
        }





    }


}




