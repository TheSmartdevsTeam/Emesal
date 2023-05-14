using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetectionScript : MonoBehaviour
{
    float _CharacterCurrentWaterDepth;
    float _CharacterWaterSurfaceHeight;
    bool _CharacterSwimming;
    bool _CharacterUnderwater;

    public bool _GetCharacterSwimming()
    {
        return _CharacterSwimming;
    }
    public bool _GetCharacterUnderwater()
    {
        return _CharacterUnderwater;
    }
    private void OnTriggerEnter(Collider other)
    {
        _CharacterWaterSurfaceHeight = other.transform.position.y;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Water")
        {
            _CharacterCurrentWaterDepth = (_CharacterWaterSurfaceHeight - transform.position.y);

            if (_CharacterCurrentWaterDepth > 0.15f && _CharacterCurrentWaterDepth < 0.2f)
            {
                _CharacterSwimming = true;
            }
            else
            {
                _CharacterSwimming = false;
            }

            _ = _CharacterCurrentWaterDepth > 0.2f ? _CharacterUnderwater = true : _CharacterUnderwater = false;

        }
    }

    
}
