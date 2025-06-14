using UnityEngine;

[CreateAssetMenu(fileName = "ArmorData", menuName = "Scriptable Objects/ArmorData")]
public class ArmorData : EquipItemData
{
    public float DefenseValue => defenseValue;

    [SerializeField] private float defenseValue;
}
