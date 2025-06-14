using System.Collections.Generic;
using Systems.Inventory;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    
    [SerializeField] private InventorySlotUI slotUIPrefab;
    [SerializeField] private Transform slotParent;

    private IInventoryService _inventory;
    private List<InventorySlotUI> _slots = new();
    
    public void Initialize(IInventoryService inventory)
    {
        _inventory = inventory;

        _inventory.OnInventoryUpdated += RefreshUI;

        for (int i = 0; i < _inventory.ItemSlots.Count; i++)
        {
            InventorySlotUI slotObj = Instantiate(slotUIPrefab, slotParent);
            slotObj.Initialize(this);
            slotObj.SetIndex(i);
            slotObj.OnSlotClicked += HandleSlotClick;
            _slots.Add(slotObj);
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].Render(_inventory.ItemSlots[i]);
        }
    }

    private void HandleSlotClick(int index)
    {
        _inventory.UseItem(index, gameObject);
        RefreshUI();
    }
    
    public void SwapSlots(int fromIndex, int toIndex)
    {
        _inventory.MoveItem(fromIndex, toIndex);
        RefreshUI();
    }
}