using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Inventory
{
    public interface IInventoryService
    {
        List<InventoryItemSlot> ItemSlots { get; }

        bool TryAddItem(Item item, int quantity);
        void RemoveItemAt(int index, int quantity);
        void MoveItem(int fromIndex, int toIndex);
        void UseItem(int index, GameObject user);
        public event Action OnInventoryUpdated;
    }
}