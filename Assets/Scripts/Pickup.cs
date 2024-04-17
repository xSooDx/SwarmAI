using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody && other.attachedRigidbody.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        CoinManager.Instance.AddCoin();
    }

    private void OnDisable()
    {
        CoinManager.Instance.RemoveCoin();
    }
}
