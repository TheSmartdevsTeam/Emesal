using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Water;

public class WaterReflectionCameraScript : MonoBehaviour
{
    Camera WaterCamera;
    Water Lake;

    // Start is called before the first frame update


    private void Awake()
    {
        WaterCamera = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
