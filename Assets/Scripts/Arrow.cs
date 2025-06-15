using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;

    private DamageInfos _damageInfos;

    public void InitArrow(ArrowData arrowData, float speed)
    {
        _damageInfos = arrowData.DamageInfos;
        
        _rb.AddForceX(speed * transform.right.x, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        Destroy(gameObject);
    }
}
