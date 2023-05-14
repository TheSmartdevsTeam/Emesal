using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    CharacterAttributesScript _PlayerAttributes;
    public Slider _Slider;
    
    void Start()
    {
        _Slider = GetComponent<Slider>();
        _PlayerAttributes = FindObjectOfType<CharacterAttributesScript>();

    }

    // Update is called once per frame
    void Update()
    {
        SetMaxHealth();
        SetCurrentHealth(_PlayerAttributes.SetupHealthValues());
        SetMaxStamina();
        SetCurrentStamina(_PlayerAttributes.SetupStaminaValues());
        SetMaxMana();
        SetCurrentMana(_PlayerAttributes.SetupManaValues());
        
    }

    public void SetMaxHealth()
    {
        
        if (_Slider.gameObject.name == "Health Bar")
        {
            _Slider.maxValue = _PlayerAttributes._GetCharacterMaxHealth;
            _Slider.minValue = 0;
        }
        
    }
    public void SetCurrentHealth(float health)
    {
        
        if (_Slider.gameObject.name == "Health Bar")
        {
            _Slider.value = health;
        }
        
    }
    public void SetMaxStamina()
    {
        if (_Slider.gameObject.name == "Stamina Bar")
        {
            _Slider.maxValue = _PlayerAttributes._GetCharacterMaxStamina;
            _Slider.minValue = 0;
        }
        
    }
    public void SetCurrentStamina(float stamina)
    {
        if (_Slider.gameObject.name == "Stamina Bar")
        {
            _Slider.value = stamina;
        }

    }
    public void SetMaxMana()
    {
        if (_Slider.gameObject.name == "Mana Bar")
        {
            _Slider.maxValue = _PlayerAttributes._GetCharacterMaxMana;
            _Slider.minValue = 0;
        }
        
    }
    public void SetCurrentMana(float mana)
    {
        if (_Slider.gameObject.name == "Mana Bar")
        {
            _Slider.value = mana;
        }

    }
}
