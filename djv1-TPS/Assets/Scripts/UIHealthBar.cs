using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Image foreground;
    [SerializeField] private Image background;
    [SerializeField] private bool followTarget;
    private Camera mainCamera;
    

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void DisableUI()
    {
        foreground.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
    }

    public void EnableUI()
    {
        foreground.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        if(!followTarget)
            return;
        Vector3 direction = (target.position - mainCamera.transform.position).normalized;
        bool isBehind = Vector3.Dot(direction, mainCamera.transform.forward) <= 0f;
        foreground.enabled = !isBehind;
        background.enabled = !isBehind;
        transform.position = mainCamera.WorldToScreenPoint(target.position + offset);
    }

    public void SetHealthBarPercentage(float percentage)
    {
        var rectTransformAnchorMax = foreground.rectTransform.anchorMax;
        rectTransformAnchorMax.x = percentage;
        foreground.rectTransform.anchorMax = rectTransformAnchorMax;
    }
}
