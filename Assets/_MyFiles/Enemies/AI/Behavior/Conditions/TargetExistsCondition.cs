using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "TargetExists", story: "[Target] Exists [Condition]", category: "Conditions", id: "2d52faecb03ff49f741ad6177b92379d")]
public partial class TargetExistsCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<bool> Condition;

    public override bool IsTrue()
    {
        bool targetExists = Target.Value != null;
        return targetExists && Condition.Value;
    }
}
