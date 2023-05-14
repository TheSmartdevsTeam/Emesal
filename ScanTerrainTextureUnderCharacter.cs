using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScanTerrainTextureUnderCharacter : MonoBehaviour
{
    public string _CurrentLayer;
    public string _GetCurrentLayer { get => _CurrentLayer; set => _CurrentLayer = value; }
    public FootstepsCollectionScript[] _FootstepsCollection;
    Terrain terrain;

    private float[] GetTextureMix(Vector3 _CharacterTransform, Terrain _CurrentTerrain)
    {
        Vector3 _TransformPosition = _CurrentTerrain.transform.position;
        TerrainData _TerrainData = _CurrentTerrain.terrainData;
        int _MapX = Mathf.RoundToInt((_CharacterTransform.x - _TransformPosition.x) / _TerrainData.size.x * _TerrainData.alphamapWidth);
        int _MapZ = Mathf.RoundToInt((_CharacterTransform.z - _TransformPosition.z) / _TerrainData.size.z * _TerrainData.alphamapHeight);
        float[,,] _AplhaMap = _TerrainData.GetAlphamaps(_MapX, _MapZ, 1, 1);

        float[] _CharacterPositionTextureMix = new float[_AplhaMap.GetUpperBound(2) + 1];
        for(int i = 0; i<_CharacterPositionTextureMix.Length; i++)
        {
            _CharacterPositionTextureMix[i] = _AplhaMap[0, 0, i];
        }
        return _CharacterPositionTextureMix;
    }

    public string GetLayerName(Vector3 _PlayerPosition, Terrain _Terrain)
    {
        float[] _CharacterPositionTextureMix = GetTextureMix(_PlayerPosition, _Terrain);
        float _Strongest = 0;
        int _MaxIndex = 0;
        for(int i=0;i<_CharacterPositionTextureMix.Length;i++)
        {
            if(_CharacterPositionTextureMix[i] > _Strongest)
            {
                _MaxIndex = i;
                _Strongest = _CharacterPositionTextureMix[i];
            }
        }
        return _Terrain.terrainData.terrainLayers[_MaxIndex].name;
    }

    public string GetLayerNameOnObject(Vector3 _PlayerPosition, Terrain _Terrain, MeshCollider _MeshCollider)
    {
        float[] _CharacterPositionTextureMix = GetTextureMix(_PlayerPosition, _Terrain);
        float _Strongest = 0;
        int _MaxIndex = 0;
        for (int i = 0; i < _CharacterPositionTextureMix.Length; i++)
        {
            if (_CharacterPositionTextureMix[i] > _Strongest)
            {
                _MaxIndex = i;
                _Strongest = _CharacterPositionTextureMix[i];
            }
        }
        return _Terrain.terrainData.terrainLayers[_MaxIndex].name;
    }



    public void CheckLayers()
    {
        GameObject terrainGO = GameObject.FindGameObjectWithTag("Terrain");
        Terrain terrain = terrainGO.GetComponent<Terrain>();
        RaycastHit hit;
        if(Physics.Raycast(transform.position,Vector3.down,out hit,3))
        {
            if (hit.transform.GetComponent<Terrain>() != null)
            {
                Terrain t = hit.transform.GetComponent<Terrain>();
                _CurrentLayer = GetLayerName(transform.position, t);
                foreach (FootstepsCollectionScript collectionScript in _FootstepsCollection)
                {
                    if (_CurrentLayer == collectionScript.name)
                    {
                        CharacterControlScript ccs = GetComponent<CharacterControlScript>();
                        ccs.SwapFootsteps(collectionScript);
                    }
                }
                if (_CurrentLayer != GetLayerName(transform.position, t))
                {
                    _CurrentLayer = GetLayerName(transform.position, t);
                    foreach (FootstepsCollectionScript collectionScript in _FootstepsCollection)
                    {
                        if(_CurrentLayer == collectionScript.name)
                        {
                            CharacterControlScript ccs = GetComponent<CharacterControlScript>();
                            ccs.SwapFootsteps(collectionScript);
                        }
                    }
                }
            }
            else
            {
                /*
                if (hit.transform.name.Contains("PLANK"))
                {
                    Debug.Log("PLANK");
                    // transform it into object detect to layer
                    GameObject go = hit.transform.GetComponent<GameObject>();
                    MeshCollider mc = hit.transform.GetComponent<MeshCollider>();
                    
                    if (_CurrentLayer != GetLayerNameOnObject(transform.position, terrain,mc))
                    {
                        
                        _CurrentLayer = GetLayerNameOnObject(transform.position, terrain, mc);


                        foreach (FootstepsCollectionScript collectionScript in _FootstepsCollection)
                        {
                            if (_CurrentLayer == collectionScript.name)
                            {
                                CharacterControlScript ccs = GetComponent<CharacterControlScript>();
                                ccs.SwapFootsteps(collectionScript);
                            }
                        }
                    }
                }*/
            }
        }
    }

}
