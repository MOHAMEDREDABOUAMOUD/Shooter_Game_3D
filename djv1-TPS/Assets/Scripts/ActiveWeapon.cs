using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This class manage the weapons used by the player, switch, pick up and equip/unequip weapons
/// </summary>
public class ActiveWeapon : MonoBehaviour
{
    /// <summary>
    /// enum class used to specify weapon type
    /// </summary>
    public enum WeaponSlot
    {
        Primary = 0,
        Secondary = 1
    }
    // Point where the weapon is aiming at
    [SerializeField]
    private Transform crossHairTarget;
    [SerializeField]
    private Animator rigController;
    [SerializeField]
    private Transform[] weaponSlots;
    [SerializeField]
    private CharacterAiming characterAiming;

    public bool isChangingWeapon;

    private RaycastWeapon[] equippedWeapons = new RaycastWeapon[2];
    private int activeWeaponIndex;
    private bool isHolstered = false;
    private int notSpritingParam;


    void Start()
    {
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equipe(existingWeapon);
        }
        notSpritingParam = Animator.StringToHash("notSprinting");
    }

    /// <summary>
    /// Used to test is the player is firing the weapon or not
    /// </summary>
    /// <returns> returns true if the weapon is been fired</returns>
    public bool IsFiring()
    {
        RaycastWeapon currentWeapon = GetActiveWeapon();
        if (!currentWeapon)
        {
            return false;
        }

        return currentWeapon.isFiring;
    }

    /// <summary>
    /// returned the equiped weapon currently in use
    /// </summary>
    public RaycastWeapon GetActiveWeapon()
    {
        return GetWeapon(activeWeaponIndex);
    }

    /// <summary>
    /// return weapon for the given index
    /// </summary>
    /// <param name="index">primary or secondary weapon</param>
    RaycastWeapon GetWeapon(int index)
    {
        if (index < 0 || index > equippedWeapons.Length)
            return null;
        return equippedWeapons[index];
    }

    void Update()
    {
        var weapon = GetWeapon(activeWeaponIndex);
        // Test if we are in the sprinting animation
        bool notSprinting = rigController.GetCurrentAnimatorStateInfo(2).shortNameHash ==
                            notSpritingParam;
        if (weapon && !isHolstered && notSprinting && !CharacterLocomotion.isRolling)
        {
            weapon.UpdateWeapon(Time.deltaTime);
        }
        // Hostle weapon
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleActiveWeapon();
        }
        // Equip primary weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(WeaponSlot.Primary);
        }
        // Equip secondary weapon
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveWeapon(WeaponSlot.Secondary);
        }
    }

    /// <summary>
    /// Equip a weapon to the player
    /// </summary>
    /// <param name="newWeapon"> The weapon to equip</param>
    public void Equipe(RaycastWeapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.rayCastDestination = crossHairTarget;
        weapon.recoil.characterAiming = characterAiming;
        weapon.recoil.rigController = rigController;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        equippedWeapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(newWeapon.weaponSlot);
    }

    /// <summary>
    /// Change the stat of the weapon (active or holstered)
    /// </summary>
    void ToggleActiveWeapon()
    {
        bool isHolstered = rigController.GetBool("holster_weapon");
        if (isHolstered)
        {
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(activeWeaponIndex));
        }
    }

    /// <summary>
    /// Make a weapon active
    /// </summary>
    /// <param name="weaponSlot">weapon to activate, primary or secondary</param>
    void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponSlot;

        if (holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }

        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }


    IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        rigController.SetInteger("weapon_index", activateIndex);
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;
    }

    IEnumerator HolsterWeapon(int index)
    {
        while (isChangingWeapon)
            yield return 0;
        isChangingWeapon = true;
        isHolstered = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_weapon", true);
            do
            {
                yield return new WaitForNextFrameUnit();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
        isChangingWeapon = false;
    }

    IEnumerator ActivateWeapon(int index)
    {
        while (isChangingWeapon)
            yield return 0;
        isChangingWeapon = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_weapon", false);
            do
            {
                rigController.Play("equip_" + weapon.weaponName);
                yield return new WaitForNextFrameUnit();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isHolstered = false;
        }
        isChangingWeapon = false;
    }

    public void DropWeapon()
    {
        var currentWeapon = GetActiveWeapon();
        if (currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            equippedWeapons[activeWeaponIndex] = null;
        }
    }
}
