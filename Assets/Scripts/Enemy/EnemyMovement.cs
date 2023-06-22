using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    //Parametros de salto y velocidad. Ajustarlo en interfaz
    [SerializeField] private float _speed;

    //Elementos propios del objeto obtenidos en el start
    private Animator _animator;
    private Rigidbody2D _body2D;
    //private bool _canMove = true;
    private float _time = 0f;
    private float _wait = 0f;

    private void Start() {
        _animator = GetComponent<Animator>();
        _body2D = GetComponent<Rigidbody2D>();
        GameEvents.OnEnemyAttack += StopMoving;
    }

    private void OnDestroy()
    {
        GameEvents.OnEnemyAttack -= StopMoving;
    }

    private void FixedUpdate() {
        _time += Time.deltaTime;
        if (_time >= _wait)
        {
            _body2D.velocity = new Vector2(-1 * _speed, _body2D.velocity.y);
            _animator.SetFloat("Movement", Math.Abs(_body2D.velocity.x));
        } else
        {
            _body2D.velocity = Vector2.zero;
        }
    }

    private void StopMoving(float seconds)
    {
        _time = 0;
        _wait = seconds;
        _body2D.velocity = Vector2.zero;
        _animator.SetFloat("Movement", 0);
        

    }
    
}
