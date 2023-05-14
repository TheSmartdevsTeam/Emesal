using System.Collections;
using UnityEngine;

public class CastSpellScript : MonoBehaviour
{
    [Header("Spells")]
    public GameObject Spell;
    public Transform CastPosition;
    public float CastForce = 10;
    public bool InfiniteMana;

    public int NumberOfSpells = 3;
    public int MaxNumberOfSpells = 3;

    public float MaxForceMultiplier = 3; // The maximum force multiplier.
    private float ForceMultiplier = 0; // Throw force multiplier.

    public float DelayToCast = 0.3f; // Delay until instantiate the grenade.

    [Header("Animations")]
    public Animation SpellAnim;

    [Space()]
    public string PullAnimName = "";
    public string ThrowAnimName = "";

    public AudioClip PullSound;
    public AudioClip ThrowSound;

    public float PullVolume = 0.3f;
    public float ThrowVolume = 0.3f;

    private bool IsThrowing; // Is already throwing a grenade?
    private bool CanThrow; // Can throw a grenade?

    [Space()]
    public WeaponsManagerScript weaponManager;
    public AudioManagerScript audioManager;
    public PlayerUIScript ui;



    private void Update()
    {
        GetUserInput(); // Checks if the user is pressing some action key.
        UpdateUI();  // Update the UI showing the amount of grenades.
    }

    /// <summary>
    /// Updates information about the items in the UI.
    /// </summary>
    private void UpdateUI()
    {
        ui.SetGranadesAmount(NumberOfSpells); // Updates the amount of grenades.

        if (IsThrowing) // Is throwing a grenade?
        {
            ui.ShowCrosshair(true); // Enable the crosshair.
            ui.SetCrosshairType(CrosshairStyle.Point); // Defines the type as a point.
        }
    }

    /// <summary>
    /// Checks whether the user is pressing any action key and invokes the corresponding method.
    /// Action keys: G = (Throw a grenade).
    /// </summary>
    public void GetUserInput()
    {
        if (NumberOfSpells > 0) // Still have grenades available?
        {
            if (!IsThrowing) // Is not already throwing a grenade?
            {
                if (Input.GetKeyDown(KeyCode.V) && weaponManager.canUseItems)
                {
                    Debug.Log("3");
                    HoldToThrowGrenade(); // Pull the grenade pin.
                    CanThrow = true; // Can throw the grenade.
                }
            }
            else
            {
                // Hold the Key to throw the grenade with more force.
                if (Input.GetKey(KeyCode.V))
                {
                    if (ForceMultiplier <= MaxForceMultiplier)
                        ForceMultiplier += Time.deltaTime; // Increase the force multiplier with time.
                }

                // Release the Key to throw the grenade with the current force (forceMultiplier).
                if (Input.GetKeyUp(KeyCode.V) && CanThrow)
                {
                    CanThrow = false;
                    StartCoroutine(ThrowGrenade(ForceMultiplier));
                    ForceMultiplier = 0;
                }
            }
        }
    }

    /// <summary>
    /// Method that starts the process of throwing a grenade.
    /// </summary>
    private void HoldToThrowGrenade()
    {
        IsThrowing = true;
        weaponManager.HideCurrentWeapon(); // Hides the current weapon.

        PullAnimation(); // Play Pull the Pin Animation.
    }

    /// <summary>
    /// Play throwing grenade animation and invokes the method that instantiates the grenade.
    /// Parameters: How much time the player holds the grenade.
    /// </summary>
    private IEnumerator ThrowGrenade(float holdTime)
    {
        if (GetPullAnimTime() < holdTime) // If has finished the pull the pin animation.
        {
            //ThrowAnimation(); // Play throw animation.

            yield return new WaitForSeconds(DelayToCast);

            InstantiateGrenade(holdTime);

            yield return new WaitForSeconds(GeThrowAnimTime() - DelayToCast);
            weaponManager.SelectCurrentWeapon(); // Activate the current weapon after throwing the grenade.
            IsThrowing = false;
        }
        else // Wait until finish the pull animation to play throw animation.
        {
            yield return new WaitForSeconds(GetPullAnimTime() - holdTime);

            ThrowAnimation(); // Play throw animation.

            yield return new WaitForSeconds(DelayToCast);

            InstantiateGrenade(holdTime);

            yield return new WaitForSeconds(GeThrowAnimTime() - DelayToCast);
            weaponManager.SelectCurrentWeapon(); // Activate the current weapon after throwing the grenade.
            IsThrowing = false;
        }
    }

    /// <summary>
    /// Instantiates the grenade.
    /// </summary>
    private void InstantiateGrenade(float holdTime)
    {
        // Create a grenade.
        GameObject SpellClone = Instantiate(Spell, CastPosition.position, CastPosition.rotation) as GameObject;

        //grenadeClone.GetComponent<GrenadeScript>().Detonate(holdTime);
        SpellClone.GetComponent<SpellScript>().Detonate(); // Calls the method responsible for blowing up the grenade.

        // Adds force to the grenade to throw it forward.
        SpellClone.GetComponent<Rigidbody>().velocity = SpellClone.transform.TransformDirection(Vector3.forward)
            * CastForce * (holdTime > 1 ? holdTime : 1);

        if (!InfiniteMana)
            NumberOfSpells--;
    }

    /// <summary>
    /// Method responsible for play the Pull animation.
    /// </summary>
    private void PullAnimation()
    {
        SpellAnim.Play(PullAnimName);
        audioManager.PlayGenericSound(PullSound, PullVolume);
    }

    /// <summary>
    /// Method responsible for play the Throw animation.
    /// </summary>
    private void ThrowAnimation()
    {
        SpellAnim.Play(ThrowAnimName);
        audioManager.PlayGenericSound(ThrowSound, ThrowVolume);
    }

    /// <summary>
    /// Returns the duration of the Throw animation in seconds.
    /// </summary>
    private float GeThrowAnimTime()
    {
        return SpellAnim != null ? ThrowAnimName.Length > 0 ? SpellAnim[ThrowAnimName].length : 0 : 0;
    }

    /// <summary>
    /// Returns the duration of the Pull animation in seconds.
    /// </summary>
    private float GetPullAnimTime()
    {
        return SpellAnim != null ? PullAnimName.Length > 0 ? SpellAnim[PullAnimName].length : 0 : 0;
    }
}
