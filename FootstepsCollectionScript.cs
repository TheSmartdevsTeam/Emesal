using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Foostep Collection", menuName = "Create New Foostep Collection")]
public class FootstepsCollectionScript : ScriptableObject
{
    public List<AudioClip> foostepSounds = new List<AudioClip>();
    public AudioClip jumpSound;
    public AudioClip landSound;
}
