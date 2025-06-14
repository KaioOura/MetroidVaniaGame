using UnityEngine;

//This class is going to centralize all UIs. Will be responsible to know when and which Uis should be displayed or not
public class UiManager : MonoBehaviour
{
    [field: SerializeField] public InventoryUIController InventoryUIController { get; private set; }
    [field: SerializeField] public InventoryUIController CombatUIController { get; private set; }
}
