using System.Collections.Generic;
using Systems.Inventory;
using UnityEngine;

public class CombatUIController : InventoryUIController
{
    public override void Initialize(IInventoryService inventory, ToolTipUiController tooltipController)
    {
        _inventory = inventory;
        _tooltipController = tooltipController;
        _inventory.OnInventoryUpdated += RefreshUI;

        var slotsUIs = slotParent.GetComponentsInChildren<CombatSlotUI>();

        for (int i = 0; i < slotsUIs.Length; i++)
        {
            slotsUIs[i].SetIndex(i);
            slotsUIs[i].Initialize(this);
            slotsUIs[i].OnSlotClicked += HandleSlotClick;
            _slots.Add(slotsUIs[i]);
        }

        RefreshUI();
    }
}
