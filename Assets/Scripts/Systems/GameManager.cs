using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private UiManager uiManager;

    private void Start()
    {
        character.Initialize();
        
        uiManager.InventoryUIController.Initialize(character.InventoryService);
        uiManager.CombatUIController.Initialize(character.CombatInventoryService);
    }
    
    
}
