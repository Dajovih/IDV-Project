using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Parametros de salto y velocidad. Ajustarlo en interfaz
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Vector3 _boxDimensions;
    //[SerializeField] private Transform _feet;


    //Elementos propios del objeto obtenidos en el start
    private Rigidbody2D _body;
    private Animator _animator;
    private Transform _feet;


    private LayerMask _layerGround;
    private bool _inGround;
    private bool _jump;
    //private bool _canMove = true;
    private float _direction;
    private float _time = 0;
    private float _wait = 0;
    private bool _stop = false;
    


    private void Start() {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _feet = transform.Find("Feet"); //Encuentra el elemento hijo feet, el cual es necesario para el salto
        _layerGround = LayerMask.GetMask("Ground"); //Obtiene la Layer de ground, necesaria para el salto
        GameEvents.OnEnemyAttack += StopMoving;
    }

    private void OnDestroy()
    {
        GameEvents.OnEnemyAttack -= StopMoving;
    }

    private void Update() {
        _time+= Time.deltaTime;
        if (_time >= _wait)
        {
            _direction = Input.GetAxis("Horizontal"); //Obtener la direccion hacia la cual se esta movimendo : (1,-1)
            _animator.SetFloat("Movement", MathF.Abs(_direction));  //Guarda la direccion en el parametro movement del animator. Siempre van a ser valor mayores a 0
            if (Input.GetButtonDown("Jump"))
            {
                _jump = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_time >= _wait)
        {
            Move();
        }
            _inGround = Physics2D.OverlapBox(_feet.position, _boxDimensions, 0f, _layerGround); //Verifica si los pies del jugador están sobre el suelo
            _animator.SetBool("InGround", _inGround);   //Establece el parámetro InGround del animator en true o false según inGround
            Jump();
            _jump = false;
        
    }

    private void Move()
    {
        _body.velocity = new Vector2(_direction * _speed, _body.velocity.y);    //Movimientos hechos a traves del rigidbody
        if (_direction > 0f)    //Voltear al jugador segun la direcci�n hacia la cual se est� moviendo
        { 
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
            _body.velocity = new Vector2(_body.velocity.x, _jumpForce); //Cambia la coordenada en y segun jumpForce
            _inGround = false;
            AudioManager.Instance.PlaySound2D("JumpSFX");
        }
    }

    private void StopMoving(float seconds)
    {
        _time = 0;
        _wait = seconds;
        _body.velocity = Vector2.zero;
        _direction = 0;
        _animator.SetFloat("Movement", 0);
        _animator.SetBool("InGround", true);
    }


    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_feet.position, _boxDimensions);
    }*/
}
