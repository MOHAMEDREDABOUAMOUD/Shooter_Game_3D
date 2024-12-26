using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : Health
{
    private Ragdoll ragdoll;
    private ActiveWeapon weapon;
    private CharacterAiming characterAiming;
    private PostProcessVolume m_Volume;
    private Vignette m_Vignette;
    private CameraManager cameraManager;
    
    protected override void OnStart()
    {
        ragdoll = GetComponent<Ragdoll>();
        weapon = GetComponent<ActiveWeapon>();
        characterAiming = GetComponent<CharacterAiming>();
        
        // Create an instance of a vignette
        m_Vignette = ScriptableObject.CreateInstance<Vignette>();
        m_Vignette.enabled.Override(true);
        m_Vignette.intensity.Override(0f);
        // Use the QuickVolume method to create a volume with a priority of 100, and assign the vignette to this volume
        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Vignette);

        // To do : change with instance from game manager 
        cameraManager = FindObjectOfType<CameraManager>();
    }

    protected override void OnDeath(Vector3 direction)
    {
        ragdoll.ActivateRagdoll();
        direction.y = 1f;
        ragdoll.ApplyForce(direction);
        weapon.DropWeapon();
        characterAiming.enabled = false;
        cameraManager.EnableKillCam();
    }

    protected override void OnDamage(Vector3 direction)
    {
        var percent = 1f - (currentHealth / maxHealth);
        m_Vignette.intensity.value = percent * 0.6f;
    }

    public new void TakeDamage(float amount, Vector3 direction)
    {
        if(!CharacterLocomotion.isRolling)
            base.TakeDamage(amount, direction);
    }
}
