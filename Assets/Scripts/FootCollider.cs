using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCollider : MonoBehaviour
{

    [SerializeField] private LayerMask _groundLayer;
    public bool IsOnGround; //{ get; private set; }

    public float FootRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGroundCollision();
    }

    void CheckGroundCollision()
    {
        IsOnGround = Physics2D.OverlapCircle(transform.position, FootRadius, _groundLayer);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, FootRadius);
    }

}
