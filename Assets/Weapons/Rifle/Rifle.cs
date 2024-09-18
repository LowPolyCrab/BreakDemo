using UnityEngine;

[RequireComponent(typeof(AimingComponent))]
public class Rifle : Weapon
{
    [SerializeField] private float damage = 5f;
    private AimingComponent _aimingComponent;
    private void Awake()
    {
        _aimingComponent = GetComponent<AimingComponent>();
    }
    public override void Attack()
    {
        AimResult target = _aimingComponent.GetAimResult(Owner.transform);
        if (target.target)
        {
            HealthComponent targetHealthComponent = target.target.GetComponent<HealthComponent>();
            targetHealthComponent?.ChangeHealth(-damage);
        }
    }
}
