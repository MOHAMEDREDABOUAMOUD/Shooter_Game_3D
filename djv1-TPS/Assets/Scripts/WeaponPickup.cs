using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script enable the player to pick up new weapon on collision 
/// </summary>
public class WeaponPickup : PickUpBase
{
    private enum WeaponType
    {
        Pistol,
        Rifle
    }
    public RaycastWeapon weaponFab;
    [SerializeField] private WeaponType weaponType;

    protected override void OnPickUp(Collider other)
    {
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        if (activeWeapon)
        {
            RaycastWeapon newWeapon = Instantiate(weaponFab);
            if (weaponType == WeaponType.Rifle)
            {
                Game.Instance.player.rifleAmmo += 100;
            }
            activeWeapon.Equipe(newWeapon);
            Destroy(gameObject);
        }
    }
}
