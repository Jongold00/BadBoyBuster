using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<PlayerController>().HitByCar();
    }
}
