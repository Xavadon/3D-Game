using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class Player : MonoBehaviour
    {
        public static Player singleton { get; private set; }

        private void Awake()
        {
            singleton = this;
        }
    }
}
