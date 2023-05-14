using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectDestroyedScript : MonoBehaviour
{
    public GameObject _CurrentObject;
    public GameObject _CurrentObjectChunks;
    public float _ExplosionRadius = 5;
    public float _ExplosionForce = 1;
    public float damage = 1;
    public float timeToExplode = 0;
    public GameObject _DestrucionParticle;
    PlayerStatusController _CurrentObjectStatus;

    public void Start()
    {
        _CurrentObjectStatus = new PlayerStatusController();
        _CurrentObjectStatus._MaxHealthPoints = 1;
        _CurrentObjectStatus._CurrentHealthPoints = _CurrentObjectStatus._MaxHealthPoints;
    }

    public void Update()
    {
        if(_CurrentObjectStatus._CurrentHealthPoints <= 0)
        {
            Detonate();
        }
    }

    public void Detonate(float holdTime)
    {
        StartCoroutine(WaitToExplode(holdTime));
    }

    public void Detonate()
    {
        StartCoroutine(WaitToExplode());
    }

    /// <summary>
    /// Instantly invokes the method responsible for instantiating the explosion particle and calculate the damage made by grenade.
    /// </summary>
    private IEnumerator WaitToExplode()
    {
        yield return new WaitForSeconds(timeToExplode);
        Explode();
    }

    /// <summary>
    /// Invokes the method responsible for instantiating the explosion particle and calculate the damage made by grenade.
    /// Parameters: How much time the player holds the grenade.
    /// </summary>
    private IEnumerator WaitToExplode(float holdTime)
    {
        yield return new WaitForSeconds(timeToExplode - holdTime);
        Explode();
    }

    /// <summary>
    /// Instantiate the explosion particle and calculates the damage caused by grenade.
    /// </summary>
    public void Explode()
    {
        GameObject explosion = Instantiate(_DestrucionParticle, transform.position, Quaternion.identity) as GameObject; // Instantiate the explosion particle.

        // Calculates damage dealt.
        Explosion.NewExplosion(_ExplosionRadius, _ExplosionForce, damage, new Vector3(transform.position.x, transform.position.y, transform.position.z));
        Destroy(explosion, 1); // Destroys the particle after 3 seconds.
        Destroy(gameObject); // Destroy the grenade.
    }

    // Draw a sphere to show grenade explosion radius
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _ExplosionRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _CurrentObjectStatus._CurrentHealthPoints -= 1;
    }
}

