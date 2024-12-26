using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

/// <summary>
/// Adding recoil to weapons with cinemachine
/// </summary>
public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public CharacterAiming characterAiming;
    [HideInInspector] public Cinemachine.CinemachineImpulseSource cameraShake;
    [HideInInspector] public Animator rigController;
    float verticalRecoil;
    float horizontalRecoil;
    public Vector2[] recoilPattern;

    public float duration;
    public float recoilModifier = 1f;

    private Camera mainCamera;

    private float time;
    private int index;

    private void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
        mainCamera = Camera.main;
    }

    public void Reset()
    {
        index = 0;
    }

    private int NextIndex(int i)
    {
        return (i + 1) % recoilPattern.Length;
    }

    public void GenerateRecoil(string weaponName)
    {
        time = duration;
        cameraShake.GenerateImpulse(mainCamera.transform.forward);
        horizontalRecoil = recoilPattern[index].x;
        verticalRecoil = recoilPattern[index].y;

        index = NextIndex(index);

        rigController.Play("weapon_recoil_" + weaponName, 1, 0f);
    }

    void Update()
    {
        if (time > 0)
        {
            characterAiming.yAxis.Value -= (((verticalRecoil / 10) * Time.deltaTime) / duration) * recoilModifier;
            characterAiming.xAxis.Value -= (((horizontalRecoil / 10) * Time.deltaTime) / duration) * recoilModifier;
            time -= Time.deltaTime;
        }
    }
}
