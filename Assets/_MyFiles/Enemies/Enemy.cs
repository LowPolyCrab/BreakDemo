using System;
using System.Runtime.CompilerServices;
using Unity.Behavior;
using UnityEngine;
using UnityEngineInternal;

[RequireComponent (typeof(HealthComponent))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    private HealthComponent _healthComponent;
    private Animator _animator;
    private static readonly int DeathId = Animator.StringToHash("Dead");

    private PerceptionComponent _perceptionComponent;
    private BehaviorGraphAgent _behaviorGraphAgent;

    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnTakenDamage += TookDamage;
        _healthComponent.OnDead += StartDeath;
        _animator = GetComponent<Animator>();
        _perceptionComponent = GetComponent<PerceptionComponent>();
        _perceptionComponent.OnPerceptionTargetUpdated += HandleTargetUpdate;
        _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }

    private void HandleTargetUpdate(GameObject target, bool bIsSensed)
    {
        if (bIsSensed)
        {
            _behaviorGraphAgent.BlackboardReference.SetVariableValue("Target", target);
        }
        else
        {
            _behaviorGraphAgent.BlackboardReference.SetVariableValue<GameObject>("Target", null);
        }
    }

    private void TookDamage(float newHealth, float delta, float maxHealth, GameObject instigator)
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
