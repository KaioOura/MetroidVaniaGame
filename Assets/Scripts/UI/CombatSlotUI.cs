using UnityEngine;
using UnityEngine.EventSystems;

public class CombatSlotUI : InventorySlotUI
{
    [SerializeField] private ItemType itemType;
    
    // public override void OnDrop(PointerEventData eventData)
    // {
    //     int fromIndex = InventoryDragHandler.Instance.DraggingIndex;
    //     
    //     if (fromIndex != _index)
    //     {
    //         InventoryUIController.Instance.SwapSlots(fromIndex, _index);
    //     }
    // }
}
