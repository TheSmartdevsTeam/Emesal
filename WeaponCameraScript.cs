using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCameraScript : MonoBehaviour
{
    public Transform mainCamera;
    // Update is called once per frame
    private void Update()
    {
        transform.localPosition = mainCamera.localPosition;
        transform.localRotation = mainCamera.localRotation;
    }
}
