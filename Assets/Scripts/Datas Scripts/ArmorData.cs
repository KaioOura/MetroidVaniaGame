using UnityEngine;

[CreateAssetMenu(fileName = "ArmorData", menuName = "Scriptable Objects/ArmorData")]
public class ArmorData : EquipItemData
{
    public float DefenseValue => defenseValue;
    public ArmorType ArmorType => armorType;

    [SerializeField] private float defenseValue;
    [SerializeField] private ArmorType armorType;
}

public enum ArmorType
{
    Head,
    Chest,
    Bottom,
    Boots
}
