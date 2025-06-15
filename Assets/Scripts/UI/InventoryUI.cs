using System;
using Systems;
using Systems.Inventory;
using UnityEngine;

public class InventoryUI : HudBase
{
    [field: SerializeField] public InventoryUIController InventoryUIController { get; private set; }
    [field: SerializeField] public InventoryUIController CombatUIController { get; private set; }

    private bool _isOpen;
    private ToolTipUiController _toolTipUi;
    private void Awake()
    {
        _isOpen = gameObject.activeSelf;
    }

    public void Initialize(IInventoryService inventoryService, IInventoryService combatInventoryService,
        ToolTipUiController toolTipUiController)
    {
        InventoryUIController.Initialize(inventoryService, toolTipUiController);
        CombatUIController.Initialize(combatInventoryService, toolTipUiController);

        _toolTipUi = toolTipUiController;
    }

    public void HandleInventoryDisplay(bool isPressing)
    {
        bool shouldOpen = !_isOpen;

        if (shouldOpen)
        {
            Show();
            GameManager.Instance.SetState(GameState.Inventory);
        }
        else
        {
            Hide();
            GameManager.Instance.SetState(GameState.Gameplay);
            
            _toolTipUi.HideTooltip();
        }

        _isOpen = !_isOpen;
    }
}