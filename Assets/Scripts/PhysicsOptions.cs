using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PhysicsOptions
{
    [SerializeField] private bool _stopHorizontalVelocity;
    [SerializeField] private bool _stopVerticalVelocity;
    [SerializeField] private bool _updateRigidBodyContraints;
    [SerializeField] private RigidbodyConstraints2D _rigidBodyConstraints;


    public bool StopHorizontalVelocity => _stopHorizontalVelocity;
    public bool StopVerticalVelocity => _stopVerticalVelocity;

    public bool UpdateRigidBodyContraints => _updateRigidBodyContraints;
    public RigidbodyConstraints2D RigidbodyConstraints => _rigidBodyConstraints;
}
