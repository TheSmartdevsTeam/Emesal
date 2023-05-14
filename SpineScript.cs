using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineScript : MonoBehaviour
{
    public GameObject _CharacterCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.rotation = _CharacterCamera.transform.rotation;
    }
}
