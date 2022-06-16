using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(BoxCollider))]
    public class EnemyDamager : MonoBehaviour
    {
        [SerializeField] private AudioList _sound;
        private AudioSource _аudioSource;

        private void Awake()
        {
            _аudioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerHealth player))
            {
                player.Hurt(GetComponentInParent<EnemyCombat>().Damage);
                if(_sound && player.GetComponent<PlayerHealth>().PlayerFlags.canTakeDamage) PlaySound();
            }
        }

        private void PlaySound()
        {
            if (_sound)
            {
                _аudioSource.clip = _sound.hit[Random.Range(0, _sound.hit.Length)];
                _аudioSource.Play();
            }
        }
    }
}
