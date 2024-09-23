using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent (typeof(HealthComponent))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    private HealthComponent _healthComponent;
    private Animator _animator;
    private static readonly int DeathId = Animator.StringToHash("Dead");

    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnTakenDamage += TookDamage;
        _healthComponent.OnDead += StartDeath;
        _animator = GetComponent<Animator>();
    }

    private void TookDamage(float newHealth, float delta, float maxHealth)
    {
        Debug.Log($"{this.gameObject.name} took {delta} amt of damage, health is now {newHealth}/{maxHealth}");
    }

    private void StartDeath()
    {
        Debug.Log($"{this.gameObject.name} died");
        _animator.SetTrigger(DeathId);
    }

    public void DeathAnimationFinished()
    {
        Destroy(gameObject);
    }
}
