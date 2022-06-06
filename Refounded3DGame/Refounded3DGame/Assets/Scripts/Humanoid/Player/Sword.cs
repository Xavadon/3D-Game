using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(AudioSource))]
    public class Sword : MonoBehaviour
    {
        [SerializeField] private Sound _sound;
        private AudioSource _аudioSource;

        private void Awake()
        {
            _аudioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.Hurt(GetComponentInParent<PlayerCombat>().Damage);
                PlaySound();
            }
        }

        private void PlaySound()
        {
            _аudioSource.clip = _sound.sword[Random.Range(0, _sound.sword.Length)];
            _аudioSource.Play();
        }
    }
}
