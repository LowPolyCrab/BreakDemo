using UnityEngine;

public abstract class Weapon : MonoBehaviour, ISocketInterface
{
    [SerializeField] string attachSocketName;

    public GameObject Owner
    {
        get; 
        private set; 
    }
    [SerializeField]

    public void Init(GameObject owner)
    {
        Owner = owner;
    }
    public void Equip()
    {
        gameObject.SetActive(true);
    }
    public void UnEquip()
    {
        gameObject.SetActive(false);
    }

    public string GetSocketName()
    {
        return attachSocketName;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
