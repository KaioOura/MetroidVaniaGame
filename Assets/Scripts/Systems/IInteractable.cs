using UnityEngine;

public interface IInteractable
{
    public void OnInteract(Transform transform);
    public Transform GetTransform();
    public void Select(); //TODO: Shader to outline the current selected
    public void Deselect(); 
    public int GetPriority();
}
