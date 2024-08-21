using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColliderCreator : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private CircleCollider2D _circleCollider;
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnReceivedColliderRequest(ColliderInfos colliderInfos)
    {

        ResetColliders();
        
        switch (colliderInfos.ColliderTypes)
        {
            case ColliderTypes.BoxCollider:
            {   
                _boxCollider.gameObject.SetActive(colliderInfos.IsActive);
                if (!colliderInfos.IsActive)
                    return;

                _boxCollider.transform.localPosition = colliderInfos.Pos;
                _boxCollider.transform.localScale = colliderInfos.Scale;
                _boxCollider.transform.localEulerAngles = 
                new Vector3(_boxCollider.transform.localEulerAngles.x, _boxCollider.transform.localEulerAngles.y, colliderInfos.Rot);
                break;
            }
            case ColliderTypes.SphereCollider:
            {
                break;
            }
            case ColliderTypes.CapsuleCollider:
            {
                break;
            }
        }
    }

    public void ResetColliders()
    {
        _boxCollider.gameObject.SetActive(false);
        // _boxCollider.gameObject.SetActive(false);
        // _boxCollider.gameObject.SetActive(false);
    }
}
