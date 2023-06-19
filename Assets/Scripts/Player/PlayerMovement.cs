using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
       
    //Par�metros de salto y velocidad. Ajustarlo en interfaz
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Animator _animator;


    private Rigidbody2D _body;
    private bool _inGround; //Para verificar que solo puede saltar si se encuentra en el suelo

    private void Start() {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        float direction = Input.GetAxis("Horizontal"); //Obtener la direccion hacia la cual se est� movimendo : (1,-1)
        _animator.SetFloat("Movement", MathF.Abs(direction)); 
        _body.velocity = new Vector2(_speed * direction, _body.velocity.y); //Aplicar desplazamiento en la coordenada x teniendo en cuenta la velocidad y direcci�n
        if (direction > 0f) { //Voltear al jugador seg�n la direcci�n hacia la cual se est� moviendo
            transform.localScale = Vector3.one;
        } else if (direction < 0f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        if (Input.GetKey(KeyCode.Space) && _inGround) {  //Salto
            Jump();
        }
        
    }

    private void Jump() {
        _body.velocity = new Vector2(_body.velocity.x, _jumpForce); //Cambia la coordenada en y seg�n jumpForce
        _inGround = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) { 
        if (collision.gameObject.tag == "Ground") {
            _inGround = true;
        }

    }
}
