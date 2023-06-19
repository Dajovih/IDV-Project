using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Par�metros de salto y velocidad. Ajustarlo en interfaz
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _feet;
    [SerializeField] private Vector3 _boxDimensions;
    [SerializeField] private LayerMask _layerGround;
    [SerializeField] private bool _inGround;


    private Rigidbody2D _body;
    private Animator _animator;
    //private bool _inGround; //Para verificar que solo puede saltar si se encuentra en el suelo
    private bool _jump = false;
    private float _direction;

    private void Start() {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        _direction = Input.GetAxis("Horizontal"); //Obtener la direccion hacia la cual se est� movimendo : (1,-1)
        _animator.SetFloat("Movement", MathF.Abs(_direction));
        if (Input.GetButtonDown("Jump")) {  //Salto
            _jump = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
        _inGround = Physics2D.OverlapBox(_feet.position, _boxDimensions, 0f, _layerGround);
        _animator.SetBool("InGround", _inGround);
        Jump();
        _jump = false;

    }

    private void Move()
    {
        _body.velocity = new Vector2(_direction * _speed, _body.velocity.y);
        if (_direction > 0f)
        { //Voltear al jugador seg�n la direcci�n hacia la cual se est� moviendo
            transform.localScale = Vector3.one;
        }
        else if (_direction < 0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Jump() {
        if (_inGround && _jump)
        {
            _body.velocity = new Vector2(_body.velocity.x, _jumpForce); //Cambia la coordenada en y seg�n jumpForce
            _inGround = false;
        }
    }
}
