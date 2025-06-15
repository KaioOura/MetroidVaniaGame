using System;
using Systems.SaveLoad;
using UnityEngine;

namespace Systems
{
    public enum GameState
    {
        Gameplay,
        Inventory,
        Pause,
        Dialogue,
        Cutscene,
        Loading
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public event Action<GameState> OnGameStateChanged;
        public GameState CurrentState => _currentState;

        [SerializeField] private Character character;
        [SerializeField] private UiManager uiManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private InputReader inputReader;

        private GameState _currentState;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }

            Instance = this;
        }

        private void Start()
        {
            saveManager.Initialize(character);
            character.Initialize();

            uiManager.InventoryUI.Initialize(character.InventoryService, character.CombatInventoryService,
                uiManager.ToolTipUiController);

            OnGameStateChanged += inputReader.HandleStateChanged;
            inputReader.OnInventoryInput += uiManager.InventoryUI.HandleInventoryDisplay;

            SetState(GameState.Gameplay);
        }

        public void SetState(GameState newState)
        {
            if (_currentState == newState) return;

            _currentState = newState;
            OnGameStateChanged?.Invoke(_currentState);
        }

        public bool IsGameplay => _currentState == GameState.Gameplay;
    }
}