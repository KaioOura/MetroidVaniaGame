using System;
using UnityEngine;

public class InventoryItemUseResolver
{
    //Only equips can be used in inventory
    //TODO: Logic to use consumables  through inventory UI

    private CombatInventoryService _combatInventory;
    
    public void Initialize(CombatInventoryService combatInventoryService)
    {
        _combatInventory = combatInventoryService;
    }
    
    public bool HandleItemUsed(Item item)
    {
        switch (item.Data) 
        {
            case ConsumableItemData:
                return false;
            case EquipItemData:
                return TryEquip(item);
        }

        return false;
    }

    private bool TryEquip(Item item)
    {
        EquipItemData equipItemData = item.Data as EquipItemData;
        
        if (equipItemData == null) return false;
        
        return _combatInventory.TryAddItem(item, item.Quantity);
    }
}
