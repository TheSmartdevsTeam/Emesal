using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoveScript : MonoBehaviour
{
    public float _Speed;
    public float _FireRate;
    // Start is called before the first frame update

    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if(_Speed != 0)
        {
            transform.position += transform.forward * (_Speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("No Speed!!!");
        }

    }

    void OnCollisionEnter(Collision col)
    {
       _Speed = 0;
        Destroy(gameObject);


    }
}
