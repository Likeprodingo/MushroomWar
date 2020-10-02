using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100;
    private float _health;
    [SerializeField]
    private Slider _slider = default;
    
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

    IEnumerator Death()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Death");
    }
    private void HealthBarUpdate()
    {
        _slider.value = _health / _maxHealth;
    }
}
