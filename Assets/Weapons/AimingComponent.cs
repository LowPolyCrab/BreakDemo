using Unity.VisualScripting;
using UnityEngine;

public struct AimResult
{
    public GameObject target;
    public Vector3 aimStart, aimDir;

    public AimResult(GameObject inTarget, Vector3 inAimStart, Vector3 inAimDir)
    {
        target = inTarget;
        aimStart = inAimStart;
        aimDir = inAimDir;
    }
}

public class AimingComponent : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private float aimRange = 10f;
    [SerializeField] private LayerMask aimMask;
    [SerializeField] private bool bOverrides = true;
    [SerializeField] private float heightOverride = 1.5f;
    [SerializeField] private float fwdOffset = 0.5f;

    private Vector3 _debugAimStart;
    private Vector3 _debugAimDir;

    public AimResult GetAimResult(Transform aimTransform)
    {
        Vector3 aimStart = muzzle.position;
        Vector3 aimDir = muzzle.forward;
        if (aimTransform)
        {
            aimStart = aimTransform.position;
            aimDir = aimTransform.forward;
        }
        if (bOverrides)
        {
            aimStart.y = heightOverride;
            aimStart += aimDir * fwdOffset;
        }

        _debugAimStart = aimStart;
        _debugAimDir = aimDir;

        GameObject target = null;
        if(Physics.Raycast(aimStart, aimDir, out RaycastHit hitInfo, aimRange, aimMask))
        {
            target = hitInfo.collider.gameObject;
        }
        return new AimResult(target, aimStart, aimDir);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_debugAimStart, _debugAimStart + _debugAimDir * aimRange);
    }
}
