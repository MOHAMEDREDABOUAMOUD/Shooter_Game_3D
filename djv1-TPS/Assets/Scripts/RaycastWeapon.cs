using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A weapon used by the player 
/// It can create bullet that are effected by gravity
/// </summary>
public class RaycastWeapon : MonoBehaviour
{
    /// <summary>
    /// A class used to represent a bullet fired by he weapon
    /// </summary>
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    public ActiveWeapon.WeaponSlot weaponSlot;
    [SerializeField]
    private float bulletSpeed = 1000f;
    [SerializeField]
    private float bulletDrop = 0f;

    public bool isFiring = false;
    [SerializeField]
    private int firerate = 25;

    public float damage = 10f;
    
    [SerializeField]
    private ParticleSystem[] muzzleFlash;
    [SerializeField]
    private ParticleSystem hitEffect;
    public Transform rayCastOrigin;
    public Transform rayCastDestination;
    [SerializeField]
    private TrailRenderer tracerEffect;
    public string weaponName;
    public LayerMask layerMask;

    public WeaponRecoil recoil;

    private Ray ray;
    private RaycastHit hitInfo;
    private float accumulatedTime;

    private List<Bullet> bullets = new();
    private float maxLifeTime = 3f;

    public void Awake()
    {
        recoil = GetComponent<WeaponRecoil>();
    }

    Vector3 GetPosition(Bullet bullet)
    {
        // p + v * t + 0.5 * g * t * t
        Vector3 gravity = Vector3.down * bulletDrop;
        return bullet.initialPosition + bullet.initialVelocity * bullet.time +
               gravity * (0.5f * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }
    public void StartFiring()
    {
        if (weaponSlot == ActiveWeapon.WeaponSlot.Primary)
        {
            if (Game.Instance.player.rifleAmmo > 0)
            {
                Game.Instance.player.rifleAmmo--;
            }
            else
            {
               // Do UI stuff when there is no amo
               return;
            }
        }
        accumulatedTime = 0f;
        isFiring = true;
        FireBullet();
        recoil.Reset();
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1f / (firerate + Game.Instance.player.attackSpeedBonus);
        while (accumulatedTime >= 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullet(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);
    
            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifeTime;
        }
        // Collision impulse

        if (hitInfo.collider.TryGetComponent<Rigidbody>(out var rb2d))
        {
            rb2d.AddForceAtPosition(ray.direction * 20, hitInfo.point, ForceMode.Impulse);
        }

        if (hitInfo.collider.TryGetComponent<HitBox>(out var hitBox))
        {
            hitBox.OnRaycastHit(this, ray.direction);
        }

        bullet.tracer.transform.position = end;
    }

    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLifeTime);
    }

    private void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            try
            {
                RaycastSegment(p0, p1, bullet);
            }
            catch (Exception)
            {
                // ignored
            }
        });
    }


    private void FireBullet()
    {
        if (weaponSlot == ActiveWeapon.WeaponSlot.Primary)
        {
            if (Game.Instance.player.rifleAmmo > 0)
            {
                Game.Instance.player.rifleAmmo--;
            }
            else
            {
                // Do UI stuff when there is no amo
                return;
            }
        }
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }
        Vector3 velocity = (rayCastDestination.position - rayCastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(rayCastOrigin.position, velocity);
        bullets.Add(bullet);

        recoil.GenerateRecoil(weaponName);
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    public void UpdateWeapon(float deltaTime)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartFiring();
        }

        if (isFiring)
        {
            UpdateFiring(deltaTime);
        }
        UpdateBullet(deltaTime);
        if (Input.GetButtonUp("Fire1"))
        {
            StopFiring();
        }
    }
}
