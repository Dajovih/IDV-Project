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
    private bool _canMove = true;

    private void Start() {
        _animator = GetComponent<Animator>();
        _body2D = GetComponent<Rigidbody2D>();
        GameEvents.onAttack += StopMoving;
    }

    private void OnDestroy()
    {
        GameEvents.onAttack -= StopMoving;
    }

    private void FixedUpdate() {
        if (_canMove)
        {
            _body2D.velocity = new Vector2(-1 * _speed, _body2D.velocity.y);
            _animator.SetFloat("Movement", Math.Abs(_body2D.velocity.x));
        }
    }

    private void StopMoving(float seconds)
    {
        StartCoroutine(StopCoroutine(seconds));
    }

    private IEnumerator StopCoroutine(float seconds)
    {
        _canMove = false;
        _body2D.velocity = Vector2.zero;
        _animator.SetFloat("Movement", 0);
        yield return new WaitForSeconds(seconds);
        _canMove = true;
    }
}
