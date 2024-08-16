using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _ledgeLayerMask;
    [SerializeField] private Transform _ledgeTransformDetection;
    [SerializeField] private float _ledgeRadiusDetection;

    [SerializeField] private LayerMask _preLedgelayerMask;
    [SerializeField] private Transform _preLedgeTransformDetection;
    [SerializeField] private Vector3 _preLedgeSizeDetection;

    public event Action<bool> OnLedgeDetected;

    
    void Update()
    {
        bool detectionPreLedge = Physics2D.OverlapBox((Vector2)_preLedgeTransformDetection.position, _preLedgeSizeDetection, 0, _preLedgelayerMask);
        bool detectionLedge = Physics2D.OverlapCircle((Vector2)_ledgeTransformDetection.position, _ledgeRadiusDetection, _ledgeLayerMask) && detectionPreLedge == false;

        OnLedgeDetected?.Invoke(detectionLedge);
        // Debug.Log($"Detection: {detectionLedge}");
        // Debug.Log($"DetectionPreLedge: {detectionPreLedge}");
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)_preLedgeTransformDetection.position, _preLedgeSizeDetection);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_ledgeTransformDetection.position, _ledgeRadiusDetection);
    }
}
