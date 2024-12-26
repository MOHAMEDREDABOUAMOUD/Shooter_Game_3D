using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : PickUpBase
{
    [SerializeField] private int ammoValue;

    protected override void OnPickUp(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.rifleAmmo += ammoValue;
            Destroy(gameObject);
        }
    }
}
