using System;
using UnityEngine;

public class CombatInventoryService : InventoryService
{
    
    public virtual void Initialize(int capacity)
    {
        var headSlot = new InventoryItemSlot();
        headSlot.SetSlotType(ItemType.Equippable);
        var chestSlot = new InventoryItemSlot();
        chestSlot.SetSlotType(ItemType.Equippable);
        var bottomSlot = new InventoryItemSlot();
        bottomSlot.SetSlotType(ItemType.Equippable);
        var bootsSlot = new InventoryItemSlot();
        bootsSlot.SetSlotType(ItemType.Equippable);
        var weaponSlot = new InventoryItemSlot();
        weaponSlot.SetSlotType(ItemType.Equippable);
        
        _itemSlots.Add(headSlot);
        _itemSlots.Add(chestSlot);
        _itemSlots.Add(bottomSlot);
        _itemSlots.Add(bootsSlot);
        _itemSlots.Add(weaponSlot);
    }

    public override bool TryAddItem(Item item, int quantity)
    {
        for (int i = 0; i < _itemSlots.Count; i++)
        {
            if (_itemSlots[i].Item == null && _itemSlots[i].SlotType == item.Data.ItemType)
            {
                _itemSlots[i].AddNewItem(item);
                InventoryUpdate();;
                return true;
            }
        }

        return false;
    }
}
