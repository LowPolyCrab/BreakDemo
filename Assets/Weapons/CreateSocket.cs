using UnityEngine;

public interface ISocketInterface
{
    string GetSocketName();
    GameObject GetGameObject();
}

public class CreateSocket : MonoBehaviour
{
    [SerializeField] string SocketName;

    public bool IsForSocket(ISocketInterface socketInterface)
    {
        return SocketName == socketInterface.GetSocketName();
    }

    public void Attach(ISocketInterface socketInterface)
    {
        socketInterface.GetGameObject().transform.parent = transform.parent;
        socketInterface.GetGameObject().transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
