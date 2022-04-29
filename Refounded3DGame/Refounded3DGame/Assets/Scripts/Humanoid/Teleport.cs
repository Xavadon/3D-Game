using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class Teleport : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //transform.position = transform.rotation * Vector3.forward * 5 + transform.position;
                GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Fall", true, 0.1f);
                GetComponent<Rigidbody>().AddForce(transform.rotation * Vector3.forward * 10, ForceMode.Impulse);
            }
        }
    }
}