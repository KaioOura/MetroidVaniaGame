using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Inventory;
using UnityEngine;

public class InventoryService : MonoBehaviour, IInventoryService
{
    public event Action OnInventoryUpdated;
    public List<InventoryItemSlot> ItemSlots => _itemSlots;

    [SerializeField] private ItemDataBase _itemDataBase;
    protected List<InventoryItemSlot> _itemSlots = new List<InventoryItemSlot>();
    private int _capacity = 5;

    bool _isCombatInventory;
    
    public event Func<Item, bool> OnItemUse;
    
    [ContextMenu("Load Inventory")]
    public void LoadItemsList()
    {
        LoadItems();
        
        foreach (var itemSlot in _itemSlots)
        {
            if (itemSlot.Item == null) continue;
            print(itemSlot.Item.Data.ItemName);
        }
    }

    protected void Awake()
    {
        _isCombatInventory = this is CombatInventoryService;
    }

    public virtual void Initialize(int capacity)
    {
        _capacity = capacity;
        for (int i = 0; i < capacity; i++)
        {
            _itemSlots.Add(new InventoryItemSlot());
        }

        LoadItems();
    }

    protected void LoadItems()
    {
        var savedSlots = SaveManager.LoadInventory(isCombatInventory: _isCombatInventory);

        if (savedSlots == null || savedSlots.Count <=0) return;
        
        for (int i = 0; i < _itemSlots.Count; i++)
        {
            if (savedSlots[i].itemDataName == "") continue;
            
            ItemData itemData = _itemDataBase.ItemsDatas.FirstOrDefault(id => id.ItemName == savedSlots[i].itemDataName);
            
            if (itemData == null) continue;
            
            Item item = new Item(itemData, savedSlots[i].quantity);

            _itemSlots[i].AddNewItem(item);
        }
        
        InventoryUpdate();
    }

    public virtual bool TryAddItem(Item item, int quantity)
    {
        // Stack first
        if (item.Data.IsStackable)
        {
            for (int i = 0; i < _itemSlots.Count; i++)
            {
                var itemTemp = _itemSlots[i];
                if (itemTemp != null && itemTemp.Item == item && itemTemp.Quantity < item.Data.MaxStack)
                {
                    itemTemp.AddQuantity(quantity);
                    InventoryUpdate();
                    return true;
                }
            }
        }

        // Find empty slot
        for (int i = 0; i < _itemSlots.Count; i++)
        {
            if (_itemSlots[i].Item == null || _itemSlots[i].IsEmpty)
            {
                _itemSlots[i].AddNewItem(item);
                InventoryUpdate();
                return true;
            }
        }

        return false;
    }

    public virtual void RemoveItemAt(int index, int quantity)
    {
        if (index < 0 || index >= _itemSlots.Count || _itemSlots[index] == null) return;

        _itemSlots[index].RemoveQuantity(quantity);
        if (_itemSlots[index].IsEmpty)
            _itemSlots[index].RemoveItem();
    }

    public void MoveItem(int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex) return;
        if (!IsValidIndex(fromIndex) || !IsValidIndex(toIndex)) return;

        var from = _itemSlots[fromIndex];
        var to = _itemSlots[toIndex];

        _itemSlots[fromIndex] = to;
        _itemSlots[toIndex] = from;
    }

    public virtual void UseItem(int index, GameObject user)
    {
        if (!IsValidIndex(index)) return;

        bool usedSuccessfully = OnItemUse?.Invoke(_itemSlots[index].Item) ?? false;

        if (usedSuccessfully)
            RemoveItemAt(index, 1);
    }

    protected bool IsValidIndex(int index) => index >= 0 && index < _itemSlots.Count;

    public void InventoryUpdate()
    {
        OnInventoryUpdated?.Invoke();
    }
}