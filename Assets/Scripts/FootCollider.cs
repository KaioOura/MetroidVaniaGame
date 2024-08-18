using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCollider : MonoBehaviour
{

    [SerializeField] private LayerMask _groundLayer;
    public bool IsOnGround; //{ get; private set; }

    public float FootRadius;

    public event Action<bool> OnIsOnGroundUpdate;

    // Start is called before the first frame update
    void Start()
    {
        FootRadius = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGroundCollision();
    }

    void CheckGroundCollision()
    {
        IsOnGround = Physics2D.OverlapCircle(transform.position, FootRadius, _groundLayer);
        OnIsOnGroundUpdate?.Invoke(IsOnGround);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, FootRadius);
    }

}
