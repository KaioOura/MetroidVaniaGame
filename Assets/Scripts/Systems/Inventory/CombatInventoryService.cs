using System;
using UnityEngine;

public class CombatInventoryService : InventoryService
{
    public event Action<ItemData> OnEquip;
    public event Action<ItemData> OnUnEquip;
    
    private InventoryItemSlot _headSlot;
    private InventoryItemSlot _chestSlot;
    private InventoryItemSlot _bottomSlot;
    private InventoryItemSlot _bootsSlot;
    private InventoryItemSlot _meleeWeaponSlot;
    private InventoryItemSlot _rangedWeaponSlot;

    public virtual void Initialize(int capacity)
    {
        _headSlot = new InventoryItemSlot();
        _headSlot.SetSlotType(ItemType.Equippable);

        _chestSlot = new InventoryItemSlot();
        _chestSlot.SetSlotType(ItemType.Equippable);

        _bottomSlot = new InventoryItemSlot();
        _bottomSlot.SetSlotType(ItemType.Equippable);

        _bootsSlot = new InventoryItemSlot();
        _bootsSlot.SetSlotType(ItemType.Equippable);
        
        _meleeWeaponSlot = new InventoryItemSlot();
        _meleeWeaponSlot.SetSlotType(ItemType.Equippable);
        
        _rangedWeaponSlot = new InventoryItemSlot();
        _rangedWeaponSlot.SetSlotType(ItemType.Equippable);

        _itemSlots.Add(_headSlot);
        _itemSlots.Add(_chestSlot);
        _itemSlots.Add(_bottomSlot);
        _itemSlots.Add(_bootsSlot);
        _itemSlots.Add(_meleeWeaponSlot);
        _itemSlots.Add(_rangedWeaponSlot);
        
        LoadItems();
    }

    public override bool TryAddItem(Item item, int quantity)
    {
        if (item.Data.ItemType is not ItemType.Equippable) return false;

        return HandleItemSlotAllocation(item);
    }

    protected override void ForceItemAdd(Item item, InventoryItemSlot slot)
    {
        base.ForceItemAdd(item, slot);
        OnEquip?.Invoke(item.Data);
    }

    private bool HandleItemSlotAllocation(Item item)
    {
        InventoryItemSlot slot = null;
        bool success = false;

        switch (item.Data)
        {
            case WeaponData weaponData:
            {
                slot = weaponData.WeaponType is WeaponType.Melee ? _meleeWeaponSlot : _rangedWeaponSlot;
                break;
            }
            case ArmorData:
                HandleArmorSlotAllocation(item, out slot);
                break;
        }

        if (slot.Item == null)
        {
            slot.AddNewItem(item);
            OnEquip?.Invoke(item.Data);
            success = true;
        }

        InventoryUpdate();
        return success;
    }

    private void HandleArmorSlotAllocation(Item item, out InventoryItemSlot slot)
    {
        ArmorData armorData = item.Data as ArmorData;
        slot = null;
        
        switch (armorData.ArmorType)
        {
            case ArmorType.Head:
                slot = _headSlot;
                break;
            case ArmorType.Chest:
                slot = _chestSlot;
                break;
            case ArmorType.Bottom:
                slot = _bottomSlot;
                break;
            case ArmorType.Boots:
                slot = _bootsSlot;
                break;
        }
    }

    public override void RemoveItemAt(int index, int quantity)
    {
        if (index < 0 || index >= _itemSlots.Count || _itemSlots[index] == null) return;

        _itemSlots[index].RemoveQuantity(quantity);
        if (_itemSlots[index].IsEmpty)
        {
            OnUnEquip?.Invoke(_itemSlots[index].Item.Data);
            _itemSlots[index].RemoveItem();
        }

    }
}