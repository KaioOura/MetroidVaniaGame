using UnityEngine;

[System.Serializable]
public class SlotData
{
    public int slotIndex;
    public string itemDataName;
    public int quantity;

    public SlotData(int slotIndex, string itemDataName, int quantity)
    {
        this.slotIndex = slotIndex;
        this.itemDataName = itemDataName;
        this.quantity = quantity;
    }
}
