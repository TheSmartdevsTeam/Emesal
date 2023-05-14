using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSoundScript : MonoBehaviour
{
    public float minImpactForce = 1; // Minimum linear velocity to detect a collision between two objects.
    public AudioClip collisionSound; // Sound played when two objects collide.
    public float volume = 1f; // Sound volume.

    /// <summary>
    /// Method responsible for checking the collision of this object with any other.
    /// Parameters: The information about the collision.
    /// </summary>
    private void OnCollisionEnter(Collision col)
    {
        if (col.relativeVelocity.magnitude > minImpactForce) // If the impact velocity is greater than the minimum speed.
        {
            GetComponent<AudioSource>().clip = collisionSound; // Set AudioSource.clip as collision sound.
            GetComponent<AudioSource>().volume = volume; // Set AudioSource.volume.
            GetComponent<AudioSource>().Play();
        }
    }
}
