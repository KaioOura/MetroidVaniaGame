using System.Collections.Generic;
using Systems.Inventory;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public ToolTipUiController ToolTipUiController => _tooltipController;
    
    [SerializeField] protected InventorySlotUI slotUIPrefab;
    [SerializeField] protected Transform slotParent;

    protected IInventoryService _inventory;
    protected List<InventorySlotUI> _slots = new();
    
    protected ToolTipUiController _tooltipController;
    
    public virtual void Initialize(IInventoryService inventory, ToolTipUiController tooltipController)
    {
        _inventory = inventory;
        
        _tooltipController = tooltipController;
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

    protected void RefreshUI()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].Render(_inventory.ItemSlots[i]);
        }
    }

    protected void HandleSlotClick(int index)
    {
        _inventory.UseItem(index, gameObject);
        RefreshUI();
        _tooltipController.HideTooltip();
    }
    
    public void SwapSlots(int fromIndex, int toIndex)
    {
        _inventory.MoveItem(fromIndex, toIndex);
        RefreshUI();
    }
}