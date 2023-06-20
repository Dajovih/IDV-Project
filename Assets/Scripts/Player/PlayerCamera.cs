using UnityEngine;
using System;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private Vector3 _position;
    
    private float _distanceFromPlayer;
    

    void Start()
    {
        _position = transform.position;
        _distanceFromPlayer = Math.Abs(_position.y - _player.transform.position.y);
    }

    void FixedUpdate()
    {   
        _position.y = _player.transform.position.y + (_distanceFromPlayer / 2);
        transform.position = _position;
    }
}
