using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Systems.SaveLoad
{
    public class SaveManager : MonoBehaviour
    {
        private static string inventoryPath;
        private static string combatInventoryPath;

        private static InventoryService _inventoryService;
        private static CombatInventoryService _combatInventoryService;
        private static InventorySaver _inventorySaver = new InventorySaver();

        [ContextMenu("Save All")]
        public void Save()
        {
            SaveAll();
        }

        [ContextMenu("Clear Save")]
        public static void ClearSave()
        {
            if (File.Exists(inventoryPath))
                File.Delete(inventoryPath);
        
            if (File.Exists(combatInventoryPath))
                File.Delete(combatInventoryPath);
            
            
            Debug.Log("Data cleared");
        }

        public void Initialize(Character character)
        {
            _inventoryService = character.InventoryService;
            _combatInventoryService = character.CombatInventoryService;

            inventoryPath = Application.persistentDataPath + "/player/inventory";
            combatInventoryPath = Application.persistentDataPath + "/player/combat_inventory";
        }

        public static void SaveAll()
        {
            SaveInventory(_inventoryService, false);
            SaveInventory(_combatInventoryService, true);
            
            Debug.Log("Data saved");
        }

        private static void SaveInventory(InventoryService inventoryService, bool isCombatInventory)
        {
            var path = isCombatInventory ? combatInventoryPath : inventoryPath;

            _inventorySaver.SaveInventory(inventoryService, path);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            throw new NotImplementedException();
        }

        public static List<SlotData> LoadInventory(bool isCombatInventory)
        {
            var path = isCombatInventory ? combatInventoryPath : inventoryPath;
            return _inventorySaver.LoadInventory(path);
        }
    }
}