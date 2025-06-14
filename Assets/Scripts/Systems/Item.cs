using UnityEngine;

public class Item
{
    public ItemData Data { get; private set; }
    
    private int _quantity;

    public int Quantity
    {
        get => Data.IsStackable ? _quantity : 1;
        private set => _quantity = value;
    }

    public Item(ItemData data, int quantity)
    {
        Data = data;
        Quantity = quantity;
    }
    
    public void AddQuantity(int amount)
    {
        Quantity = Mathf.Min(Quantity + amount, Data.MaxStack);
    }

    public void RemoveQuantity(int amount)
    {
        Quantity -= amount;
        if (Quantity <= 0) Quantity = 0;
    }

    public bool IsEmpty => Quantity <= 0;
}
