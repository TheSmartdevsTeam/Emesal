using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AnimateFontChangeScript : MonoBehaviour
{
    public TMP_FontAsset _DefaultFont;
    public TMP_FontAsset _ClearFont;
    
    private TMP_Text _CurrentText;

    float Timer = 3;

    void Start()
    {
        
        _DefaultFont = Resources.Load("CalligImprovis-Bold SDF") as TMP_FontAsset;
        _ClearFont = Resources.Load("Primeval SDF") as TMP_FontAsset;
        if (GetComponent<TMP_Text>())
        {
            _CurrentText = GetComponent<TMP_Text>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (GetComponent<TMP_Text>() && Timer < 0)
        {
            //_CurrentText = GetComponent<TMP_Text>();
            
            if(_CurrentText.font == _DefaultFont)
            {
                Timer = 0.01f;
                StartCoroutine(WaitForClearFontTime());
                _CurrentText.font = _ClearFont;
            }
            else
            {
                Timer = 0.01f;
                StartCoroutine(WaitForDefaultFontTime());
                _CurrentText.font = _DefaultFont;
            }
        }
    }

    IEnumerator WaitForClearFontTime()
    {
        yield return new WaitForSeconds(0.5f);
        Timer = 1f + Random.Range(1,5);
        _CurrentText.alpha = 0.1f;
        _CurrentText.alpha = 0.2f;
        _CurrentText.alpha = 0.3f;
        _CurrentText.alpha = 0.4f;
        _CurrentText.alpha = 0.5f;
        _CurrentText.alpha = 0.6f;
        _CurrentText.alpha = 0.7f;
        _CurrentText.alpha = 0.8f;
        _CurrentText.alpha = 0.9f;
        _CurrentText.alpha = 1f;
    }

    IEnumerator WaitForDefaultFontTime()
    {
        yield return new WaitForSeconds(0.5f);
        Timer = 1f + Random.Range(3, 5);
        _CurrentText.alpha = 0f;
    }

}
