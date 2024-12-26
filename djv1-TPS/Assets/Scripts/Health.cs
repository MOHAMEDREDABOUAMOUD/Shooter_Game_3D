using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material blinkMaterial;
    private Material originalMaterial;

    [SerializeField] private float blinkDuration;
    private float blinkTimer;

    private SkinsManager skinsManager;
    

    [SerializeField] private UIHealthBar healthBar;
    void Start()
    {
        skinsManager = GetComponent<SkinsManager>();
        skinnedMeshRenderer = skinsManager.GetSelectedSkin().GetComponent<SkinnedMeshRenderer>();
        currentHealth = maxHealth;
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
            if (hitBox.gameObject != gameObject)
            {
                hitBox.gameObject.layer = LayerMask.NameToLayer("Hitbox");
            }
        }

        originalMaterial = skinnedMeshRenderer.material;

        //healthBar = GetComponentInChildren<UIHealthBar>();
        OnStart();  
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        if(currentHealth < 0) return;
        currentHealth -= amount;
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        OnDamage(direction);
        if (currentHealth <= 0f)
        {
            Die(direction);
        }

        blinkTimer = blinkDuration;
        skinnedMeshRenderer.material = blinkMaterial;
    }

    public void UpdateHealthUI(float newHeath)
    {
        if(newHeath < maxHealth)
            healthBar.SetHealthBarPercentage(newHeath / maxHealth);
        else
            healthBar.SetHealthBarPercentage(1);
    }

    private void Die(Vector3 direction)
    {
        OnDeath(direction);

    }

    private void Update()
    {
            
        blinkTimer -= Time.deltaTime;
        if (blinkTimer < 0)
        {
            skinnedMeshRenderer.material = originalMaterial;
        }
    }

    protected virtual void OnStart()
    {
        
    }

    protected virtual void OnDeath(Vector3 direction)
    {
        
    }

    protected virtual void OnDamage(Vector3 direction)
    {
        
    }
}
