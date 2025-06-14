using System;
using UnityEngine;

public class InventoryItemSlot
{
    public Item Item { get; private set; }
    public int Quantity { get; private set; }
    public ItemType SlotType { get; private set; } = ItemType.None;

    public void AddNewItem(Item item)
    {
        Item = item;
        Quantity = Item.Quantity;
    }
    
    public void AddQuantity(int amount)
    {
        Quantity = Mathf.Min(Quantity + amount, Item.Data.MaxStack);
    }

    public void RemoveQuantity(int amount)
    {
        Quantity -= amount;
        if (Quantity <= 0) Quantity = 0;
    }

    public void RemoveItem()
    {
        Item = null;
    }

    public bool IsEmpty => Quantity <= 0;

    public void SetSlotType(ItemType slotType)
    {
        SlotType = slotType;
    }
}

