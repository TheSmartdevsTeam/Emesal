using System.Collections;
using UnityEngine;

public class CharacterAttributesScript : MonoBehaviour
{
    
    CharacterControlScript _CharacterControlScript;
    #region Variables
    private string _CharacterFirstName;
    private string _CharacterLastName;

    private float _CharacterMaxHealth;
    private float _CharacterCurrentHealth;

    private float _CharacterMaxStamina;
    private float _CharacterCurrentStamina;

    private float _CharacterMaxMana;
    private float _CharacterCurrentMana;

    private float _CharacterBodyWeigth;
    private float _CharacterTotalWeigth;

    private float _CharacterHeight;

    private float _CharacterStrength;
    private float _CharacterAgility;
    private float _CharacterIntelligence;

    private float _CharacterPhysicalArmor;
    private float _CharacterElementalArmor;

    private float _FistAttackStaminaCost;

    private float _CharacterMeleeDamage;
    private float _CharacterRangeDamage;
    private float _CharacterSpellDamage;
    #endregion
    public float _GetCharacterMeleeDamage
    {
        get
        {
            return _CharacterMeleeDamage;
        }
    }
    public void _SetCHaracterMeleeDamage(float newDamage)
    {
        _CharacterMeleeDamage = newDamage;
    }
    public float _GetCharacterWeigth
    {
        get
        {
            return _CharacterBodyWeigth;
        }
    }
    public float _GetCharacterMaxMana
    {
        get
        {
            return _CharacterMaxMana;
        }
    }
    public float _GetCharacterCurrentMana
    {
        get
        {
            return _CharacterCurrentMana;
        }
    }
    public void _SetCharacterCurrentMana(float newmana)
    {
        _CharacterCurrentMana = newmana;
    }
    public float _GetCharacterMaxStamina
    {
        get
        {
            return _CharacterMaxStamina;
        }
    }
    public float _GetCharacterCurrentStamina
    {
        get
        {
            return _CharacterCurrentStamina;
        }
    }
    public void _SetCharacterCurrentStamina(float newstamina)
    {
        _CharacterCurrentStamina = newstamina;
    }
    public float _GethealthAsPrecentage()
    {
        return _CharacterCurrentHealth / _CharacterMaxHealth;
    }
    public float _GetCharacterCurrentHealth
    {
        get
        {
            return _CharacterCurrentHealth;
        }
    }
    public float _GetCharacterMaxHealth
    {
        get
        {
            return _CharacterMaxHealth;
        }
    }
    public float _GetFistAttackStaminaCost
    {
        get
        {
            return _FistAttackStaminaCost;
        }
    }
    public void _SetFistAttackStaminaCost(float newFistAttackStaminaCost)
    {
        _FistAttackStaminaCost = newFistAttackStaminaCost;
    }

    public void _SetCharacterCurrentHealth(float newhealth)
    {
        _CharacterCurrentHealth = newhealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        _CharacterControlScript = GetComponent<CharacterControlScript>();
        _CharacterMaxHealth = 100;
        _CharacterCurrentHealth = 50;

        _CharacterMaxStamina = 100;
        _CharacterCurrentStamina = 50;

        _CharacterMaxMana = 100;
        _CharacterCurrentMana = 50;

        _CharacterMeleeDamage = 10;
        _FistAttackStaminaCost = 5;
    }       

    void Update()
    {
        SetupHealthValues();
        SetupStaminaValues();
        SetupManaValues();

    }


    public float SetupHealthValues()
    {

        if (_CharacterCurrentHealth > _CharacterMaxHealth)
        {
            _CharacterCurrentHealth = _CharacterMaxHealth;
        }
        if (_CharacterCurrentHealth < 0)
        {
            _CharacterCurrentHealth = 0;
        }
        if (_CharacterCurrentHealth < _CharacterMaxHealth && _CharacterControlScript._GetCharacterAttacking() == false && _CharacterControlScript._GetCharacterChanneling() == false && _CharacterControlScript._GetCharacterCasting() == false)
        {
            StartCoroutine(RegenerateHealthWithDelay());
            return _CharacterCurrentHealth;
        }
        else
        {
            StopCoroutine(RegenerateHealthWithDelay());
            return _CharacterCurrentHealth;
        }
    }
    IEnumerator RegenerateHealthWithDelay()
    {

        yield return new WaitForSeconds(2);
        _CharacterCurrentHealth += 0.01f;

    }
    public float SetupStaminaValues()
    {
        
        if (_CharacterCurrentStamina > _CharacterMaxStamina)
        {
            _CharacterCurrentStamina = _CharacterMaxStamina;
        }
        if(_CharacterCurrentStamina < 0)
        {
            _CharacterCurrentStamina = 0;
        }
        if (_CharacterCurrentStamina < _CharacterMaxStamina && _CharacterControlScript._GetCharacterRunning() == false && _CharacterControlScript._GetCharacterJumping() == false)
        {
            StartCoroutine(RegenerateStaminaWithDelay());
            return _CharacterCurrentStamina;
        }
        else
        {
            
            StopCoroutine(RegenerateStaminaWithDelay());
            return _CharacterCurrentStamina;
        }
    }
    IEnumerator RegenerateStaminaWithDelay()
    {
        
        yield return new WaitForSeconds(2);
        _CharacterCurrentStamina += 0.01f;

    }

    public float SetupManaValues()
    {
        
        if (_CharacterCurrentMana > _CharacterMaxMana)
        {
            _CharacterCurrentMana = _CharacterMaxMana;
        }
        if (_CharacterCurrentMana < 0)
        {
            _CharacterCurrentMana = 0;
        }
        if (_CharacterCurrentMana < _CharacterMaxMana /*&& _CharacterControlScript._GetCharacterChanneling() == false && _CharacterControlScript._GetCharacterCasting() == false*/)
        {
            StartCoroutine(RegenerateManaWithDelay());
            return _CharacterCurrentMana;
        }
        else
        {

            StopCoroutine(RegenerateManaWithDelay());
            return _CharacterCurrentMana;
        }
    }
    IEnumerator RegenerateManaWithDelay()
    {

        yield return new WaitForSeconds(2);
        _CharacterCurrentMana += 0.01f;

    }
}
