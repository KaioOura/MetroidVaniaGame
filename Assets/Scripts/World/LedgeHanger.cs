using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeHanger : MonoBehaviour
{
    [SerializeField] private Transform _hangPos;

    public Transform HangPos => _hangPos;  
}
