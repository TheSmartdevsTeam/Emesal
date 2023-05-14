using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class RotateToMouseScript : MonoBehaviour
{
    PlayerControllerScript pcs;
    public Camera _Camera;
    public float _MaxLength;

    private Ray _RayMouse;
    private Vector3 _Position;
    private Vector3 _Direction;
    private Quaternion _Rotation;

    private MouseLook _MouseLook;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(_Camera != null)
        {
            
            RaycastHit hit;

            var MousePos = Input.mousePosition;
            _RayMouse = _Camera.ScreenPointToRay(MousePos);
            if(Physics.Raycast(_RayMouse.origin, _RayMouse.direction, out hit, _MaxLength))
            {
                RotateToMouseDirection(gameObject, hit.point);

            }
            else
            {
                var pos = _RayMouse.GetPoint(_MaxLength);
                RotateToMouseDirection(gameObject, pos);
                
            }
        }
        else
        {
            Debug.Log("No Camera!!!");
        }
    }


    void RotateToMouseDirection(GameObject obj, Vector3 destination)
    {
        _Direction = destination - obj.transform.position;
        _Rotation = Quaternion.LookRotation(_Direction);
        



        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, _Rotation,1);
        _MouseLook.LookRotation(transform, _Camera.transform);

    }

    public Quaternion GetRotation()
    {
        return _Rotation;
    }
}
