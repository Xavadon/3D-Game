using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OK
{
    [RequireComponent(typeof(Collider))]
    public class EnemyCollisionHandler : MonoBehaviour
    {
        public void DisableCollisions()
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Collider>().enabled = false;
        }
    }
}
