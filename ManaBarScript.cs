using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarScript : MonoBehaviour
{
    CharacterAttributesScript _PlayerAttributes;
    public Slider _Slider;

    void Start()
    {
        _Slider = GetComponent<Slider>();
        _PlayerAttributes = FindObjectOfType<CharacterAttributesScript>();
        SetMaxMana(_PlayerAttributes._GetCharacterMaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        SetCurrentMana(_PlayerAttributes._GetCharacterCurrentMana);
    }

    public void SetMaxMana(float mana)
    {
        _Slider.maxValue = _PlayerAttributes._GetCharacterMaxHealth;
        _Slider.value = _PlayerAttributes._GetCharacterCurrentMana;
    }
    public void SetCurrentMana(float mana)
    {
        _Slider.value = mana;
    }
}
