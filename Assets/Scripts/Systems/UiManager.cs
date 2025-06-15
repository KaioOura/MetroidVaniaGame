using UnityEngine;

//This class is going to centralize all UIs. Will be responsible to know when and which Uis should be displayed or not
namespace Systems
{
    public class UiManager : MonoBehaviour
    {
        [field: SerializeField] public InventoryUI InventoryUI { get; private set; }
        
        [field: SerializeField] public ToolTipUiController ToolTipUiController { get; private set; }
    }
}
