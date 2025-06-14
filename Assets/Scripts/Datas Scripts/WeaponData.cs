using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponDataScriptableObject", order = 1)]
public class WeaponData : EquipItemData
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private ActionData[] _actionAttackDatas;
    [SerializeField] private ActionData[] _actionAirAttackDatas;

    public WeaponType WeaponType => _weaponType;
    public ActionData[] ActionAttackDatas => _actionAttackDatas;
    public ActionData[] ActionAirAttackDatas => _actionAirAttackDatas;

}

public enum WeaponType
{
    Melee,
    Ranged
}
