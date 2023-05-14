using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class EnemyAbilityScript : MonoBehaviour, IDamageable
    {
        AudioSource _AudioSource;
        public float _AudioVolume;
        public AudioClip _DetectionSound;
        public float _DetectionRadius = 3;
        public bool _DetectionSoundOn;

        public Animator _Animator;

        public UnityEngine.AI.NavMeshAgent _NavMeshAgent;

        public bool Delayed;

        public GameObject _Player;
        public Transform _TargetLocation;

        float _MaxHealth = 100f;
        float _CurrentHealth = 100f;

        [SerializeField] GameObject _ProjectileToUse;
        [SerializeField] GameObject _ProjectileSocket;
        ProjectileScript projectileScript;
        public float _ProjectileSpeed = 0.1f;
        float _ProjectileDamage = 5;
        float _ProjectileCooldown = 0.5f;

        float _Distance;
        public float _MeleeAttackRange = 4;
        public float _RangedAttackRange;
        private bool _isAttacking;
        [SerializeField] Vector3 _AimOffset = new Vector3(0, 0.5f, 0);

        // Start is called before the first frame update
        void Start()
        {
            _Player = GameObject.FindGameObjectWithTag("Player");
            _TargetLocation = _Player.transform;
            _isAttacking = false;
        }

        // Update is called once per frame
        void Update()
        {
            _Distance = Vector3.Distance(transform.position, _Player.transform.position);
            _ = StartCoroutine(DetectTargets());

            if (_Distance <= _MeleeAttackRange && !_isAttacking)
            {
                _isAttacking = true;
                /*
                
                InvokeRepeating("SpawnProjectile", 0f, _ProjectileCooldown);  //TODO switch to coroutines
                SpawnProjectile();*/
            }

            if (_Distance > _MeleeAttackRange)
            {
                _isAttacking = false;
                CancelInvoke();
            }


        }

        public void SetProjectileDamage(float damage)
        {
            _ProjectileDamage = damage;
        }

        public float GetProjectileDamage()
        {
            return _ProjectileDamage;
        }

        void SpawnProjectile()
        {
            GameObject newProjectile = Instantiate(_ProjectileToUse, _ProjectileSocket.transform.position, Quaternion.identity);
            newProjectile.GetComponent<ProjectileScript>().damageCaused = _ProjectileDamage;
            Vector3 unitVectorToPlayer = (_Player.transform.position + _AimOffset - _ProjectileSocket.transform.position).normalized;
            projectileScript = newProjectile.GetComponent<ProjectileScript>();
            _ProjectileSpeed = projectileScript.speed;
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * _ProjectileSpeed;

        }

        public void TakeDamage(float damage)
        {
            _CurrentHealth = Mathf.Clamp(_CurrentHealth - damage, 0, _MaxHealth);
            print("ENEMY DAMAGED: " + _CurrentHealth);
        }


        public IEnumerator DetectTargets()
        {
            if (_Distance <= _DetectionRadius && _Distance > (_MeleeAttackRange + 1))
            {
                Vector3 NPC_Direction = _TargetLocation.transform.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(NPC_Direction), 1f);
                _Animator.SetBool("MeleeAttack", false);
                _Animator.SetBool("Moving", true);
                _ = StartCoroutine(RuntToTarget());
            }
            else if (_Distance <= _MeleeAttackRange)
            {
                _Animator.SetBool("MeleeAttack", true);
                _Animator.SetBool("Moving", false);
                _TargetLocation = transform;
                //_ = StartCoroutine(RotateTowardsTarget());

                //print("Attacking: " + _Player.name);
            }

            else if (_Distance > 10)
            {
                _Animator.SetBool("Moving", false);
                _Animator.SetBool("MeleeAttack", false);
                _TargetLocation = transform;
            }

            if (name == "Melee Enemy1" || name == "TransformedMaleCharacter")
            {
                
                if (_Distance <= _MeleeAttackRange)
                {
                    _Animator.SetBool("MeleeAttack", true);
                    _Animator.SetBool("Moving", false);
                    //Debug.Log("Melee Enemy Attacks");
                    _NavMeshAgent.speed = 0f;
                    _NavMeshAgent.acceleration = 0f;
                }
                else
                {
                    _ = StartCoroutine(RuntToTarget());
                    //_Animator.SetBool("AttackingState", false);
                    //Debug.Log("No Attacks");
                    _Animator.SetBool("MeleeAttack", false);
                    _Animator.SetBool("Moving", true);
                }
                
            }
            
            if(name == "Range Enemy1")
            {
                if (_Distance <= _RangedAttackRange)
                {
                    _Animator.SetBool("RangedAttack", true);
                    _Animator.SetBool("Moving", false);

                    //Debug.Log("Range Enemy Attacks");
                    _NavMeshAgent.speed = 0f;
                    _NavMeshAgent.acceleration = 0f;
                }
                else
                {
                    _ = StartCoroutine(RuntToTarget());
                    _Animator.SetBool("RangedAttack", false);
                }
            }
            

            if (Delayed == false)
            {
                yield return new WaitForSeconds(0.5f);
                Delayed = true;
            }
            else
            {
                Delayed = false;
            }
        }

        public IEnumerator RuntToTarget()
        {
            Vector3 NPC_Direction = _TargetLocation.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(NPC_Direction), 0.5f);
            yield return new WaitForSeconds(1);
            _NavMeshAgent.SetDestination(_Player.transform.position);
            _NavMeshAgent.speed = 2f;
            _NavMeshAgent.acceleration = 1f;
        }

        public IEnumerator RotateTowardsTarget()
        {
            Vector3 NPC_Direction = _TargetLocation.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(NPC_Direction), 0.1f);
            _NavMeshAgent.SetDestination(_Player.transform.position);
            yield return new WaitForSeconds(0.1f);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _DetectionRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _MeleeAttackRange);
        }
    }
}

