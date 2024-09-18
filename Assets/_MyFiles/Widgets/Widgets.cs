using UnityEngine;

public abstract class Widgets : MonoBehaviour
{
    private GameObject owner;

    public virtual void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }
}
