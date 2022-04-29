using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class CursorHide : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.visible = false;
        }
    }
}
