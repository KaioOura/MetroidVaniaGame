using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private SaveManager saveManager;

    private void Start()
    {
        saveManager.Initialize(character);
        character.Initialize();
        
        uiManager.InventoryUIController.Initialize(character.InventoryService);
        uiManager.CombatUIController.Initialize(character.CombatInventoryService);
        

    }
    
    
}
