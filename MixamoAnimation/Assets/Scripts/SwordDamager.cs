using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log(enemy.name);
        }
    }
}
