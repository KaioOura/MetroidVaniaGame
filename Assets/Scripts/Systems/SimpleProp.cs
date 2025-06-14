using UnityEngine;

public class SimpleProp : MonoBehaviour, IInteractable
{
    public void Deselect()
    {
        //print("Deselect: " + gameObject.name);
    }

    public int GetPriority()
    {
        return InteractionPriority.PROP;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnInteract(Transform transform)
    {
        print("Interaction!");
    }

    public void Select()
    {
        //print("Select: " + gameObject.name);
    }
}
