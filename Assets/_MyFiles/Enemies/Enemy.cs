using System;
using UnityEngine;

[RequireComponent (typeof(HealthComponent))]
public class Enemy : MonoBehaviour
{
    private HealthComponent _healthComponent;

    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnTakenDamage += TookDamage;
        _healthComponent.OnDead += StartDeath;
    }

    private void TookDamage(float newHealth, float delta, float maxHealth)
    {
        Debug.Log($"{this.gameObject.name} took {delta} amt of damage, health is now {newHealth}/{maxHealth}");
    }

    private void StartDeath()
    {
        Debug.Log($"{this.gameObject.name} died");
        Destroy(this.gameObject);
    }
}
