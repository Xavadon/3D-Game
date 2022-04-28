using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _enemy;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Instantiate(_enemy, transform.position, Quaternion.identity);
            }
        }
    }
}
