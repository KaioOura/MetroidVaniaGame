using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Scriptable Objects/ConsumableItemData")]
public class ConsumableItemData : ItemData
{
    public float Value => value;

    public ConsumableType ConsumableType => consumableType;

    [SerializeField] private float value;
    [SerializeField] private ConsumableType consumableType;
}

public enum ConsumableType
{
    Healing,
    Mana
}
