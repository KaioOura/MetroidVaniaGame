using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackData", menuName = "ScriptableObjects/RangedAttackDataScriptableObject", order = 1)]
public class RangedAttackData : ActionData
{
    [SerializeField] private FrameAction _shotFrame;

    public FrameAction ShotFrame => _shotFrame;

}
