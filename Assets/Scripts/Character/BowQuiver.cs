using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class BowQuiver : MonoBehaviour
{
    [SerializeField] private ArrowData _currentArrowData;
    [SerializeField] private float _arrowSpeed;
    [SerializeField] private Transform _shotPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShootArrowRequested()
    {
        //Prefab, speed, arrow, instatiate
        Arrow arrow = Instantiate(_currentArrowData.ArrowPrefab, _shotPos.position, transform.parent.rotation);
        arrow.InitArrow(_currentArrowData, _arrowSpeed);
        //Debug.Log("Arrow SHOT");
    }
}
