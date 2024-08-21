using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _health;
    private float _maxHealth;

    public event Action OnTakeHit;
    public event Action OnHeal;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(float maxHealth)
    {
        _maxHealth = maxHealth;
        _health = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        OnTakeHit?.Invoke();
    }

    public void Heal(float heal)
    {
        _health += heal;
        OnHeal?.Invoke();
    }
}
