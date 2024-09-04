using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ArrowData", menuName = "ScriptableObjects/ArrowDataScriptableObject", order = 1)]
public class ArrowData : ScriptableObject
{
    [SerializeField] private DamageInfos _damageInfos;
    [SerializeField] private Arrow _arrowPrefab;

    public Arrow ArrowPrefab => _arrowPrefab;
    public DamageInfos DamageInfos => _damageInfos;
}

