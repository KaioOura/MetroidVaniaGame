using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LedgeData", menuName = "ScriptableObjects/LedgeDataScriptableObject", order = 1)]
public class LedgeData : ScriptableObject
{
    [SerializeField] private LayerMask _ledgeCheckLayerMask;
    [SerializeField] private float _checkDistance = 0.5f;
    [SerializeField] private float _hangCoolDown = 0.15f;
    [SerializeField] private Vector2 _offSet = new Vector2(-0.18f, -1.55f);

    public LayerMask LedgeCheckLayerMask => _ledgeCheckLayerMask;
    public float CheckDistance => _checkDistance;
    public float HangCoolDown => _hangCoolDown;
    public Vector2 OffSet => _offSet;
 
}
