using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float damageCaused;
    public float speed;  //other classes can set this
    private void OnTriggerEnter(Collider other)
    {
        
        var damageableComponenet = other.gameObject.GetComponent(typeof(IDamageable));
        //print("damageableComponent " + damageableComponenet);
        if (damageableComponenet)
        {
            (damageableComponenet as IDamageable).TakeDamage(damageCaused);
        }

        
    }
}
