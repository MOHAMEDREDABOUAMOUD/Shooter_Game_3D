using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : PickUpBase
{
    [SerializeField] private int healValue = 50;

    protected override void OnPickUp(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.currentHealth = (playerHealth.currentHealth + healValue) <= playerHealth.maxHealth
                ? playerHealth.currentHealth + healValue
                : playerHealth.maxHealth;
            playerHealth.UpdateHealthUI(playerHealth.currentHealth);
            Destroy(gameObject);
        }
    }
}
