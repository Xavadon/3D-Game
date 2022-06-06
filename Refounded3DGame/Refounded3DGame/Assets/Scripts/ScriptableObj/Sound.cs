using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [CreateAssetMenu(fileName = "ScriptableObjects/Sound")]
    public class Sound : ScriptableObject
    {
        public AudioClip[] sword;
        public AudioClip[] steps;
    }
}
