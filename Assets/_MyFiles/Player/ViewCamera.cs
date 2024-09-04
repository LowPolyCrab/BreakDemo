using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
[ExecuteAlways]

public class ViewCamera : MonoBehaviour
{
    [SerializeField] private Transform pitchTransform;
    [SerializeField] private Camera viewCamera;
    [SerializeField] private float armLength = 7f;
    [SerializeField] private float cameraTurnSpeed = 30f;

    private Transform _parentTransform;

    public Camera GetViewCamera()
    {
        return viewCamera; 
    }

    Vector3 GetViewRightDir()
    {
        return viewCamera.transform.right;
    }

    Vector3 GetViewUpDir()
    {
        return Vector3.Cross(GetViewRightDir(), Vector3.up);
    }

    public Vector3 InputToWorldDir(Vector2 input)
    {
        return GetViewRightDir() * input.x + GetViewUpDir() * input.y;
    }

    public void SetFollowParent(Transform parentTransform)
    {
        _parentTransform = parentTransform;
    }

    public void AddYawInput(float amt)
    {
        transform.Rotate(Vector3.up, amt * cameraTurnSpeed * Time.deltaTime);
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
