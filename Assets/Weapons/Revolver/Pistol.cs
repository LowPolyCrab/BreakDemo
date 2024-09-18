using UnityEngine;

[RequireComponent(typeof(AimingComponent))]
public class Pistol : Weapon
{
    private AimingComponent _aimingComponent;
    private void Awake()
    {
        _aimingComponent = GetComponent<AimingComponent>();
    }
    public override void Attack()
    {
        AimResult target = _aimingComponent.GetAimResult(Owner.transform);
        if(target.target)
            Debug.Log($"damaging {target.target.name}");
    }
}
