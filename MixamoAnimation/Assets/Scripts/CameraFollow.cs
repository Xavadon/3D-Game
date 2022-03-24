using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(26, 0, 0);
    }
    private void Update()
    {
        transform.position = new Vector3(_target.position.x, _target.position.y + 2.5f, _target.position.z - 1.9f);
    }
}
