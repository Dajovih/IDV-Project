using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    Vector3 position;

    void Start()
    {
        position = transform.position;
    }

    void FixedUpdate()
    {   
        position.x += _speed / 100;
        transform.position = position;
    }
}
