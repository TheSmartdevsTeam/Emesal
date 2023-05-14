using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTransformScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _spellBolt;
    void Start()
    {
        _spellBolt = gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        _spellBolt.transform.Rotate(0, 0, 1.0f, Space.Self);
        if(transform.localPosition.y < 180)
        {
            _spellBolt.GetComponent<Rigidbody>().AddForce(0, 0.005f, 0, ForceMode.Force);
            
        }
        else if (transform.localPosition.y > 180)
        {
            _spellBolt.GetComponent<Rigidbody>().AddForce(0, -0.005f, 0, ForceMode.Force);
            
        }
        
    }
    IEnumerator Wait3Secs()
    {
        
        yield return new WaitForSeconds(3);
    }

}
