using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponDataScriptableObject", order = 1)]
public class WeaponData : ScriptableObject
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private AttackData[] _attackDatas;
    [SerializeField] private AttackData[] _airAttackDatas;

    public WeaponType WeaponType => _weaponType;
    public AttackData[] AttackDatas => _attackDatas;
    public AttackData[] AirAttackDatas => _airAttackDatas;

}

public enum WeaponType
{
    Melee,
    Ranged
}
