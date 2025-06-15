using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class InventorySaver
{
    public void SaveInventory(InventoryService inventoryService, string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        
        List<SlotData> slotDatas = new List<SlotData>();
        
        for (var index = 0; index < inventoryService.ItemSlots.Count; index++)
        {
            var inventoryItemSlot = inventoryService.ItemSlots[index];
            var itemDataName = inventoryItemSlot.Item == null
                ? ""
                : inventoryItemSlot.Item.Data.ItemName;
            
            SlotData data = new SlotData(index, itemDataName, inventoryItemSlot.Quantity);
            slotDatas.Add(data);
        }
        
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, slotDatas);
        stream.Close();
        
    }

    public List<SlotData> LoadInventory(string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            List<SlotData> slotDatas = (List<SlotData>)formatter.Deserialize(stream);
            stream.Close();
            return slotDatas;
        }
        else
        {
            Debug.LogError("Save File not found in" + path);
            return null;
        }
    }
}
