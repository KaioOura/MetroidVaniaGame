using UnityEngine;

public class CombatItemUseResolver
{
    
    private InventoryService _inventoryService;
    
    public void Initialize(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }
    public bool HandleItemUsed(Item item)
    {
        switch (item.Data) 
        {
            case ConsumableItemData:
                return false;
            case EquipItemData:
                return TryUnEquip(item);
        }

        return false;
    }

    private bool TryUnEquip(Item item)
    {
        return _inventoryService.TryAddItem(item, item.Quantity);
    }
}
