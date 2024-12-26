using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUpBase : MonoBehaviour
{
    public GameObject pickUpEffectPrefab;
    
    private void OnTriggerEnter(Collider other)
    {
        OnPickUp(other);
        StartCoroutine(SpawnEffect());
    }

    private IEnumerator SpawnEffect()
    {
        var effect = Instantiate(pickUpEffectPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        Destroy(effect);
    }

    protected virtual void OnPickUp(Collider other)
    {
        
    }
}
