using UnityEngine;
[ExecuteAlways]

public class ViewCamera : MonoBehaviour
{
    [SerializeField] private Transform pitchTransform;
    [SerializeField] private Camera viewCamera;
    [SerializeField] private float armLength = 7f;

    private Transform _parentTransform;

    public void SetFollowParent(Transform parentTransform)
    {
        _parentTransform = parentTransform;
    }

    private void Update()
    {
        viewCamera.transform.position = pitchTransform.position - viewCamera.transform.forward * armLength;
    }

    private void LateUpdate()
    {
        if (_parentTransform is null)
            return;
        transform.position = _parentTransform.position;
    }
}
