using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script control where the crosshair is pointing at
/// the crossHair is used to indicate where the weapon is pointing at to shoot the raycast
/// </summary>
public class CrossHairTarget : MonoBehaviour
{
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hitInfo;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;
        // if the ray hit something we move the crosshair to the hit point
        if (Physics.Raycast(ray, out hitInfo))
        {
            transform.position = hitInfo.point;
        }
        // if the ray didn't hit anything we just point forward
        else
        {
            transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0.5F, 0.5F, 10f));
        }

    }
}
