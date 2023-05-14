using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarScript : MonoBehaviour
{
    CharacterAttributesScript _PlayerAttributes;
    public Slider _Slider;

    void Start()
    {
        _Slider = GetComponent<Slider>();
        _PlayerAttributes = FindObjectOfType<CharacterAttributesScript>();
        SetMaxStamina(_PlayerAttributes._GetCharacterMaxStamina);
        
    }

    void Update()
    {
        SetCurrentStamina(_PlayerAttributes._GetCharacterCurrentStamina);
    }

    public void SetMaxStamina(float stamina)
    {
        _Slider.maxValue = _PlayerAttributes._GetCharacterMaxStamina;
        _Slider.value = _PlayerAttributes._GetCharacterCurrentStamina;
    }
    public void SetCurrentStamina(float stamina)
    {
        _Slider.value = stamina;
    }


}
