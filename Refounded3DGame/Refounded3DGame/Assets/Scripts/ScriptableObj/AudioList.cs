using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [CreateAssetMenu(fileName = "ScriptableObjects/Sound")]
    public class AudioList: ScriptableObject
    {
        public AudioClip[] footsteps;
        public AudioClip[] hit;
        public AudioClip[] swordSwings;
        public AudioClip[] hurt;
        public AudioClip[] dead;
        public AudioClip[] jump;
    }
}
