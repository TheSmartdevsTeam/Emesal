using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFloatHelper
{
    public Transform _Transform = null;
    public Rigidbody _Rigidbody = null;
    public bool _SetDrag = false;
    public bool _AddForce = false;
    public float _TransitionTime = 0f;

    public float _OriginalDrag = 0f;
    public float _Original_Angular_Drag = 0f;

    public WaterFloatHelper(Transform _tranform, Rigidbody _rigidbody, float _originalDrag, float _originalAngularDrag)
    {
        _Rigidbody = _rigidbody;
        _Transform = _tranform;
        _SetDrag = false;
        _AddForce = false;
        _TransitionTime = 0f;
        _OriginalDrag = _originalDrag;
        _Original_Angular_Drag = _originalAngularDrag;
    }
}

public class MyWaterScript : MonoBehaviour
{
    public float _Force = 0.2f;

    public float _WaterDrag = 1f;

    private float _Water_height = 0.00000000000f;

    private HashSet<Transform> _Transform_set = new HashSet<Transform>();
    private List<WaterFloatHelper> _Item_list = new List<WaterFloatHelper>();
    private Dictionary<Transform, int> _Transform_to_int = new Dictionary<Transform, int>();

    public void Start()
    {
        _Water_height = transform.position.y;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (!collider.attachedRigidbody)
        {
            return;
        }
        if (!_Transform_set.Contains(collider.transform))
        {
            
            _Transform_set.Add(collider.transform);
            _Item_list.Add(new WaterFloatHelper(collider.transform, collider.attachedRigidbody, collider.attachedRigidbody.drag, collider.attachedRigidbody.angularDrag));
            _Transform_to_int[collider.transform] = _Item_list.Count - 1;

        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if (_Transform_set.Contains(collider.transform))
        {
            int _ObjectInList = _Transform_to_int[collider.transform];

            _AddForce(_ObjectInList);

            if (_Item_list[_ObjectInList]._AddForce)
            {
                _Item_list[_ObjectInList]._Rigidbody.useGravity = true;
            }
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (_Transform_set.Contains(collider.transform))
        {
            int _ObjectInList = _Transform_to_int[collider.transform];

            _Item_list[_ObjectInList]._Rigidbody.useGravity = true;
            _Item_list[_ObjectInList]._Rigidbody.drag = _Item_list[_ObjectInList]._OriginalDrag;
            _Item_list[_ObjectInList]._Rigidbody.angularDrag = _Item_list[_ObjectInList]._Original_Angular_Drag;

            _Item_list.RemoveAt(_ObjectInList);

            _Transform_to_int.Remove(collider.transform);

            _Transform_set.Remove(collider.transform);

            List<Transform> to_adjust = new List<Transform>();

            foreach (KeyValuePair<Transform, int> entry in _Transform_to_int)
            {
                if(entry.Value > _ObjectInList)
                {
                    to_adjust.Add(entry.Key);
                }
            }

            for(int i = 0; i < to_adjust.Count; i++)
            {
                _Transform_to_int[to_adjust[i]] = _Transform_to_int[to_adjust[i]] - 1;
            }
        }
    }

    public float _Max_Weight = 300f;
    public float _Mass_Force_Multiplier = 1f;
    public Vector3 _Down_Stream_Float_Strength;
    public float _ObjectsDrag = 0.5f;

    public void _AddForce(int _ObjectInList)
    {
        Transform _Object = _Item_list[_ObjectInList]._Transform;
        var _Distance2 = Vector3.Distance(transform.position, _Object.position);


        _Item_list[_ObjectInList]._AddForce = true;
        if (_Item_list[_ObjectInList]._Transform.position.y >= _Water_height)
        {
            _Force = 0f;
        }
        if (_Item_list[_ObjectInList]._Transform.position.y <= _Water_height - 0.2f)
        {
            _Force = 0.05f;
        }
        if (_Item_list[_ObjectInList]._Transform.position.y <= _Water_height -1)
        {
            _Force = 0.5f;
        }
        if (_Item_list[_ObjectInList]._Transform.position.y <= _Water_height - 2)
        {
            _Force = 1f;
        }

        if (_Item_list[_ObjectInList]._Transform.position.y < _Water_height)
        {
            //float rough_mass_calc = _Item_list[_ObjectInList]._Rigidbody.mass < 1f ? 1f : 1.0f+(Mathf.InverseLerp(1f, _Max_Weight, _Item_list[_ObjectInList]._Rigidbody.mass)) * _Mass_Force_Multiplier;
            //_Item_list[_ObjectInList]._Rigidbody.AddForce(Vector3.up * (_Force+Mathf.Abs(Physics.gravity.y)) * _Item_list[_ObjectInList]._Rigidbody.mass);
            _Item_list[_ObjectInList]._Rigidbody.AddForce(Vector3.up * (_Force + Mathf.Abs(Physics.gravity.y)) * _Item_list[_ObjectInList]._Rigidbody.mass);
            
        }
        else if (_Item_list[_ObjectInList]._Transform.position.y >= _Water_height)
        {
            _Item_list[_ObjectInList]._Rigidbody.useGravity = true;
        }
        
        _Item_list[_ObjectInList]._Rigidbody.AddForce(_Down_Stream_Float_Strength);

        float _CalculatedPosition = Mathf.Max(_ObjectsDrag, transform.position.y - _Object.position.y);

        if (_Item_list[_ObjectInList]._TransitionTime < 1.0f)
        {
            _Item_list[_ObjectInList]._TransitionTime += Time.deltaTime * (_CalculatedPosition);
            if (_Item_list[_ObjectInList]._TransitionTime >= 1.0f)
            {
                _Item_list[_ObjectInList]._TransitionTime = 1.0f;
            }
        }

        _Item_list[_ObjectInList]._Rigidbody.drag = _WaterDrag * _CalculatedPosition * _Item_list[_ObjectInList]._TransitionTime + (_WaterDrag * _CalculatedPosition * _Item_list[_ObjectInList]._TransitionTime);
        _Item_list[_ObjectInList]._Rigidbody.angularDrag = _WaterDrag * _CalculatedPosition * _Item_list[_ObjectInList]._TransitionTime;
        
    }

}