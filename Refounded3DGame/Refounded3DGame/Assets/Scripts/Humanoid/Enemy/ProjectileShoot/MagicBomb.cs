using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class MagicBomb : MonoBehaviour
    {
        private Transform _target => PlayerSingleton.singleton.transform;
        [SerializeField] private float _damage;
        [SerializeField] private float _explodeRadius;
        [SerializeField] private float _radiusToExplode;
        [SerializeField] private float _timeToExplode = 0.3f;
        [SerializeField] private GameObject _explotion;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            Vector3 direction = (_target.position - transform.position) * 2 + Vector3.up;
            _rigidbody.AddForce(direction, ForceMode.Impulse);
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _target.position) < _explodeRadius)
            {
                StartCoroutine(nameof(Explode));
                StartCoroutine(nameof(DestroyAfterTime));
            }
            if (Vector3.Distance(transform.position, _target.position) < 1)
            {
                _timeToExplode = 0;
                StartCoroutine(nameof(Explode));
                StartCoroutine(nameof(DestroyAfterTime));
            }
        }

        private IEnumerator Explode()
        {
            yield return new WaitForSeconds(_timeToExplode);

            GameObject explotion = Instantiate(_explotion, transform.position, Quaternion.identity);
            explotion.transform.localScale = Vector3.one;
            gameObject.SetActive(false);

            DamageTarget();
        }

        private void DamageTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _explodeRadius);

            foreach (Collider collider in colliders)
            {
                //collider.GetComponent<PlayerHealth>().Hurt(_damage);
                if(collider.TryGetComponent<PlayerHealth>(out PlayerHealth player))
                {
                    player.Hurt(_damage);
                }
                //do try get component for player and enemies
            }

        }

        private IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(0.6f);
            Destroy(gameObject);
        }
    }
}
