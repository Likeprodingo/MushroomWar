using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Player
{
    public class PlayerHealthScript: MonoBehaviour,IHealth
    {
        [SerializeField] protected float _maxHealth = 100;
        [SerializeField] private Slider _slider = default;
        private float _health;
        private void Start()
        {
            _health = _maxHealth;
            HealthBarUpdate();
        }

        public void GetDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                StartCoroutine(Death());
            }
            HealthBarUpdate();
        }

        public IEnumerator Death()
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Death");
        }
        private void HealthBarUpdate()
        {
            _slider.value = _health / _maxHealth;
        }
    }
}
